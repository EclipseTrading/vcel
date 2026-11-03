using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;

namespace VCEL.Test;

public class CSharpCollectionAllocationTests
{
    public sealed class Row
    {
        public object? A { get; set; }
        public object? B { get; set; }
        public object? Code { get; set; }
    }

    public sealed class ObservedRow
    {
        public List<string> Reads { get; } = new();
        public object A { get { Reads.Add("A"); return 1d; } }
        public object B { get { Reads.Add("B"); return "second"; } }
        public object Fail { get { Reads.Add("Fail"); throw new InvalidOperationException("operand"); } }
        public IEnumerable<object> Items
        {
            get
            {
                Reads.Add("Items");
                return Enumerate();
            }
        }

        private IEnumerable<object> Enumerate()
        {
            Reads.Add("Enumerate");
            yield return "spread";
        }
    }

    public sealed class MembershipRow(params object?[] values)
    {
        public int Reads { get; private set; }
        public object? Code
        {
            get
            {
                if (Reads >= values.Length)
                {
                    throw new InvalidOperationException("unexpected read");
                }

                return values[Reads++];
            }
        }
    }

    public sealed class ComparedValue
    {
        public List<object?> Comparisons { get; } = new();
        public override bool Equals(object? obj)
        {
            Comparisons.Add(obj);
            if (Equals(obj, "B"))
            {
                throw new InvalidOperationException("equality");
            }

            return false;
        }

        public override int GetHashCode() => 0;
    }

    [TestCase("Code in {'A','B','C'}", "A", true, 2)]
    [TestCase("Code in {'A','B','C'}", "C", true, 6)]
    [TestCase("Code in {'A','B','C'}", "missing", false, 6)]
    [TestCase("Code in {'A','A','B'}", "B", true, 4)]
    [TestCase("Code in {}", "unused", false, 0)]
    public void SetMembershipPreservesGetterCountsAndShortCircuiting(
        string text, string value, bool expected, int reads)
    {
        var row = new MembershipRow(Enumerable.Repeat<object?>(value, reads).ToArray());
        Assert.That(Compile(text)(row), Is.EqualTo(expected).And.TypeOf<bool>());
        Assert.That(row.Reads, Is.EqualTo(reads));
    }

    [Test]
    public void SetMembershipReevaluatesTheValueForEachVisitedElement()
    {
        var row = new MembershipRow("missing", "missing", "B", "B");
        Assert.That(Compile("Code in {'A','B','C'}")(row), Is.True);
        Assert.That(row.Reads, Is.EqualTo(4));
    }

    [Test]
    public void SetMembershipPreservesCustomEqualityOrderAndExceptions()
    {
        var value = new ComparedValue();
        Assert.Throws<InvalidOperationException>(() =>
            Compile("Code in {'A','B','C'}")(new Row { Code = value }));
        Assert.That(value.Comparisons, Is.EqualTo(new object[] { "A", "B" }));
    }

    [TestCase("Code in {1,2,3}", 1d, true)]
    [TestCase("Code in {1,2,3}", 1L, true)]
    [TestCase("Code in {1,2,3}", 1f, true)]
    [TestCase("Code in {1,2,3}", "1", false)]
    [TestCase("Code in {1,'2',3}", "2", true)]
    [TestCase("Code in {1,'2',3}", 2d, false)]
    [TestCase("Code in {1.5,2.5}", 1.5d, true)]
    [TestCase("Code in {1,2}", double.NaN, false)]
    [TestCase("Code in {1,2}", double.PositiveInfinity, false)]
    [TestCase("Code in {'A','B'}", null, false)]
    [TestCase("Code in {null}", null, false)]
    [TestCase("Code in {null}", "null", true)]
    public void SetMembershipPreservesExistingLiteralAndMixedTypeEquality(
        string text, object? value, bool expected)
    {
        Assert.That(Compile(text)(new Row { Code = value }), Is.EqualTo(expected).And.TypeOf<bool>());
    }

    [Test]
    public void SetMembershipStillPromotesDecimalProperties()
    {
        Assert.That(Compile("Code in {1,2,3}")(new Row { Code = 2m }), Is.True);
    }

    [TestCase("Code in {1,1.0,1L}")]
    [TestCase("Code in {null,'null'}")]
    public void SetMembershipDeduplicatesRenderedLiteralsBeforeReadingTheValue(string text)
    {
        var row = new MembershipRow("missing", "missing");
        Assert.That(Compile(text)(row), Is.False);
        Assert.That(row.Reads, Is.EqualTo(2));
    }

