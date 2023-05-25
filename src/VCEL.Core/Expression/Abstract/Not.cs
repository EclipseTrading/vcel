using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Not : IExpressionNode
{
    public Not(IExpressionNode expression)
    {
        Expression = expression;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Not;

    public IExpressionNode Expression { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}