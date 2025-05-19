using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Double : IExpressionNode
{
    [JsonProperty("$type")]
    public NodeType Type => NodeType.Double;

    public Double(double value)
    {
        Value = value;
    }

    public double Value { get; }
}