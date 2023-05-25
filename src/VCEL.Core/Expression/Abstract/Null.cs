using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Null : IExpressionNode
{
    [JsonProperty("$type")] public NodeType Type => NodeType.Null;

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}