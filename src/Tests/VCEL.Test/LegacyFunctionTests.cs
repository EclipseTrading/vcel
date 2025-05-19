using NUnit.Framework;
using System;
using VCEL.Test.Shared;

namespace VCEL.Test;

public class LegacyFunctionTests
{
    [TestCase("T(System.DateTime).Today")]
    [TestCase("(T(System.DateTime).Today)")]
    [TestCase("T(DateTime).Today")]
    [TestCase("(T(DateTime).Today)")]
    public void EvalLegacyToday(string exprString)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today));
        }
    }

    [TestCase("T(System.DateTime).Today.Day")]
    [TestCase("(T(System.DateTime).Today).Day")]
    [TestCase("T(DateTime).Today.Day")]
    [TestCase("(T(DateTime).Today).Day")]
    public void EvalLegacyTodayDay(string exprString)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today.Day));
        }
    }

    [TestCase("(@2020-10-10 - @2020-10-09).TotalDays")]
    public void EvalTotalDays(string exprString)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(1));
        }
    }
}