using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Bool : IExpressionNode
{
    [JsonProperty("$type")]
    public NodeType Type => NodeType.Bool;

    public Bool(bool value)
    {
        Value = value;
    }

    public bool Value { get; }
}