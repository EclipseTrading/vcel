using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;
using VCEL.Expression;

namespace VCEL.Test;

public class CSharpStaticBindingTests
{
    public sealed class Row
    {
        public object? Value { get; set; }
        public object? Other { get; set; }
    }

    public sealed class ObservedRow(params object?[] values)
    {
        public List<string> Reads { get; } = new();
        public object? Value
        {
            get
            {
                Reads.Add("Value");
                return values[Reads.Count - 1];
            }
        }

        public object Other { get { Reads.Add("Other"); return 2d; } }
        public object Fail { get { Reads.Add("Fail"); throw new InvalidOperationException("operand"); } }
    }

    public sealed class ComparableValue(int value) : IComparable
    {
        private int Value { get; } = value;
        public List<object?> Comparisons { get; } = new();
        public int CompareTo(object? obj)
        {
            Comparisons.Add(obj);
            return Value.CompareTo(((ComparableValue)obj!).Value);
        }
    }

    [TestCase("Value in {'A','B'}",
        "((vcelContext.Value) is string ? str_set_ID.Contains((vcelContext.Value).ToString()) : false)",
        "private static readonly HashSet<string> str_set_ID = [\"A\",\"B\"];")]
    [TestCase("Value in {1,2}",
        "(CSharpHelper.IsNumber((object)(vcelContext.Value)) ? numeric_set_ID.Contains((double)(vcelContext.Value)) : false)",
        "private static readonly HashSet<double> numeric_set_ID = [1d,2d];")]
    public void AdvancedSetCodeSnapshot(string text, string expected, string member)
    {
        var (code, members) = Generate(text);
        Assert.That(NormalizeSetNames(code), Is.EqualTo(expected));
        Assert.That(members.Select(NormalizeSetNames), Does.Contain(member));
    }

    [TestCase("<")]
    [TestCase("<=")]
    [TestCase(">")]
    [TestCase(">=")]
    public void ComparisonCodeSnapshot(string operation)
    {
        var (code, _) = Generate($"Value{operation}Other");
        Assert.That(code, Is.EqualTo(
            $"(CSharpHelper.UpCastCompare((CSharpHelper.IsNumber((object)((vcelContext.Value))) ? ((double)((vcelContext.Value))) : ((vcelContext.Value))), (CSharpHelper.IsNumber((object)((vcelContext.Other))) ? ((double)((vcelContext.Other))) : ((vcelContext.Other))), \"{operation}\"))"));
    }

    [TestCase(1, "missing", 0)]
    [TestCase(128, "missing", 0)]
    [TestCase(128, "127", 24)]
    [TestCase(128, null, 0)]
    public void StringSetMatchOnlyAllocatesItsResult(int count, string? value, int expectedBytes)
    {
        var text = "match " + string.Join(" ", Enumerable.Range(0, count)
            .Select(i => $"| Value in {{'{i}'}}={i + 1}")) + " | otherwise null";
        var evaluate = Compile(text);
        var row = new Row { Value = value };
        Assert.That(evaluate(row), Is.EqualTo(value == "127" ? 128 : null));
        AssertAllocation(() => evaluate(row), expectedBytes);
    }

