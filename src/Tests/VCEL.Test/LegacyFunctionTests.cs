using NUnit.Framework;
using System;
using VCEL;
using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;

namespace VECL.Test
{
    public class LegacyFunctionTests
    {
        [TestCase("-T(System.Math).Round(GetValue('parameter_volatility', 0.3) + GetValue('parameter_volatility', 0.7), 2)", -1)]
        public void EvalLegacyMathFunction(string exprString, object expected)
        {
            var funcs = new DefaultFunctions<object>();
            funcs.Register("GetValue", (a, b) => Convert.ToDouble(a[1]));
            var expr = VCExpression.ParseDefault(exprString, funcs).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("T(System.Math).Abs(-1)", 1)]
        [TestCase("-T(System.Math).Abs(-1)", -1)]
        [TestCase("-1 + -T(System.Math).Abs(-1)", -2)]
        [TestCase("-T(System.Math).Abs(-1) + -1", -2)]
        [TestCase("-T(System.Math).Abs(-1) + -T(System.Math).Abs(-1)", -2)]
        public void EvalLegacyFunction(string exprString, object expected)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("T(System.DateTime).Today")]
        [TestCase("(T(System.DateTime).Today)")]
        [TestCase("T(DateTime).Today")]
        [TestCase("(T(DateTime).Today)")]
        public void EvalLegacyToday(string exprString)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today));
        }

        [TestCase("T(System.DateTime).Today.Day")]
        [TestCase("(T(System.DateTime).Today).Day")]
        [TestCase("T(DateTime).Today.Day")]
        [TestCase("(T(DateTime).Today).Day")]
        public void EvalLegacyTodayDay(string exprString)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTime.Today.Day));
        }

        [TestCase("(@2020-10-10 - @2020-10-09).TotalDays")]
        public void EvalTotalDays(string exprString)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
