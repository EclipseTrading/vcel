using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Divide : IBinary, IExpressionNode
{
    public Divide(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Divide;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}