    [TestCase("Value in {'A','B'}", "B", true)]
    [TestCase("Value in {'A','B'}", "missing", false)]
    [TestCase("Value in {'A','B'}", null, false)]
    [TestCase("Value in {1,2,3}", 2d, true)]
    [TestCase("Value in {1,2,3}", 2, true)]
    [TestCase("Value in {1,2,3}", 2L, true)]
    [TestCase("Value in {1,2,3}", 2f, true)]
    [TestCase("Value in {1,2,3}", "2", false)]
    [TestCase("Value in {1,2,3}", null, false)]
    [TestCase("Value in {1,2,3}", double.NaN, false)]
    [TestCase("Value in {1,2,3}", double.PositiveInfinity, false)]
    [TestCase("match | Value in {1,2,3}=1 | Value in {4,5,6}=2 | otherwise 0", 10d, 0)]
    [TestCase("Value<Other", 1d, true)]
    [TestCase("Value<=Other", 2d, true)]
    [TestCase("Value>Other", 3d, true)]
    [TestCase("Value>=Other", 2d, true)]
    public void ValueCallsPreserveResultsAndAllocationBounds(string text, object? value, object expected)
    {
        var evaluate = Compile(text);
        var legacy = Compile(text, legacy: true);
        var row = new Row { Value = value, Other = 2d };
        Assert.That(evaluate(row), Is.EqualTo(expected));
        Assert.That(evaluate(row), Is.EqualTo(legacy(row)));
        Assert.That(evaluate(row)?.GetType(), Is.EqualTo(legacy(row)?.GetType()));
        var maximumBytes = text.Contains("Other", StringComparison.Ordinal) ? 120 : 24;
        AssertAllocation(() => evaluate(row), maximumBytes);
    }

    [TestCase("Value<Other ? 1 : 2", 1d, 1, 72)]
    [TestCase("!(Value>=Other) ? 1 : 2", 1d, 1, 72)]
    [TestCase("Value in {'A','B'} ? 'yes' : 'no'", "A", "yes", 0)]
    [TestCase("true and Value in {'A','B'} ? 'yes' : 'no'", "A", "yes", 0)]
    public void BooleanConditionsRetainStaticBindingSavings(string text, object value, object expected, int maximumBytes)
    {
        var evaluate = Compile(text);
        var row = new Row { Value = value, Other = 2d };
        Assert.That(evaluate(row), Is.EqualTo(expected).And.TypeOf(expected.GetType()));
        AssertAllocation(() => evaluate(row), maximumBytes);
    }

    [Test]
    public void NumericSetsPreserveDecimalPromotion()
    {
        Assert.That(Compile("Value in {1,2,3}")(new Row { Value = 2m }), Is.True);
    }

    [TestCase("Value in {'A','B'}", "A", "B", true)]
    [TestCase("Value in {1,2}", 1d, 2d, true)]
    [TestCase("Value in {'A','B'}", null, null, false)]
    [TestCase("Value in {1,2}", null, null, false)]
    public void SetsPreserveChangingGetterReadCounts(string text, object? first, object? second, bool expected)
    {
        foreach (var legacy in new[] { true, false })
        {
            var row = new ObservedRow(first, second);
            Assert.That(Compile(text, legacy)(row), Is.EqualTo(expected));
            Assert.That(row.Reads, Has.Count.EqualTo(first is null ? 1 : 2));
        }
    }

    [TestCase("Value in {'A','B'}", "A", null)]
    [TestCase("Value in {1,2}", 1d, "invalid")]
    [TestCase("Value in {1,2}", 1d, null)]
    public void SetsPreserveChangingGetterErrors(string text, object first, object? second)
    {
        var previous = Assert.Catch(() => Compile(text, legacy: true)(new ObservedRow(first, second)));
        var current = Assert.Catch(() => Compile(text)(new ObservedRow(first, second)));
        Assert.That(current!.GetType(), Is.EqualTo(previous!.GetType()));
    }

    [TestCase("<")]
    [TestCase("<=")]
    [TestCase(">")]
    [TestCase(">=")]
    public void ComparisonsPreserveTypesNullsAndExceptions(string operation)
    {
        var current = Compile($"Value{operation}Other");
        var previous = Compile($"Value{operation}Other", legacy: true);
        var values = new object?[] { null, 1, 2L, 3f, 4d, 5m, "A", "B", true,
            double.NaN, double.PositiveInfinity, new object(),
            new DateTime(2026, 1, 1), TimeSpan.FromSeconds(1) };
        foreach (var left in values)
        {
            foreach (var right in values)
            {
                var row = new Row { Value = left, Other = right };
                object? expected;
                try
                {
                    expected = previous(row);
                }
                catch (Exception error)
                {
                    Assert.That(Assert.Catch(() => current(row))!.GetType(), Is.EqualTo(error.GetType()));
                    continue;
                }

                Assert.That(current(row), Is.EqualTo(expected), $"{left} {operation} {right}");
            }
        }
    }

