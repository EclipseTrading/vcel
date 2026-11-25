using System;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;

namespace VCEL.Test;

public class CSharpConditionalCompatibilityTests
{
    [TestCase("match | true=(1==1)",
        "((true) ? (TypeOperation.EqualsChecked(1, 1)) :  default)")]
    [TestCase("true ? int(1==1) : 'other'",
        "(((bool?)(true) ?? false) ? VcelType.Integer(TypeOperation.EqualsChecked(1, 1)) : \"other\")")]
    public void ValueBoundaryCodeMatchesSnapshot(string text, string expected)
    {
        var parsed = CSharpExpression.ParseCode(text);
        Assert.That(parsed.Success, Is.True);
        Assert.That(parsed.Expression.Evaluate(new object()), Is.EqualTo(expected));
    }

    public sealed class Row
    {
        public object? Flag { get; set; }
        public object? Code { get; set; } = "A";
        public int Reads { get; private set; }
        public object Fail { get { Reads++; throw new InvalidOperationException("unselected"); } }
    }

    [TestCase("let x=Flag ? Code=='A' : null in x", true, true)]
    [TestCase("let x=Flag ? Code=='A' : null in x", false, null)]
    [TestCase("let x=Flag ? Code!='A' : null in x", true, false)]
    [TestCase("let x=Flag ? Code!='A' : null in x", false, null)]
    [TestCase("let x=Flag ? null : Code=='A' in x", false, true)]
    [TestCase("let x=Flag ? null : Code!='A' in x", false, false)]
    [TestCase("let x=Flag ? (Code=='A') : 'other' in x", true, true)]
    [TestCase("let x=Flag ? (Code=='A') : 'other' in x", false, "other")]
    [TestCase("let x=Flag ? Code!='A' : 1 in x", false, 1)]
    [TestCase("let x=Flag ? 1 : Code!='A' in x", false, false)]
    [TestCase("let x=Flag ? Code=='A' : false in !x", true, false)]
    [TestCase("let x=Flag ? Code=='A' : null in x==null", false, true)]
    [TestCase("let x=Flag ? Code=='A' : null in x ? 1 : 2", true, 1)]
    [TestCase("let x=Flag ? Code=='A' : null in [x,x]", true, null)]
    [TestCase("let x=Flag ? (Flag ? Code=='A' : null) : 'other' in x", true, true)]
    [TestCase("let equal=Code=='A',x=Flag ? equal : null in x", true, true)]
    [TestCase("let equal=Code!='A',x=Flag ? equal : 'other' in x", false, "other")]
    [TestCase("let x=Flag ? !(Code=='A') : null in x", true, false)]
    [TestCase("let equal=!(Code!='A'),x=Flag ? equal : null in x", true, true)]
    [TestCase("match | Flag=(Code=='A')", false, null)]
    [TestCase("match | Flag=(Code=='A')", true, true)]
    [TestCase("match | Flag=(Code!='A')", false, null)]
    [TestCase("match | Flag=(Code!='A')", true, false)]
    [TestCase("let x=(match | Flag=(Code=='A') | otherwise null) in x", false, null)]
    [TestCase("let x=(match | Flag=(Code=='A') | otherwise null) in x", true, true)]
    [TestCase("let x=(match | Flag=(Code!='A') | otherwise null) in x", true, false)]
    [TestCase("let x=(match | Flag=null | otherwise (Code=='A')) in x", false, true)]
    [TestCase("let x=(match | Flag=null | otherwise (Code!='A')) in x", false, false)]
    [TestCase("match | Flag=(Code=='A') | Code=='B'=(Code!='A')", false, null)]
    [TestCase("match | Flag=1", false, 0)]
    [TestCase("match | Flag=true", false, false)]
    [TestCase("let x=Flag ? int(Code=='A') : 'other' in x", true, 1)]
    [TestCase("let x=Flag ? int(Code=='A') : 'other' in x", false, "other")]
    [TestCase("let x=Flag ? 'other' : int(Code!='A') in x", false, 0)]
    [TestCase("let x=Flag ? bool(Code=='A') : 'other' in x", true, true)]
    [TestCase("let x=Flag ? bool(Code!='A') : 'other' in x", true, false)]
    [TestCase("let x=Flag ? double(Code=='A') : 'other' in x", true, 1d)]
    [TestCase("let x=Flag ? long(Code=='A') : 'other' in x", true, 1L)]
    [TestCase("let x=Flag ? int(bool(Code=='A')) : 'other' in x", true, 1)]
    [TestCase("let x=int(Code=='A'),y=Flag ? x : 'other' in y", true, 1)]
    [TestCase("let x=(match | Flag=(int(Code=='A')) | otherwise 'other') in x", true, 1)]
    [TestCase("let x=(match | Flag=(int(Code=='A')) | otherwise 'other') in x", false, "other")]
    [TestCase("let x=Flag ? bool(!(Code=='A')) : 'other' in x", true, false)]
    public void ConditionalLetPreservesBranchTypes(string text, bool flag, object? expected)
    {
        var result = Compile(text)(new Row { Flag = flag });
        if (text.Contains("[x,x]", StringComparison.Ordinal))
        {
            var items = (System.Collections.Generic.List<object>)result!;
            Assert.That(items[0], Is.SameAs(items[1]));
        }
        else
        {
            Assert.That(result, Is.EqualTo(expected));
            Assert.That(result?.GetType(), Is.EqualTo(expected?.GetType()));
        }
    }

