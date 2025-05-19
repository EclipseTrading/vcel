using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Let : IExpressionNode
{
    public Let(IReadOnlyList<(string Binding, IExpressionNode Expression)> bindings, IExpressionNode expression)
    {
        Bindings = bindings;
        Expression = expression;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Let;

    public IReadOnlyList<(string Binding, IExpressionNode Expression)> Bindings { get; }
    public IExpressionNode Expression { get; }
}