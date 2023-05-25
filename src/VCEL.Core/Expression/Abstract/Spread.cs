using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Spread : IExpressionNode
{
    public Spread(IExpressionNode list)
    {
        List = list;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Spread;

    public IExpressionNode List { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}