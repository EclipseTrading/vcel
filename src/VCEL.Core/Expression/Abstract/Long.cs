using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Long : IExpressionNode
{
    public Long(long value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Long;

    public long Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}