using NUnit.Framework;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class MathematicsExpressions
    {
        [TestCase("0", 0)]
        [TestCase("-0", 0)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        [TestCase("(1)", 1)]
        [TestCase("  (  1    )  ", 1)]
        [TestCase("(-1)", -1)]
        [TestCase("-(1)", -1)]
        [TestCase("2 + 1", 3)]
        [TestCase("(2 + 1)", 3)]
        [TestCase("(2) + (1)", 3)]
        [TestCase("((2) + (1))", 3)]
        [TestCase("2 - 1", 1)]
        [TestCase("2 + -1", 1)]
        [TestCase("(2 - 1)", 1)]
        [TestCase("(2 - (-1))", 3)]
        [TestCase("(2) - (1)", 1)]
        [TestCase("((2) - (1))", 1)]
        [TestCase("2 * 3", 6)]
        [TestCase("2 * 3 + 1", 7)]
        [TestCase("(2 * 3) + 1", 7)]
        [TestCase("2 * (3 + 1)", 8)]
        [TestCase("2 * 3 - 1", 5)]
        [TestCase("(2 * 3) - 1", 5)]
        [TestCase("2 * (3 - 1)", 4)]
        [TestCase("6 / 3", 2)]
        [TestCase("6 / 3 + 1", 3)]
        [TestCase("6 / 3 - 1", 1)]
        [TestCase("6 / 3 -1", 1)]
        [TestCase("8 / (3 + 1)", 2)]
        [TestCase("6 / (3 - 1)", 3)]
        [TestCase("12.0 / 4.0 * 100", 300)]
        [TestCase("144.0 / 12.0 / 4.0 * 100", 300)]
        [TestCase("12.0 / (4.0 * 100)", 0.03)]
        [TestCase("144.0 / 12.0 / (4.0 * 100.0)", 0.03)]
        [TestCase("(12 / 4) * 100", 300)]
        [TestCase("144 / 12 / 4", 3)]
        [TestCase("144 / 12", 12)]
        [TestCase("3.1 * 2", 6.2)]
        [TestCase("2 * 3.1", 6.2)]
        [TestCase("4.2 / 2.1", 2)]
        [TestCase("10 / 2.5", 4)]
        [TestCase("10^2", 100)]
        [TestCase("3^2^3", 6561)]
        [TestCase("3^3^2", 19683)]
        [TestCase("3^(2^3)", 6561)]
        [TestCase("(3^2)^3", 729)]
        [TestCase("10^2", 100)]
        [TestCase("3^2 + 1^2", 10)]
        [TestCase("(3^2 + 1)^2", 100)]
        [TestCase("3^2*2^2", 36)]
        [TestCase("3^2/2^2", 2.25)]
        [TestCase("3^(2*2)^2", 43046721)]
        [TestCase("3^(2/2)^2", 3)]
        public void ShouldEvaluate(string exprStr, object expected)
        {
            var parseResult = VCExpression.ParseMaybe(exprStr);
            Assert.That(parseResult.Success, Is.True, "Is successful");

            var expr = parseResult.Expression;

            var result = expr.Evaluate(new { });
            Assert.That(result.HasValue, Is.True, "Have value");
            Assert.That(result.Value, Is.EqualTo(expected));
        }

    }

}
