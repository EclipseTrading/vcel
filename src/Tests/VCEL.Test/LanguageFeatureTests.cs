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
            var expr = VCExpression.ParseMaybe(exprStr).Expression;
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

            var expr = VCExpression.ParseMaybe(exprStr).Expression;
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

            var expr = VCExpression.ParseMaybe(exprStr).Expression;
            var result = expr.Evaluate(new { A = a });
            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [TestCase(0.7, 2.0, "Low")]
        [TestCase(1.2, 2.0, "Normal")]
        [TestCase(1.6, 2.0, "Near Breach")]
        [TestCase(2.2, 2.0, "Breach")]
        [TestCase(5.0, 2.0, "Critical")]
        public void StringLetGuard(double a, double l, string expected)
        {
            var exprStr = @"
let 
   p = a / l
in (match
   | p < 0.5  = 'Low'
   | p < 0.75 = 'Normal'
   | p < 1.0  = 'Near Breach'
   | p < 1.25 = 'Breach'
   | otherwise 'Critical')
";
            var expr = VCExpression.ParseMaybe(exprStr).Expression;
            var result = expr.Evaluate(new { a, l });
            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void MultipleLetEval()
        {
            var exprString = "let x = a + 1 in x + b";
            var expr = VCExpression.ParseDefault(exprString);
            var result1 = expr.Expression.Evaluate(new { a = 5, b = 6 });
            var result2 = expr.Expression.Evaluate(new { a = 10, b = 11 });
            Assert.That(result1, Is.EqualTo(12));
            Assert.That(result2, Is.EqualTo(22));
        }

        [TestCase("A ? (B + 1) : (C + 2)", true, 5, 10, 6)]
        [TestCase("A ? B + 1 : C + 2", false, 5, 10, 12)]
        [TestCase("(A ? B : C) + 1", true, 5, 10, 6)]
        [TestCase("(A ? B : C) + 1", false, 5, 10, 11)]
        public void TernaryOp(string exprStr, object a, object b, object c, object expected)
        {
            var expr = VCExpression.ParseMaybe(exprStr).Expression;
            var result = expr.Evaluate(new { A = a, B = b, C = c });

            Assert.That(result.HasValue);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [TestCase("A ? B + 1 : C + 2", null, 5, 10)]
        public void TernaryMaybeNone(string exprStr, object a, object b, object c)
        {
            var expr = VCExpression.ParseMaybe(exprStr).Expression;
            var result = expr.Evaluate(new { A = a, B = b, C = c });
            Assert.That(result.HasValue, Is.False);
        }

        [TestCase("A == null ? 1 : 2", null, 1)]
        [TestCase("A == null ? 1 : 2", "A", 2)]
        [TestCase("null == A ? 1 : 2", "A", 2)]
        [TestCase("null != A ? 1 : 2", "A", 1)]
        [TestCase("A != null ? 1 : 2", "A", 1)]
        [TestCase("A != null ? 1 : 2", null, 2)]
        [TestCase("null != A ? 1 : 2", null, 2)]
        public void TernaryNullCase(string exprStr, object a, object c)
        {
            var expr = VCExpression.ParseMaybe(exprStr).Expression;
            var result = expr.Evaluate(new { A = a });
            Assert.That(result.HasValue, Is.True);
            if (result.HasValue)
            {
                Assert.AreEqual(c, result.Value);
            }
        }
    }
}
