using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Paren : IExpressionNode
{
    public Paren(IExpressionNode expression)
    {
        Expression = expression;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Paren;

    public IExpressionNode Expression { get; }
}