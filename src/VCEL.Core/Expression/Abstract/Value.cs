using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Value : IExpressionNode
{
    [JsonProperty("$type")]
    public NodeType Type => NodeType.Value;

    public Value(object? value)
    {
        ValueProperty = value;
    }

    [JsonProperty("value")]
    public object? ValueProperty { get; }
}