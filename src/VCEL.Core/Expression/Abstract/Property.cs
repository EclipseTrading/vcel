using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Property : IExpressionNode
{
    public Property(string name)
    {
        Name = name;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Property;

    public string Name { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}