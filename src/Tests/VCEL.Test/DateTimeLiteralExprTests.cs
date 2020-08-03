using NUnit.Framework;
using System;
using VCEL;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;

namespace VECL.Test
{
    public class DateTimeLiteralExprTests
    {

        [TestCase("(@2020-03-04T08:35:15.341Z - @2020-03-04T08:30:00.000Z) < 00:05:00", false)]
        [TestCase("(@2020-03-04T08:35:15.341Z - @2020-03-04T08:30:00.000Z) < 00:06:00", true)]
        [TestCase("@2020-03-04T08:35:15.341Z - @2020-03-04T08:30:00.000Z < 00:05:00", false)]
        [TestCase("@2020-03-04T08:35:15.341Z - @2020-03-04T08:30:00.000Z < 00:06:00", true)]
        public void CompareIntervalExpression(string exprString, bool expectedResult)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("@2020-03-04T08:35:15.341Z - @2020-03-04T08:30:00.000Z", 0, 0, 5, 15, 341)]
        [TestCase("@2020-03-04T08:35:15.341Z - @2020-03-02T08:30:00.000Z", 2, 0, 5, 15, 341)]
        [TestCase("@2020-03-04 - @2020-03-02", 2, 0, 0, 0, 0)]
        [TestCase("08:30:15.123 - 08:00:15.123", 0, 0, 30, 0, 0)]
        public void Subtract(string exprString, int d, int h, int m, int s, int ms)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(new TimeSpan(d, h, m, s, ms)));
        }

        [TestCase("@2020-03-04T08:35:15.341Z - 08:05:15.123", 2020, 3, 4, 0, 30, 0, 218)]
        public void SubtractTimeFromDateTime(string exprString, int y, int mo, int d, int h, int m, int s, int ms)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(new DateTimeOffset(y, mo, d, h, m, s, ms, TimeSpan.Zero)));
        }
    }
}
