using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class DateTimeOffset : IExpressionNode
{
    public DateTimeOffset(System.DateTimeOffset value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.DateTimeOffset;

    public System.DateTimeOffset Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}