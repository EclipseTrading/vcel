using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class UnaryMinus : IExpressionNode
{
    public UnaryMinus(IExpressionNode expression)
    {
        Expression = expression;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.UnaryMinus;

    public IExpressionNode Expression { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}