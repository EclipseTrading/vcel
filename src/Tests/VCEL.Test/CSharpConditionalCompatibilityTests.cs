using System;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;

namespace VCEL.Test;

public class CSharpConditionalCompatibilityTests
{
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

    [TestCase(true)]
    [TestCase(false)]
    public void ConditionalOnlyAllocatesSelectedBoolean(bool flag)
    {
        var evaluate = Compile("let x=Flag ? Code=='A' : null in x");
        var row = new Row { Flag = flag };
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
        TestContext.WriteLine($"{flag}: {allocated / (double)count} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo((flag ? 24L : 0L) * count + 64));
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
