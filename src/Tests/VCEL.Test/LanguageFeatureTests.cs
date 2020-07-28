using NUnit.Framework;
using VCEL;
using VCEL.Core.Lang;

namespace VECL.Test
{
    public class LanguageFeatureTests
    {
        [Test]
        public void LetOp()
        {
            var exprStr = "let x = 1, y = 2 in x + y";
            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new object());

            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(3));
        }

        [TestCase(-0.01, 0.01)]
        [TestCase(0.04, 0.05)]
        [TestCase(0.21, 0.15)]
        [TestCase(0.35, 0.3)]
        [TestCase(0.45, 0.5)]
        [TestCase(0.7, 0.7)]
        [TestCase(0.8, 0.85)]
        [TestCase(0.95, 0.95)]
        [TestCase(0.98, 0.99)]
        public void GuardOp(double a, double expected)
        {
            var exprStr = @"
match
    | A < 0.03  = 0.01
    | A < 0.1   = 0.05
    | A < 0.225 = 0.15
    | A < 0.4   = 0.3
    | A < 0.6   = 0.5
    | A < 0.775 = 0.7
    | A < 0.9   = 0.85
    | A < 0.97  = 0.95
    | otherwise 0.99";

            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new { A = a });
            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));


        }

        [TestCase(-0.01, 0.01)]
        [TestCase(0.04, 0.05)]
        [TestCase(0.21, 0.15)]
        [TestCase(0.35, 0.3)]
        [TestCase(0.45, 0.5)]
        [TestCase(0.7, 0.7)]
        [TestCase(0.8, 0.85)]
        [TestCase(0.95, 0.95)]
        [TestCase(0.98, 0.99)]
        public void GuardWithoutOtherwise(double a, double expected)
        {
            var exprStr = @"
match
    | A < 0.03  = 0.01
    | A < 0.1   = 0.05
    | A < 0.225 = 0.15
    | A < 0.4   = 0.3
    | A < 0.6   = 0.5
    | A < 0.775 = 0.7
    | A < 0.9   = 0.85
    | A < 0.97  = 0.95
    | A >= 0.97 = 0.99";

            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new { A = a });
            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [TestCase("A ? (B + 1) : (C + 2)", true, 5, 10, 6)]
        [TestCase("A ? B + 1 : C + 2", false, 5, 10, 12)]
        [TestCase("(A ? B : C) + 1", true, 5, 10, 6)]
        [TestCase("(A ? B : C) + 1", false, 5, 10, 11)]
        public void TernaryOp(string exprStr, object a, object b, object c, object expected)
        {
            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new { A = a, B = b, C = c });

            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [TestCase("A ? B + 1 : C + 2", null, 5, 10)]
        public void TernaryMaybeNone(string exprStr, object a, object b, object c)
        {
            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new { A = a, B = b, C = c });
            Assert.That(result.HasValue, Is.False);
        }
    }
}
