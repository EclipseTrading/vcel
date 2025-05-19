using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Pow : IBinary, IExpressionNode
{
    public Pow(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Pow;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }
}