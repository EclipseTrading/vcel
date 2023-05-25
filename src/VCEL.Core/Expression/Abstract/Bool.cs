using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Bool : IExpressionNode
{
    public Bool(bool value)
    {
        Value = value;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.Bool;

    public bool Value { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);

    public static readonly Bool True = new(true);
}