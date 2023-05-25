using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class GreaterThan : IBinary, IExpressionNode
{
    public GreaterThan(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.GreaterThan;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}