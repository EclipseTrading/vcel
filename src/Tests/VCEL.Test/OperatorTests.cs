using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Test.Shared;

namespace VCEL.Test;

public class OperatorTests
{
    [TestCase("IntValue + IntValue", 2)]
    [TestCase("IntValue + FloatValue", 2.1)]
    [TestCase("IntValue + LongValue", 2)]
    [TestCase("IntValue + DoubleValue", 2.1)]
    [TestCase("IntValue + DecimalValue", 2.1)]
    [TestCase("FloatValue + IntValue", 2.1)]
    [TestCase("FloatValue + FloatValue", 2.2)]
    [TestCase("FloatValue + LongValue", 2.1)]
    [TestCase("FloatValue + DoubleValue", 2.2)]
    [TestCase("FloatValue + DecimalValue", 2.2)]
    [TestCase("LongValue + IntValue", 2)]
    [TestCase("LongValue + FloatValue", 2.1)]
    [TestCase("LongValue + LongValue", 2)]
    [TestCase("LongValue + DoubleValue", 2.1)]
    [TestCase("LongValue + DecimalValue", 2.1)]
    [TestCase("DoubleValue + IntValue", 2.1)]
    [TestCase("DoubleValue + FloatValue", 2.2)]
    [TestCase("DoubleValue + LongValue", 2.1)]
    [TestCase("DoubleValue + DoubleValue", 2.2)]
    [TestCase("DoubleValue + DecimalValue", 2.2)]
    [TestCase("DecimalValue + IntValue", 2.1)]
    [TestCase("DecimalValue + FloatValue", 2.2)]
    [TestCase("DecimalValue + LongValue", 2.1)]
    [TestCase("DecimalValue + DoubleValue", 2.2)]
    [TestCase("DecimalValue + DecimalValue", 2.2)]
    public void MixedNumericPropertiesAdd(string exprString, object expected)
        => Evaluate(exprString, expected, new
        {
            IntValue = 1,
            FloatValue = 1.1f,
            LongValue = 1L,
            DoubleValue = 1.1d,
            DecimalValue = 1.1m,
        });

    [TestCase("IntValue - IntValue", 0)]
    [TestCase("IntValue - FloatValue", -0.1)]
    [TestCase("IntValue - LongValue", 0)]
    [TestCase("IntValue - DoubleValue", -0.1)]
    [TestCase("IntValue - DecimalValue", -0.1)]
    [TestCase("FloatValue - IntValue", 0.1)]
    [TestCase("FloatValue - FloatValue", 0)]
    [TestCase("FloatValue - LongValue", 0.1)]
    [TestCase("FloatValue - DoubleValue", 0)]
    [TestCase("FloatValue - DecimalValue", 0)]
    [TestCase("LongValue - IntValue", 0)]
    [TestCase("LongValue - FloatValue", -0.1)]
    [TestCase("LongValue - LongValue", 0)]
    [TestCase("LongValue - DoubleValue", -0.1)]
    [TestCase("LongValue - DecimalValue", -0.1)]
    [TestCase("DoubleValue - IntValue", 0.1)]
    [TestCase("DoubleValue - FloatValue", 0)]
    [TestCase("DoubleValue - LongValue", 0.1)]
    [TestCase("DoubleValue - DoubleValue", 0)]
    [TestCase("DoubleValue - DecimalValue", 0)]
    [TestCase("DecimalValue - IntValue", 0.1)]
    [TestCase("DecimalValue - FloatValue", 0)]
    [TestCase("DecimalValue - LongValue", 0.1)]
    [TestCase("DecimalValue - DoubleValue", 0)]
    [TestCase("DecimalValue - DecimalValue", 0)]
    public void MixedNumericPropertiesSub(string exprString, object expected)
        => Evaluate(exprString, expected, new
        {
            IntValue = 1,
            FloatValue = 1.1f,
            LongValue = 1L,
            DoubleValue = 1.1d,
            DecimalValue = 1.1m,
        });

