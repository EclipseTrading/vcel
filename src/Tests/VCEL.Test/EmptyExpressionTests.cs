using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class EmptyExpressionTest
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \n ")]
        public void TestEmptyExpr(string exprString)
        {
            var maybeExpr = VCExpression.ParseMaybe(exprString);
            var maybeResult = maybeExpr.Expression.Evaluate(new {});
            Assert.IsFalse(maybeResult.HasValue);
            foreach (var expr in CompositeExpression.ParseMultiple(exprString))
            {
                var result = expr.Expression.Evaluate(new { });
                Assert.IsNull(result);
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \n ")]
        public void TestEmptyExprWithContext(string exprString)
        {
            var maybeExpr = VCExpression.ParseMaybe(exprString);
            var maybeResult = maybeExpr.Expression.Evaluate(new { a = 10, b = 5});
            Assert.IsFalse(maybeResult.HasValue);
            foreach (var expr in CompositeExpression.ParseMultiple(exprString))
            {
                var result = expr.Expression.Evaluate(new { a = 10, b = 5});
                Assert.IsNull(result);
            }
        }
    }
}
