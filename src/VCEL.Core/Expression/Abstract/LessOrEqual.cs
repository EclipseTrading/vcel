using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class LessOrEqual : IBinary, IExpressionNode
{
    public LessOrEqual(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.LessOrEqual;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }
}