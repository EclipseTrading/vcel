using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Function : IExpressionNode
{
    public Function(string name, IReadOnlyList<IExpressionNode> args)
    {
        Name = name;
        Args = args;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Function;

    public string Name { get; }
    public IReadOnlyList<IExpressionNode> Args { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}