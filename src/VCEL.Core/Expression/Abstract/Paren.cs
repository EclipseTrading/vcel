using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Paren : IExpressionNode
{
    public Paren(IExpressionNode expression)
    {
        Expression = expression;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Paren;

    public IExpressionNode Expression { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}