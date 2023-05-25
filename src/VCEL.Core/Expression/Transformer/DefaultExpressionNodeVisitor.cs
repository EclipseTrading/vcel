using System.Linq;
using VCEL.Core.Expression.Abstract;

namespace VCEL.Core.Expression.Transformer;

/// <summary>
/// Default visitor that performs no transformations.
/// </summary>
/// <remarks>
/// Inheritors can override specific visitors to adjust behavior or perform transformations by returning a different
/// <see cref="IExpressionNode"/> implementation.
/// </remarks>
public class DefaultExpressionNodeVisitor : IExpressionNodeVisitor
{
    public virtual IExpressionNode Visit(Add node)
        => new Add(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(And node)
        => new And(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Between node)
        => new Between(node.Left.Accept(this), node.Lower.Accept(this), node.Upper.Accept(this));

    public virtual IExpressionNode Visit(Bool node) => node;

    public virtual IExpressionNode Visit(DateTimeOffset node) => node;

    public virtual IExpressionNode Visit(Divide node)
        => new Divide(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Double node) => node;

    public virtual IExpressionNode Visit(Eq node)
        => new Eq(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Function node)
        => new Function(node.Name, node.Args.Select(arg => arg.Accept(this)).ToArray());

    public virtual IExpressionNode Visit(GreaterOrEqual node)
        => new GreaterOrEqual(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(GreaterThan node)
        => new GreaterThan(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Guard node)
        => new Guard(
            node.Clauses.Select(x => (x.Condition.Accept(this), x.Expression.Accept(this))).ToArray(),
            node.Otherwise.Accept(this));

    public virtual IExpressionNode Visit(In node)
        => new In(node.Left.Accept(this), node.List.Accept(this));

    public virtual IExpressionNode Visit(InSet node)
        => new InSet(node.Left.Accept(this), node.Set);

    public virtual IExpressionNode Visit(Int node) => node;

    public virtual IExpressionNode Visit(LessOrEqual node)
        => new LessOrEqual(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(LessThan node)
        => new LessThan(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Let node)
        => new Let(
            node.Bindings.Select(x => (x.Binding, x.Expression.Accept(this))).ToArray(),
            node.Expression.Accept(this));

    public virtual IExpressionNode Visit(List node)
        => new List(node.Items.Select(x => x.Accept(this)).ToArray());

    public virtual IExpressionNode Visit(Long node) => node;

    public virtual IExpressionNode Visit(Matches node)
        => new Matches(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Mod node)
        => new Mod(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Multiply node)
        => new Multiply(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Not node)
        => new Not(node.Expression.Accept(this));

    public virtual IExpressionNode Visit(NotEq node)
        => new NotEq(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Null node) => node;

    public virtual IExpressionNode Visit(ObjectMember node)
        => new ObjectMember(node.Object.Accept(this), node.Member.Accept(this));

    public virtual IExpressionNode Visit(Or node)
        => new Or(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Paren node)
        => new Paren(node.Expression.Accept(this));

    public virtual IExpressionNode Visit(Pow node)
        => new Pow(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Property node) => node;

    public virtual IExpressionNode Visit(Set node) => node;

    public virtual IExpressionNode Visit(Spread node)
        => new Spread(node.List.Accept(this));

    public virtual IExpressionNode Visit(String node) => node;

    public virtual IExpressionNode Visit(Subtract node)
        => new Subtract(node.Left.Accept(this), node.Right.Accept(this));

    public virtual IExpressionNode Visit(Ternary node)
        => new Ternary(
            node.Condition.Accept(this),
            node.TrueExpression.Accept(this),
            node.FalseExpression.Accept(this));

    public virtual IExpressionNode Visit(TimeSpan node) => node;

    public virtual IExpressionNode Visit(UnaryMinus node)
        => new UnaryMinus(node.Expression.Accept(this));

    public virtual IExpressionNode Visit(Value node) => node;
}