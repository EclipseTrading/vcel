namespace VCEL.Core.Expression.Abstract;

public interface IExpressionNode
{
    NodeType Type { get; }

    /// <remarks>
    /// Implements the visitor pattern, where the visited node may be transformed.
    /// </remarks>
    IExpressionNode Accept(IExpressionNodeVisitor visitor);
}