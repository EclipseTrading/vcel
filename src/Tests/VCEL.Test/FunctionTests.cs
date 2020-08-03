using NUnit.Framework;
using System;
using VCEL;
using VCEL.Core.Lang;

namespace VECL.Test
{
    public class FunctionTests
    {
        [TestCase("min(4, 3, 7)", 3)]
        [TestCase("min(4.5, 3.1, 7.5)", 3.1)]
        [TestCase("min('C', 'B', 'A')", "A")]
        [TestCase("max(4, 3, 7)", 7)]
        [TestCase("max(4.5, 3.1, 7.5)", 7.5)]
        [TestCase("max('C', 'B', 'A')", "C")]
        [TestCase("abs(1)", 1)]
        [TestCase("abs(-1)", 1)]
        [TestCase("abs(1.1)", 1.1)]
        [TestCase("abs(-1.1)", 1.1)]
        public void EvalDefaultFunction(string exprString, object expected)
        {
            var expr = VCExpression.ParseDefault(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("min(@2019-11-01, @2020-01-01)", "2019-11-01")]
        [TestCase("min(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2019-11-01T09:15:35Z")]
        [TestCase("max(@2019-11-01, @2020-01-01)", "2020-01-01")]
        [TestCase("max(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2020-01-01T09:15:35Z")]
        public void EvalDateFunction(string exprString, string dateTime)
        {
            var expected = DateTimeOffset.Parse(dateTime);
            var expr = VCExpression.ParseDefault(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EvalNow()
        {
            var expr = VCExpression.ParseDefault("now()").Expression;
            var value = expr.Evaluate(new { });
            Assert.That(value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void EvalToday()
        {
            var expr = VCExpression.ParseDefault("today()").Expression;
            var value = expr.Evaluate(new { });
            Assert.That(value, Is.EqualTo(DateTime.Today));
        }
    }
}
