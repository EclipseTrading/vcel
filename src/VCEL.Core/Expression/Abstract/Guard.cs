using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Guard : IExpressionNode
{
    public Guard(IReadOnlyList<(IExpressionNode Condition, IExpressionNode Expression)> clauses, IExpressionNode otherwise)
    {
        Clauses = clauses;
        Otherwise = otherwise;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Guard;

    public IReadOnlyList<(IExpressionNode Condition, IExpressionNode Expression)> Clauses { get; }
    public IExpressionNode Otherwise { get; }
}