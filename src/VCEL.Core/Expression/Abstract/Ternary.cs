using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Ternary : IExpressionNode
{
    public Ternary(IExpressionNode condition, IExpressionNode trueExpression, IExpressionNode falseExpression)
    {
        Condition = condition;
        TrueExpression = trueExpression;
        FalseExpression = falseExpression;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Ternary;

    public IExpressionNode Condition { get; }
    public IExpressionNode TrueExpression { get; }
    public IExpressionNode FalseExpression { get; }
}