    [Test]
    public void ComparisonsKeepCustomComparableDispatch()
    {
        var left = new ComparableValue(1);
        var right = new ComparableValue(2);
        Assert.That(Compile("Value<Other")(new Row { Value = left, Other = right }), Is.True);
        Assert.That(left.Comparisons, Is.EqualTo(new[] { right }));
    }

    [Test]
    public void ComparisonsPreserveGetterOrderAndExceptionShortCircuiting()
    {
        foreach (var legacy in new[] { true, false })
        {
            var row = new ObservedRow(1d, 1d);
            Assert.That(Compile("Value<Other", legacy)(row), Is.True);
            Assert.That(row.Reads, Is.EqualTo(new[] { "Value", "Value", "Other", "Other" }));
            row.Reads.Clear();
            Assert.Throws<InvalidOperationException>(() => Compile("Fail<Other", legacy)(row));
            Assert.That(row.Reads, Is.EqualTo(new[] { "Fail" }));
        }
    }

    private static string NormalizeSetNames(string code)
        => Regex.Replace(code, @"(str_set_|numeric_set_)\d+", "${1}ID");

    private static (string Code, string[] Members) Generate(string text)
    {
        var parser = new ExpressionParser<string>(new AdvanceToCSharpCodeFactory(ConcatStringMonad.Instance));
        var parsed = parser.Parse(text);
        Assert.That(parsed.Success, Is.True, text);
        return (parsed.Expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext)),
            parsed.Expression.Dependencies.OfType<CSharpMemberDependency>().Select(d => d.Declaration).ToArray());
    }

    private static Func<object, object?> Compile(string text, bool legacy = false)
    {
        var (code, members) = Generate(text);
        if (legacy)
        {
            // Recreate the pre-fix calls to compare behavior on the same inputs.
            code = code.Replace(".Contains((string)", ".Contains(", StringComparison.Ordinal)
                .Replace("CSharpHelper.IsNumber((object)(vcelContext.", "CSharpHelper.IsNumber((vcelContext.", StringComparison.Ordinal);
            var syntax = SyntaxFactory.ParseExpression(code);
            var comparisons = syntax.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString() == "CSharpHelper.UpCastCompare");
            code = syntax.ReplaceNodes(comparisons, (_, rewritten) =>
                rewritten.WithArgumentList(rewritten.ArgumentList.WithArguments(
                    SyntaxFactory.SeparatedList(rewritten.ArgumentList.Arguments.Select(arg =>
                        arg.Expression is CastExpressionSyntax cast
                            ? arg.WithExpression(cast.Expression) : arg))))).ToFullString();
        }

        var (type, errors) = CodeGenCSharpClass.Generate("StaticBindingTest", code, members);
        Assert.That(type, Is.Not.Null, () => $"{code}\n{string.Join(Environment.NewLine, errors!.Diagnostics)}");
        return type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
    }

    private static void AssertAllocation(Func<object?> evaluate, int maximumBytes)
    {
        for (var i = 0; i < 1000; i++)
        {
            evaluate();
        }

        const int iterations = 10000;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < iterations; i++)
        {
            evaluate();
        }

        var bytes = GC.GetAllocatedBytesForCurrentThread() - before;
        TestContext.WriteLine($"{bytes / (double)iterations} bytes/evaluation");
        Assert.That(bytes, Is.LessThanOrEqualTo((long)maximumBytes * iterations + 64),
            "Allow a fixed measurement overhead, not an extra allocation per evaluation.");
    }
}
