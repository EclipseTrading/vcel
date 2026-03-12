using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Test.Shared;

namespace VCEL.Test;

public class EmptyExpressionTest
{
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(" \n ")]
    public void TestEmptyExpr(string exprString)
    {
        var maybeExpr = VCExpression.ParseMaybe(exprString);
        Assert.That(maybeExpr.Success, Is.False);
        foreach (var expr in CompositeExpression.ParseMultiple(exprString))
        {
            Assert.That(expr.Success, Is.False);
        }
    }
}