    [TestCase(@"Code in {'A','\u0041'}", "A", true)]
    [TestCase(@"Code in {'A','\U00000041'}", "missing", false)]
    [TestCase(@"Code in {'A','\x41'}", "A", true)]
    [TestCase(@"Code in {'\u0041','\x41'}", "missing", false)]
    [TestCase(@"Code in {'CAT','C\u0041T'}", "CAT", true)]
    [TestCase(@"Code in {'CAT','C\u0041T'}", "DOG", false)]
    [TestCase(@"Code in {'\u0041','B'}", "A", true)]
    [TestCase(@"Code in {'\u0041','B'}", "B", true)]
    [TestCase(@"Code in {'\u0041','B'}", "missing", false)]
    [TestCase(@"Code in {'\\','B'}", "\\", true)]
    public void EscapedSetMembershipPreservesCompiledStringValues(string text, string value, bool expected)
    {
        var evaluate = Compile(text);
        var row = new Row { Code = value };
        Assert.That(evaluate(row), Is.EqualTo(expected).And.TypeOf<bool>());
        Assert.That(AllocatedPerEvaluation(() => evaluate(row)), Is.LessThanOrEqualTo(24));
    }

    [TestCase("true and Code in {1,2}", 1, true)]
    [TestCase("true and Code in {1,2}", 3, false)]
    [TestCase("false or Code in {1,2}", 2, true)]
    [TestCase("Code in {1,2} ? 'yes' : 'no'", 1, "yes")]
    [TestCase("match | Code in {1,2}=1 | otherwise 2", 3, 2)]
    [TestCase("true and Code in {'A','B'}", "A", true)]
    [TestCase("true and Code in {'A','B'}", "missing", false)]
    [TestCase("false or Code in {'A','B'}", "B", true)]
    [TestCase("Code in {'A','B'} ? 'yes' : 'no'", "A", "yes")]
    [TestCase("match | Code in {'A','B'}=1 | otherwise 2", "missing", 2)]
    [TestCase("true and Code in {1,2}", null, false)]
    [TestCase("true and Code in {'A','B'}", null, false)]
    [TestCase("true and Code in {1,2}", "1", false)]
    [TestCase("true and Code in {'A','B'}", 1, false)]
    public void MemberFactorySetLiteralsPreserveComposition(string text, object? value, object expected)
    {
        var parsed = CSharpExpression.ParseMethodWithMembers(text);
        Assert.That(parsed.Success, Is.True,
            () => string.Join(Environment.NewLine, parsed.ParseErrors.Select(error => error.Message)));
        var result = parsed.Expression.Evaluate(new Row { Code = value });
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(result?.GetType(), Is.EqualTo(expected.GetType()));
    }

    [Test]
    public void ListMembershipStillBuildsAllElementsBeforeReadingTheValue()
    {
        var row = new ObservedRow();
        Assert.That(Compile("B in [B,A]")(row), Is.True);
        Assert.That(row.Reads, Is.EqualTo(new[] { "B", "B", "A", "A", "B", "B" }));
    }

    [TestCase("(Code in {'A','B'})?1:2", "B", 1)]
    [TestCase("match | Code in {'A','B'}=1 | otherwise 2", "C", 2)]
    [TestCase("let x=Code in (x in {'A','B'})", "A", true)]
    [TestCase("Code in {'A','B'} and Code!='C'", "A", true)]
    public void SetMembershipComposesWithOtherExpressions(string text, string value, object expected)
    {
        Assert.That(Compile(text)(new Row { Code = value }), Is.EqualTo(expected));
    }

    [Test]
    public void PlainListsPreserveOrderNullsDuplicatesAndNumericPromotion()
    {
        var marker = new object();
        var row = new Row { A = marker, B = 2m };
        var list = (List<object?>)Compile("[A,null,B,A,[B]]")(row)!;
        Assert.That(list, Has.Count.EqualTo(5));
        Assert.That(list[0], Is.SameAs(marker));
        Assert.That(list[1], Is.Null);
        Assert.That(list[2], Is.EqualTo(2d).And.TypeOf<double>());
        Assert.That(list[3], Is.SameAs(marker));
        Assert.That(list[4], Is.TypeOf<List<object>>().And.EqualTo(new object[] { 2d }));
        list.Add("mutable");
        Assert.That(list, Has.Count.EqualTo(6));
    }

