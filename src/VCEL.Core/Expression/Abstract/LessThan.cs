using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class LessThan : IBinary, IExpressionNode
{
    public LessThan(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.LessThan;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}