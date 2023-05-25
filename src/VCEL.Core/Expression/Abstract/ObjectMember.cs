using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class ObjectMember : IExpressionNode
{
    public ObjectMember(IExpressionNode @object, IExpressionNode member)
    {
        Object = @object;
        Member = member;
    }

    [JsonProperty("$type")] public NodeType Type => NodeType.ObjectMember;

    public IExpressionNode Object { get; }
    public IExpressionNode Member { get; }

    public IExpressionNode Accept(IExpressionNodeVisitor visitor) => visitor.Visit(this);
}