using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class TimeSpan : IExpressionNode
{
    public TimeSpan(System.TimeSpan value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.TimeSpan;

    public System.TimeSpan Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}