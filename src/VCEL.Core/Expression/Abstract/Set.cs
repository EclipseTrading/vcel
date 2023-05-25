using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Set : IExpressionNode
{
    public Set(ISet<object> value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Set;

    public ISet<object> Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}