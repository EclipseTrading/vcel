using NUnit.Framework;
using System;
using VCEL;
using VCEL.Core.Lang;

namespace VECL.Test
{
    public class LegacyFunctionTests
    {
        [TestCase("T(System.Math).Abs(-1)", 1)]
        public void EvalLegacyFunction(string exprString, object expected)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("T(System.DateTime).Today")]
        [TestCase("(T(System.DateTime).Today)")]
        public void EvalLegacyToday(string exprString)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today));
        }

        [TestCase("T(System.DateTime).Today.Day")]
        [TestCase("(T(System.DateTime).Today).Day")]
        public void EvalLegacyTodayDay(string exprString)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today.Day));
        }
    }
}
