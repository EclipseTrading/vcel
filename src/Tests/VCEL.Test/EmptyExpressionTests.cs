using NUnit.Framework;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class EmptyExpressionTest
    {
        [TestCase("", null)]
        public void TestEmptyExpr(string exprString, object expected)
        {
            foreach (var expr in CompositeExpression.ParseMultiple(exprString))
            {
                var result = expr.Expression.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }
    }
}