    [Test]
    public void PlainListsPreserveGetterOrderAndExceptionShortCircuiting()
    {
        var row = new ObservedRow();
        Assert.That(Compile("[A,B,A]")(row), Is.EqualTo(new object[] { 1d, "second", 1d }));
        Assert.That(row.Reads, Is.EqualTo(new[] { "A", "A", "B", "B", "A", "A" }));
        row.Reads.Clear();
        Assert.Throws<InvalidOperationException>(() => Compile("[B,Fail,A]")(row));
        Assert.That(row.Reads, Is.EqualTo(new[] { "B", "B", "Fail" }));
    }

    [Test]
    public void SpreadListsStillEvaluateLaterItemsBeforeEnumeratingSpreads()
    {
        var row = new ObservedRow();
        Assert.That(Compile("[...Items,B]")(row), Is.EqualTo(new object[] { "spread", "second" }));
        Assert.That(row.Reads, Is.EqualTo(new[] { "Items", "Items", "B", "B", "Enumerate" }));
    }

    [TestCase(0, -1)]
    [TestCase(3, -1)]
    [TestCase(3, 0)]
    [TestCase(3, 2)]
    [TestCase(32, -1)]
    [TestCase(128, 127)]
    [TestCase(512, -1)]
    public void StringSetMembershipOnlyAllocatesTheReturnedBoolean(int size, int match)
    {
        var text = $"Code in {{{string.Join(",", Enumerable.Range(0, size).Select(i => $"'ITEM_{i}'"))}}}";
        var evaluate = Compile(text);
        var row = new Row { Code = match < 0 ? "missing" : $"ITEM_{match}" };
        Assert.That(evaluate(row), Is.EqualTo(match >= 0).And.TypeOf<bool>());
        var allocated = AllocatedPerEvaluation(() => evaluate(row));
        TestContext.WriteLine($"{size} elements, match {match}: {allocated} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo(24));
    }

    [TestCase("[]", 0)]
    [TestCase("[A]", 1)]
    [TestCase("[A,B,'text']", 3)]
    public void PlainListsOnlyAllocateTheReturnedListAndStorage(string text, int count)
    {
        var evaluate = Compile(text);
        var row = new Row { A = "first", B = "second" };
        var expected = new List<object?>(count);
        for (var i = 0; i < count; i++)
        {
            expected.Add(i == 0 ? row.A : i == 1 ? row.B : "text");
        }

        var actual = evaluate(row);
        Assert.That(actual, Is.TypeOf<List<object>>().And.EqualTo(expected));
        var expectedAllocation = AllocatedPerEvaluation(() => new List<object>(count));
        var allocated = AllocatedPerEvaluation(() => evaluate(row));
        TestContext.WriteLine($"{text}: {allocated} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo(expectedAllocation));
        Assert.That(evaluate(row), Is.Not.SameAs(actual));
    }

    [Test]
    public void StandaloneSetsKeepTheirMutableHashSetResult()
    {
        var evaluate = Compile("{'A','B','A'}");
        var first = evaluate(new Row());
        Assert.That(first, Is.TypeOf<HashSet<object>>().And.EquivalentTo(new[] { "A", "B" }));
        ((HashSet<object>)first!).Add("changed");
        Assert.That(evaluate(new Row()), Is.EquivalentTo(new[] { "A", "B" }).And.Not.SameAs(first));
    }

    [Test]
    public void MembershipInSpreadCollectionsKeepsMixedTypeEquality()
    {
        var row = new Row { A = new object?[] { null, 2, "text" }, Code = 2m };
        var evaluate = Compile("Code in [...A]");
        Assert.That(evaluate(row), Is.True);
        row.Code = null;
        Assert.That(evaluate(row), Is.True);
        row.Code = "missing";
        Assert.That(evaluate(row), Is.False);
    }

    private static double AllocatedPerEvaluation(Func<object?> evaluate)
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

        return (GC.GetAllocatedBytesForCurrentThread() - before) / (double)iterations;
    }

    private static Func<object, object?> Compile(string text)
    {
        var parsed = CSharpExpression.ParseCode(text);
        Assert.That(parsed.Success, Is.True, text);
        var code = parsed.Expression.Evaluate(new object());
        var (type, errors) = CodeGenCSharpClass.Generate("CollectionTest", code);
        Assert.That(errors, Is.Null, () => $"{code}\n{string.Join(Environment.NewLine, errors!.Diagnostics)}");
        return type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
    }
}