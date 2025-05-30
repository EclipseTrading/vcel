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
        Assert.IsFalse(maybeExpr.Success);
        foreach (var expr in CompositeExpression.ParseMultiple(exprString))
        {
            Assert.IsFalse(expr.Success);
        }
    }
}