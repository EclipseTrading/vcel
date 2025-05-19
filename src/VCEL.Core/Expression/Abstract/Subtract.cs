using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Subtract : IBinary, IExpressionNode
{
    public Subtract(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Subtract;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }
}