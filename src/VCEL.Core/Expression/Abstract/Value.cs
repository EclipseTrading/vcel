using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Value : IExpressionNode
{
    public Value(object? value)
    {
        ValueProperty = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Value;

    [JsonProperty("value")] public object? ValueProperty { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}