using System;
using System.Linq;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;

namespace VCEL.Test;

public class CSharpMatchAllocationTests
{
    public sealed class Row
    {
        public string? Code { get; set; } = "";
    }

    [TestCase(1, -1, false, true)]
    [TestCase(128, -1, false, true)]
    [TestCase(128, 0, false, true)]
    [TestCase(128, 127, false, true)]
    [TestCase(128, -1, true, true)]
    [TestCase(128, 127, true, true)]
    [TestCase(128, -1, false, false)]
    [TestCase(128, 127, false, false)]
    [TestCase(128, -2, false, false)]
    public void MatchDoesNotBoxEachEqualityCondition(
        int clauseCount, int matchingIndex, bool useNotEqual, bool includeOtherwise)
    {
        var text = "match " + string.Join(" ", Enumerable.Range(0, clauseCount)
            .Select(i => useNotEqual
                ? $"| !(Code != '{i}') = {i + 1}"
                : $"| Code == '{i}' = {i + 1}")) + (includeOtherwise ? " | otherwise 0" : "");
        var parsed = CSharpExpression.ParseCode(text);
        Assert.That(parsed.Success, Is.True);
        var code = parsed.Expression.Evaluate(new object());
        var (type, errors) = CodeGenCSharpClass.Generate("MatchAllocation", code);
        Assert.That(errors, Is.Null, () => string.Join(Environment.NewLine, errors!.Diagnostics));
        Assert.That(type, Is.Not.Null);
        var evaluate = type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
        var row = new Row { Code = matchingIndex == -2 ? null : matchingIndex.ToString() };

        for (var i = 0; i < 100; i++)
        {
            evaluate(row);
        }

        const int iterations = 1000;
        object? result = null;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < iterations; i++)
        {
            result = evaluate(row);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.That(result, Is.EqualTo(Math.Max(0, matchingIndex + 1)));
        TestContext.WriteLine($"{clauseCount} clauses, match {matchingIndex}: {allocated / iterations} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo(32L * iterations),
            "Only the numeric result may be boxed, not each Boolean condition.");
    }
}
