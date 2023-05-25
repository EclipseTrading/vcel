using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class String : IExpressionNode
{
    public String(string value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.String;

    public string Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}