using NUnit.Framework;
using System;
using VCEL;
using VCEL.Core.Expression.Op;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;

namespace VECL.Test
{
    public class LiteralTests
    {

        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("'ABC'", "ABC")]
        [TestCase("'true'", "true")]
        [TestCase("'0'", "0")]
        [TestCase("1", 1)]
        [TestCase("1L", 1L)]
        [TestCase("0.5f", 0.5f)]
        [TestCase("0.5", 0.5d)]
        [TestCase("(true)", true)]
        [TestCase("('ABC')", "ABC")]
        [TestCase("(1)", 1)]
        [TestCase("(1L)", 1L)]
        [TestCase("(0.5f)", 0.5f)]
        [TestCase("(0.5)", 0.5d)]
        public void TestPrimitives(string exprString, object expected)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(exprString);
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("@2020-03-04T08:35:15.341Z")]
        [TestCase("@2020-03-04T08:35:15.341+08:00")]
        [TestCase("@2020-03-04T08:35:15.341-08:00")]
        [TestCase("@2020-03-04T08:35:15.341-08")]
        [TestCase("@2020-03-04T08:35:15-08:00")]
        [TestCase("@2020-03-04T08:35:15")]
        [TestCase("@2020-03-04")]
        // Need to add invalid dates
        public void TestDateTimeOffset(string dateStr)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance, new DefaultOperators());
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(dateStr);
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(DateTimeOffset.Parse(dateStr.Substring(1))));
        }


        [TestCase("23:59:59.999")]
        [TestCase("1.11:59:59.999")]
        [TestCase("-251.11:59:59.999")]
        // Need to add invalid times
        public void TestTimeSpan(string timeStr)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance, new DefaultOperators());
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(timeStr);
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(TimeSpan.Parse(timeStr)));
        }
    }
}
