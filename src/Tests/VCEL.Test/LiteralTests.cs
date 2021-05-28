using System;
using NUnit.Framework;
using VCEL.Test.Shared;

namespace VCEL.Test
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
        [TestCase(".5", 0.5d)]
        [TestCase("5.5", 5.5d)]
        [TestCase("(true)", true)]
        [TestCase("('ABC')", "ABC")]
        [TestCase("(1)", 1)]
        [TestCase("(1L)", 1L)]
        [TestCase("(0.5f)", 0.5f)]
        [TestCase("(0.5)", 0.5d)]
        public void TestPrimitives(string exprString, object expected)
        {
            foreach (var expr in CompositeExpression.ParseMultiple(exprString))
            {
                var result = expr.Expression.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
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
            foreach (var expr in CompositeExpression.ParseMultiple(dateStr))
            {
                var result = expr.Expression.Evaluate(new { });
                Assert.That(result, Is.EqualTo(DateTimeOffset.Parse(dateStr.Substring(1))));
            }
        }

        [TestCase("23:59:59.999")]
        [TestCase("1.11:59:59.999")]
        [TestCase("-251.11:59:59.999")]
        // Need to add invalid times
        public void TestTimeSpan(string timeStr)
        {
            foreach (var expr in CompositeExpression.ParseMultiple(timeStr))
            {
                var result = expr.Expression.Evaluate(new { });
                Assert.That(result, Is.EqualTo(TimeSpan.Parse(timeStr)));
            }
        }
    }
}
