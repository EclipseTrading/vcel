using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Multiply : IBinary, IExpressionNode
{
    public Multiply(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Multiply;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}