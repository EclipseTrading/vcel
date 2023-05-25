namespace VCEL.Core.Expression.Abstract;

public interface IExpressionNodeVisitor
{
    IExpressionNode Visit(Add node);
    IExpressionNode Visit(And node);
    IExpressionNode Visit(Between node);
    IExpressionNode Visit(Bool node);
    IExpressionNode Visit(DateTimeOffset node);
    IExpressionNode Visit(Divide node);
    IExpressionNode Visit(Double node);
    IExpressionNode Visit(Eq node);
    IExpressionNode Visit(Function node);
    IExpressionNode Visit(GreaterOrEqual node);
    IExpressionNode Visit(GreaterThan node);
    IExpressionNode Visit(Guard node);
    IExpressionNode Visit(In node);
    IExpressionNode Visit(InSet node);
    IExpressionNode Visit(Int node);
    IExpressionNode Visit(LessOrEqual node);
    IExpressionNode Visit(LessThan node);
    IExpressionNode Visit(Let node);
    IExpressionNode Visit(List node);
    IExpressionNode Visit(Long node);
    IExpressionNode Visit(Matches node);
    IExpressionNode Visit(Mod node);
    IExpressionNode Visit(Multiply node);
    IExpressionNode Visit(Not node);
    IExpressionNode Visit(NotEq node);
    IExpressionNode Visit(Null node);
    IExpressionNode Visit(ObjectMember node);
    IExpressionNode Visit(Or node);
    IExpressionNode Visit(Paren node);
    IExpressionNode Visit(Pow node);
    IExpressionNode Visit(Property node);
    IExpressionNode Visit(Set node);
    IExpressionNode Visit(Spread node);
    IExpressionNode Visit(String node);
    IExpressionNode Visit(Subtract node);
    IExpressionNode Visit(Ternary node);
    IExpressionNode Visit(TimeSpan node);
    IExpressionNode Visit(UnaryMinus node);
    IExpressionNode Visit(Value node);
}