using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class And : IBinary, IExpressionNode
{
    public And(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.And;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}