    [TestCase("IntValue * IntValue", 1)]
    [TestCase("IntValue * FloatValue", 1.5)]
    [TestCase("IntValue * LongValue", 1)]
    [TestCase("IntValue * DoubleValue", 1.5)]
    [TestCase("IntValue * DecimalValue", 1.5)]
    [TestCase("FloatValue * IntValue", 1.5)]
    [TestCase("FloatValue * FloatValue", 2.25)]
    [TestCase("FloatValue * LongValue", 1.5)]
    [TestCase("FloatValue * DoubleValue", 2.25)]
    [TestCase("FloatValue * DecimalValue", 2.25)]
    [TestCase("LongValue * IntValue", 1)]
    [TestCase("LongValue * FloatValue", 1.5)]
    [TestCase("LongValue * LongValue", 1)]
    [TestCase("LongValue * DoubleValue", 1.5)]
    [TestCase("LongValue * DecimalValue", 1.5)]
    [TestCase("DoubleValue * IntValue", 1.5)]
    [TestCase("DoubleValue * FloatValue", 2.25)]
    [TestCase("DoubleValue * LongValue", 1.5)]
    [TestCase("DoubleValue * DoubleValue", 2.25)]
    [TestCase("DoubleValue * DecimalValue", 2.25)]
    [TestCase("DecimalValue * IntValue", 1.5)]
    [TestCase("DecimalValue * FloatValue", 2.25)]
    [TestCase("DecimalValue * LongValue", 1.5)]
    [TestCase("DecimalValue * DoubleValue", 2.25)]
    [TestCase("DecimalValue * DecimalValue", 2.25)]
    public void MixedNumericPropertiesMultiply(string exprString, object expected)
        => Evaluate(exprString, expected, new
        {
            IntValue = 1,
            FloatValue = 1.5f,
            LongValue = 1L,
            DoubleValue = 1.5d,
            DecimalValue = 1.5m,
        });

    [TestCase("IntValue / IntValue", 1)]
    [TestCase("IntValue / FloatValue", 0.8)]
    [TestCase("IntValue / LongValue", 1)]
    [TestCase("IntValue / DoubleValue", 0.8)]
    [TestCase("IntValue / DecimalValue", 0.8)]
    [TestCase("FloatValue / IntValue", 1.25)]
    [TestCase("FloatValue / FloatValue", 1)]
    [TestCase("FloatValue / LongValue", 1.25)]
    [TestCase("FloatValue / DoubleValue", 1)]
    [TestCase("FloatValue / DecimalValue", 1)]
    [TestCase("LongValue / IntValue", 1)]
    [TestCase("LongValue / FloatValue", 0.8)]
    [TestCase("LongValue / LongValue", 1)]
    [TestCase("LongValue / DoubleValue", 0.8)]
    [TestCase("LongValue / DecimalValue", 0.8)]
    [TestCase("DoubleValue / IntValue", 1.25)]
    [TestCase("DoubleValue / FloatValue", 1)]
    [TestCase("DoubleValue / LongValue", 1.25)]
    [TestCase("DoubleValue / DoubleValue", 1)]
    [TestCase("DoubleValue / DecimalValue", 1)]
    [TestCase("DecimalValue / IntValue", 1.25)]
    [TestCase("DecimalValue / FloatValue", 1)]
    [TestCase("DecimalValue / LongValue", 1.25)]
    [TestCase("DecimalValue / DoubleValue", 1)]
    [TestCase("DecimalValue / DecimalValue", 1)]
    public void MixedNumericPropertiesDivide(string exprString, object expected)
        => Evaluate(exprString, expected, new
        {
            IntValue = 2,
            FloatValue = 2.5f,
            LongValue = 2L,
            DoubleValue = 2.5d,
            DecimalValue = 2.5m,
        });

    private void Evaluate(string exprString, object expected, object? o = null, decimal precision = 0.0001m)
    {
        foreach (var expr in CompositeExpression.ParseMultiple(exprString))
        {
            Assert.True(expr.Success, "Default expression parse");
            var result = expr.Expression.Evaluate(o ?? new { });
            Assert.That(result, Is.EqualTo(expected).Within(precision), "Default expression evaluated");
        }

        var maybeExpr = VCExpression.ParseMaybe(exprString);
        Assert.True(maybeExpr.Success, "Maybe expression parse");
        var maybeResult = maybeExpr.Expression.Evaluate(o ?? new { });
        Assert.True(maybeResult.HasValue, "Maybe expression evaluate has value");
        Assert.That(maybeResult.Value, Is.EqualTo(expected).Within(precision), "Maybe expression evaluated");
    }
}