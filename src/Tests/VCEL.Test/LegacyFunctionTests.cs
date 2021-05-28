using System;
using NUnit.Framework;
using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.CSharp.Expression.Func;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class LegacyFunctionTests
    {
        [TestCase("-T(System.Math).Round(GetValue('parameter_volatility', 0.3) + GetValue('parameter_volatility', 0.7), 2)", -1)]
        public void EvalLegacyMathFunction(string exprString, object expected)
        {
            var funcs = new DefaultFunctions<object>();
            funcs.Register("GetValue", (a, b) => Convert.ToDouble(a[1]));
            var parseResult = VCExpression.ParseDefault(exprString, funcs);
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));

            var funcs2 = new DefaultCSharpFunctions();
            funcs2.Register("GetValue", (a, b) => $"Convert.ToDouble({a[1]})");
            var parseResult2 = CSharpExpression.ParseNative(exprString, funcs2);
            var expr2 = parseResult2.Expression;
            var result2 = expr2.Evaluate(new { });
            Assert.That(result2, Is.EqualTo(expected));
        }

        [TestCase("T(System.Math).Abs(-1)", 1)]
        [TestCase("-T(System.Math).Abs(-1)", -1)]
        [TestCase("-1 + -T(System.Math).Abs(-1)", -2)]
        [TestCase("-T(System.Math).Abs(-1) + -1", -2)]
        [TestCase("-T(System.Math).Abs(-1) + -T(System.Math).Abs(-1)", -2)]
        public void EvalLegacyFunction(string exprString, object expected)
        {
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var result = expr.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }

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
}