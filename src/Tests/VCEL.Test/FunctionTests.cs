using System;
using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class FunctionTests
    {
        [TestCase("abs(1)", 1)]
        [TestCase("abs(-1)", 1)]
        [TestCase("abs(1.1)", 1.1)]
        [TestCase("abs(-1.1)", 1.1)]
        [TestCase("acos(0.5)", 1.0471975511965979)]
        [TestCase("asin(0.5)", 0.52359877559829893)]
        [TestCase("atan(0.5)", 0.46364760900080609)]
        [TestCase("atan2(1, 0.5)", 1.1071487177940904)]
        [TestCase("ceiling(1.1)", 2)]
        [TestCase("cos(0.5)", 0.87758256189037276)]
        [TestCase("cosh(0.5)", 1.1276259652063807)]
        [TestCase("exp(0.5)", 1.6487212707001282)]
        [TestCase("floor(0.5)", 0)]
        [TestCase("floor(-0.5)", -1)]
        [TestCase("log(0.5)", -0.69314718055994529)]
        [TestCase("log10(0.5)", -0.3010299956639812)]
        [TestCase("min(4, 3, 7)", 3)]
        [TestCase("min(4.5, 3.1, 7.5)", 3.1)]
        [TestCase("min('C', 'B', 'A')", "A")]
        [TestCase("max(4, 3, 7)", 7)]
        [TestCase("max(4.5, 3.1, 7.5)", 7.5)]
        [TestCase("max('C', 'B', 'A')", "C")]
        [TestCase("pow(0.5, 2)", 0.25)]
        [TestCase("round(0.5)", 0)]
        [TestCase("round(0.6)", 1)]
        [TestCase("round(-0.5)", 0)]
        [TestCase("round(-0.6)", -1)]
        [TestCase("sign(0.5)", 1)]
        [TestCase("sign(-0.1)", -1)]
        [TestCase("sign(42)", 1)]
        [TestCase("sin(0.5)", 0.479425538604203)]
        [TestCase("sinh(0.5)", 0.52109530549374738)]
        [TestCase("sqrt(0.5)", 0.70710678118654757)]
        [TestCase("tan(0.5)", 0.54630248984379048)]
        [TestCase("tanh(0.5)", 0.46211715726000974)]
        [TestCase("truncate(0.5)", 0)]
        [TestCase("truncate(0.9)", 0)]
        [TestCase("truncate(-1.9)", -1)]
        public void EvalDefaultFunction(string exprString, object expected)
        {
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var result = expr.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        [TestCase("min(@2019-11-01, @2020-01-01)", "2019-11-01")]
        [TestCase("min(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2019-11-01T09:15:35Z")]
        [TestCase("max(@2019-11-01, @2020-01-01)", "2020-01-01")]
        [TestCase("max(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2020-01-01T09:15:35Z")]
        public void EvalDateFunction(string exprString, string dateTime)
        {
            var expected = DateTimeOffset.Parse(dateTime);
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var result = expr.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        [Test]
        public void EvalNow()
        {
            var exprString = "now()";
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var value = expr.Evaluate(new { });
                Assert.That(value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }
        }

        [Test]
        public void EvalToday()
        {
            var exprString = "today()";
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var value = expr.Evaluate(new { });
                Assert.That(value, Is.EqualTo(DateTime.Today));
            }
        }
    }
}
