using NUnit.Framework;
using VCEL.Core.Expression;
using VCEL.Core.Expression.Abstract;
using VCEL.Core.Expression.Transformer;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.Test.Expression.Transformer;

/// <summary>
/// When users add a new in-place filter in the filter bar, and they only select a column for an in expression with the
/// value left empty, the result is a `null` value instead of empty list `{}` as that would otherwise clear the grid.
/// This transformer removes those null expressions from the filter as they should be disabled.
/// </summary>
public class RemoveNullTransformerTests
{
    private static readonly IExpressionNodeVisitor Transformer = new RemoveNullTransformer();

    [TestCase("y == 50 and x == null", "y == 50 and x == null")]
    [TestCase("y == 50 and x != null", "y == 50 and x != null")]
    public void ShouldNotStripNull(string expression, string expected) => Process(expression, expected);

    /// <remarks>
    /// After migrating from SpEL, the previous "not x in null" expression is now "!(x in null)".
    /// TODO: Should we iterate multiple times with additional transformers to remove the extraneous `true` or `!(true)` expressions?
    /// </remarks>
    [TestCase("y == 50 and x in null", "y == 50 and true")]
    [TestCase("y == 50 and !(x in null)", "y == 50 and !(true)")]
    public void ShouldStripNull(string expression, string expected) => Process(expression, expected);

    private static void Process(string expression, string expected)
    {
        var parsedExpression = VCExpression.ParseDefault(expression);
        var expressionFactory = new ExpressionFactory<object>(ExprMonad.Instance);
        var expressionMapper = new ExpressionNodeMapper<object>(expressionFactory);
        var rootNode = expressionMapper.ToExpressionNode(parsedExpression.Expression);
        var transformedNode = ExpressionNodeTransformer.Transform(rootNode, new[] { Transformer });
        var stringExpressionFactory = new ToStringExpressionFactory(ConcatStringMonad.Instance);
        var stringExpressionMapper = new ExpressionNodeMapper<string>(stringExpressionFactory);
        var transformedExpression = stringExpressionMapper.ToExpression(transformedNode);

        var result = transformedExpression.Evaluate(new { });

        Assert.AreEqual(expected, result);
    }
}