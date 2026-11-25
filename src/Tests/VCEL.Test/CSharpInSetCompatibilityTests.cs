using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;
using VCEL.Expression;

namespace VCEL.Test;

[SetCulture("en-US")]
public class CSharpInSetCompatibilityTests
{
    public sealed class Row(params object?[] values)
    {
        public int Reads { get; private set; }
        public object? Code => Reads < values.Length
            ? values[Reads++]
            : throw new InvalidOperationException("unexpected read");
    }

    public sealed class ComparedValue(object failure)
    {
        public List<object?> Comparisons { get; } = [];
        public override bool Equals(object? value)
        {
            Comparisons.Add(value);
            if (Equals(value, failure))
                throw new InvalidOperationException("equality");
            return false;
        }
        public override int GetHashCode() => 0;
    }

    [TestCase(false)]
    [TestCase(true)]
    public void EmptyDirectSetDoesNotReadTheOperand(bool advanced)
    {
        var row = new Row();
        Assert.That(Compile(advanced, new HashSet<object>())(row), Is.False);
        Assert.That(row.Reads, Is.Zero);
    }

    [TestCase(false, false)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    public void DirectSetPreservesCustomEqualityOrderAndExceptions(bool advanced, bool numeric)
    {
        var values = Values(numeric);
        var compared = new ComparedValue(values.ElementAt(1));
        var row = new Row(Enumerable.Repeat<object?>(compared, 6).ToArray());
        var exception = Assert.Throws<InvalidOperationException>(() => Compile(advanced, values)(row));
        Assert.That(exception!.Message, Is.EqualTo("equality"));
        Assert.That(compared.Comparisons, Is.EqualTo(values.Take(2)));
        Assert.That(row.Reads, Is.EqualTo(4));
    }

    [TestCase(false, false, false)]
    [TestCase(false, false, true)]
    [TestCase(false, true, false)]
    [TestCase(false, true, true)]
    [TestCase(true, false, false)]
    [TestCase(true, false, true)]
    [TestCase(true, true, false)]
    [TestCase(true, true, true)]
    public void DirectSetPreservesRepeatedReadsAndShortCircuiting(bool advanced, bool numeric, bool match)
    {
        var values = Values(numeric);
        var missing = numeric ? (object)9d : "missing";
        var second = numeric ? (object)2d : "B";
        var row = match
            ? new Row(missing, missing, second, second)
            : new Row(Enumerable.Repeat(missing, 6).ToArray());
        Assert.That(Compile(advanced, values)(row), Is.EqualTo(match).And.TypeOf<bool>());
        Assert.That(row.Reads, Is.EqualTo(match ? 4 : 6));
    }

    [TestCase(false)]
    [TestCase(true)]
    public void DirectSetStopsAtAThrowingGetter(bool advanced)
    {
        var row = new Row("missing", "missing");
        var exception = Assert.Throws<InvalidOperationException>(() => Compile(advanced, Values(false))(row));
        Assert.That(exception!.Message, Is.EqualTo("unexpected read"));
        Assert.That(row.Reads, Is.EqualTo(2));
    }

    [TestCase(0, "")]
    [TestCase(1, "\"A\",\"B\",\"C\"")]
    [TestCase(2, "1,2,3")]
    public void AdvancedDirectSetCodeMatchesPrePr104Snapshot(int kind, string literals)
    {
        var (code, members) = Generate(true, kind == 0 ? new HashSet<object>() : Values(kind == 2));
        var name = members.Single().Name;
        Assert.That(code.Replace(name, "snapshot_set"),
            Is.EqualTo("snapshot_set.Any(elem => TypeOperation.EqualsChecked((CSharpHelper.IsNumber((object)((vcelContext.Code))) ? ((double)((vcelContext.Code))) : ((vcelContext.Code))), elem))"));
        Assert.That(members.Single().Declaration.Replace(name, "snapshot_set"),
            Is.EqualTo($"private static readonly HashSet<object> snapshot_set = [{literals}];"));
    }

    private static HashSet<object> Values(bool numeric)
        => numeric ? [1, 2, 3] : ["A", "B", "C"];

    private static (string Code, CSharpMemberDependency[] Members) Generate(bool advanced, ISet<object> values)
    {
        ToCSharpCodeFactory factory = advanced
            ? new AdvanceToCSharpCodeFactory(ConcatStringMonad.Instance)
            : new ToCSharpCodeFactory(ConcatStringMonad.Instance);
        var expression = factory.InSet(factory.Property("Code"), values);
        return (expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, "vcelContext")),
            expression.Dependencies.OfType<CSharpMemberDependency>().ToArray());
    }

    private static Func<object, object?> Compile(bool advanced, ISet<object> values)
    {
        var (code, members) = Generate(advanced, values);
        var (type, errors) = CodeGenCSharpClass.Generate("InSetCompatibility", code,
            members.Select(member => member.Declaration).ToArray());
        Assert.That(type, Is.Not.Null, () => $"{code}\n{string.Join(Environment.NewLine, errors!.Diagnostics)}");
        return type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
    }
}
