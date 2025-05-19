using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class DateTimeOffset : IExpressionNode
{
    [JsonProperty("$type")]
    public NodeType Type => NodeType.DateTimeOffset;

    public DateTimeOffset(System.DateTimeOffset value)
    {
        Value = value;
    }

    public System.DateTimeOffset Value { get; }
}