    [Test]
    public void ConditionalDoesNotEvaluateUnselectedBranch()
    {
        var row = new Row { Flag = true };
        Assert.That(Compile("let x=Flag ? Code=='A' : Fail in x")(row), Is.True);
        Assert.That(row.Reads, Is.Zero);
    }

    [TestCase("match | false=(Fail=='A')")]
    [TestCase("match | false=(int(Fail=='A'))")]
    [TestCase("let x=Flag ? int(Code=='A') : int(Fail=='A') in x")]
    public void ValueBoundariesDoNotEvaluateUnselectedResults(string text)
    {
        var row = new Row { Flag = true };
        var result = Compile(text)(row);
        Assert.That(result, Is.EqualTo(text.StartsWith("let", StringComparison.Ordinal) ? 1 : null));
        Assert.That(row.Reads, Is.Zero);
    }

    [TestCase("match | Flag=(Code=='A')", false, 0)]
    [TestCase("match | Flag=(Code=='A')", true, 24)]
    [TestCase("match | Flag=(Code!='A')", false, 0)]
    [TestCase("match | Flag=(Code!='A')", true, 48)]
    public void MatchOnlyAllocatesSelectedBoolean(string text, bool flag, int expectedBytes)
    {
        var evaluate = Compile(text);
        var row = new Row { Flag = flag };
        AssertAllocation(evaluate, row, expectedBytes);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ConditionalOnlyAllocatesSelectedBoolean(bool flag)
    {
        var evaluate = Compile("let x=Flag ? Code=='A' : null in x");
        var row = new Row { Flag = flag };
        AssertAllocation(evaluate, row, flag ? 24 : 0);
    }

    [TestCase("Code=='A' ? 1 : 2")]
    [TestCase("(Code=='A' && Code!='B') ? 1 : 2")]
    [TestCase("(Code=='B' || Code=='A') ? 1 : 2")]
    [TestCase("!(Code!='A') ? 1 : 2")]
    [TestCase("match | !(Code!='A')=1 | otherwise 2")]
    public void BooleanConditionsDoNotBoxIntermediateEqualities(string text)
    {
        var evaluate = Compile(text);
        var row = new Row();
        Assert.That(evaluate(row), Is.EqualTo(1));
        AssertAllocation(evaluate, row, 24);
    }

    private static void AssertAllocation(Func<object, object?> evaluate, Row row, int expectedBytes)
    {
        for (var i = 0; i < 1000; i++)
        {
            evaluate(row);
        }

        const int count = 10000;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < count; i++)
        {
            evaluate(row);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        TestContext.WriteLine($"{allocated / (double)count} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo((long)expectedBytes * count + 64));
    }

    private static Func<object, object?> Compile(string text)
    {
        var parsed = CSharpExpression.ParseCode(text);
        Assert.That(parsed.Success, Is.True);
        var code = parsed.Expression.Evaluate(new object());
        var (type, errors) = CodeGenCSharpClass.Generate("ConditionalCompatibility", code);
        Assert.That(type, Is.Not.Null, () => $"{code}\n{string.Join(Environment.NewLine, errors!.Diagnostics)}");
        return type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
    }
}
