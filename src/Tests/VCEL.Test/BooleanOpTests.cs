using NUnit.Framework;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class BooleanOpTests
    {
        [TestCase("true and true", true)]
        [TestCase("true and false", false)]
        [TestCase("false and true", false)]
        [TestCase("false and false", false)]
        [TestCase("null and null", null)]
        [TestCase("null and true", null)]
        [TestCase("null and false", null)]
        [TestCase("true and null", null)]
        [TestCase("false and null", false)] // odd one out for null conditions
        [TestCase("(1 == 0) and (1 == 1)", false)]
        [TestCase("(1 == 1) and (1 == 1)", true)]
        [TestCase("(1 == 0) and (0 == 1)", false)]
        [TestCase("true && true", true)]
        [TestCase("true && false", false)]
        [TestCase("false && true", false)]
        [TestCase("false && false", false)]
        [TestCase("null && null", null)]
        [TestCase("null && true", null)]
        [TestCase("null && false", null)]
        [TestCase("true && null", null)]
        [TestCase("false && null", false)] // odd one out for null conditions
        [TestCase("(1 == 0) && (1 == 1)", false)]
        [TestCase("(1 == 1) && (1 == 1)", true)]
        [TestCase("(1 == 0) && (0 == 1)", false)]
        public void And(string exprString, object expected)
            => Compare(exprString, expected);

        [TestCase("true or true", true)]
        [TestCase("true or false", true)]
        [TestCase("false or true", true)]
        [TestCase("false or false", false)]
        [TestCase("null or null", null)]
        [TestCase("null or true", null)]
        [TestCase("null or false", null)]
        [TestCase("true or null", true)] // odd one out for null conditions
        [TestCase("false or null", null)]
        [TestCase("(1 == 0) or (1 == 1)", true)]
        [TestCase("(1 == 1) or (1 == 1)", true)]
        [TestCase("(1 == 0) or (0 == 1)", false)]
        [TestCase("true || true", true)]
        [TestCase("true || false", true)]
        [TestCase("false || true", true)]
        [TestCase("false || false", false)]
        [TestCase("null || null", null)]
        [TestCase("null || true", null)]
        [TestCase("null || false", null)]
        [TestCase("true || null", true)] // odd one out for null conditions
        [TestCase("false || null", null)]
        [TestCase("(1 == 0) || (1 == 1)", true)]
        [TestCase("(1 == 1) || (1 == 1)", true)]
        [TestCase("(1 == 0) || (0 == 1)", false)]
        public void Or(string exprString, object expected)
            => Compare(exprString, expected);

        [TestCase("!true", false)]
        [TestCase("!false", true)]
        [TestCase("!(1 == 1)", false)]
        [TestCase("!(0 == 1)", true)]
        public void Not(string exprString, object expected)
            => Compare(exprString, expected);

        private void Compare(string exprString, object expected)
        {
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var result = parseResult.Expression.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }
    }
}