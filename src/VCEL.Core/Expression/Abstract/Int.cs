using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Int : IExpressionNode
{
    public Int(int value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Int;

    public int Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}