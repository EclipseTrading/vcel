using VCEL.Core.Expression.Abstract;

namespace VCEL.Core.Expression.Transformer;

/// <summary>
/// Strips out `in null`, `not in null`, but keeps `!= null` and `== null`.
/// </summary>
public class RemoveNullTransformer : DefaultExpressionNodeVisitor
{
    public override IExpressionNode Visit(In node)
        => node.List is Null ? Bool.True : base.Visit(node);

    public override IExpressionNode Visit(Not node)
        => node.Expression is In { List: Null } ? Bool.True : base.Visit(node);
}