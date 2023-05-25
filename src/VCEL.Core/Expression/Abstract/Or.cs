using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Or : IBinary, IExpressionNode
{
    public Or(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Or;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}