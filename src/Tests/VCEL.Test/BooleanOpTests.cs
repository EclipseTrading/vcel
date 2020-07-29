using NUnit.Framework;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class BooleanOpTests
    {
        [TestCase("true and true", true)]
        [TestCase("true and false", false)]
        [TestCase("false and true", false)]
        [TestCase("false and false", false)]
        [TestCase("(1 == 0) and (1 == 1)", false)]
        [TestCase("(1 == 1) and (1 == 1)", true)]
        [TestCase("(1 == 0) and (0 == 1)", false)]
        [TestCase("true && true", true)]
        [TestCase("true && false", false)]
        [TestCase("false && true", false)]
        [TestCase("false && false", false)]
        [TestCase("(1 == 0) && (1 == 1)", false)]
        [TestCase("(1 == 1) && (1 == 1)", true)]
        [TestCase("(1 == 0) && (0 == 1)", false)]
        public void And(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("true or true", true)]
        [TestCase("true or false", true)]
        [TestCase("false or true", true)]
        [TestCase("false or false", false)]
        [TestCase("(1 == 0) or (1 == 1)", true)]
        [TestCase("(1 == 1) or (1 == 1)", true)]
        [TestCase("(1 == 0) or (0 == 1)", false)]
        [TestCase("true || true", true)]
        [TestCase("true || false", true)]
        [TestCase("false || true", true)]
        [TestCase("false || false", false)]
        [TestCase("(1 == 0) || (1 == 1)", true)]
        [TestCase("(1 == 1) || (1 == 1)", true)]
        [TestCase("(1 == 0) || (0 == 1)", false)]
        public void Or(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("!true", false)]
        [TestCase("!false", true)]
        [TestCase("!(1 == 1)", false)]
        [TestCase("!(0 == 1)", true)]
        public void Not(string exprString, bool expected)
            => Compare(exprString, expected);

        private void Compare(string exprString, bool expected)
        {
            var expr = VCExpression.ParseDefault(exprString);
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
