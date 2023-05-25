using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Double : IExpressionNode
{
    public Double(double value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Double;

    public double Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}