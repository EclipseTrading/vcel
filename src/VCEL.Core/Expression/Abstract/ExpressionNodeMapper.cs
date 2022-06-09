using System;
using System.Linq;
using System.Threading.Tasks;
using VCEL.Core.Expression.Impl;
using VCEL.Expression;

namespace VCEL.Core.Expression.Abstract
{
    public class ExpressionNodeMapper<T>
    {
        private readonly IExpressionFactory<T> factory;

        public ExpressionNodeMapper(IExpressionFactory<T> factory)
        {
            this.factory = factory;
        }

        public IExpression<T> ToExpression(IExpressionNode? node)
        {
            return node switch
            {
                Ternary n => factory.Ternary(ToExpression(n.Condition), ToExpression(n.TrueExpression), ToExpression(n.FalseExpression)),
                Let n => factory.Let(n.Bindings.Select(binding => (binding.Binding, ToExpression(binding.Expression))).ToArray(),
                    ToExpression(n.Expression)),
                Guard n => factory.Guard(
                    n.Clauses.Select(clause => (ToExpression(clause.Condition), ToExpression(clause.Expression))).ToArray(),
                    ToExpression(n.Otherwise)),
                LessThan n => factory.LessThan(ToExpression(n.Left), ToExpression(n.Right)),
                GreaterThan n => factory.GreaterThan(ToExpression(n.Left), ToExpression(n.Right)),
                LessOrEqual n => factory.LessOrEqual(ToExpression(n.Left), ToExpression(n.Right)),
                GreaterOrEqual n => factory.GreaterOrEqual(ToExpression(n.Left), ToExpression(n.Right)),
                Between n => factory.Between(ToExpression(n.Left), ToExpression(n.Right)),
                InSet n => factory.InSet(ToExpression(n.Left), n.Set),
                In n => factory.In(ToExpression(n.Left), ToExpression(n.List)),
                Spread n => factory.Spread(ToExpression(n.List)),
                Matches n => factory.Matches(ToExpression(n.Left), ToExpression(n.Right)),
                And n => factory.And(ToExpression(n.Left), ToExpression(n.Right)),
                Or n => factory.Or(ToExpression(n.Left), ToExpression(n.Right)),
                Not n => factory.Not(ToExpression(n.Expression)),
                List n => factory.List(n.Items.Select(ToExpression).ToArray()),
                Add n => factory.Add(ToExpression(n.Left), ToExpression(n.Right)),
                Multiply n => factory.Multiply(ToExpression(n.Left), ToExpression(n.Right)),
                Subtract n => factory.Subtract(ToExpression(n.Left), ToExpression(n.Right)),
                Divide n => factory.Divide(ToExpression(n.Left), ToExpression(n.Right)),
                Pow n => factory.Pow(ToExpression(n.Left), ToExpression(n.Right)),
                Paren n => factory.Paren(ToExpression(n.Expression)),
                Property n => factory.Property(n.Name),
                Function n => factory.Function(n.Name, n.Args.Select(ToExpression).ToArray()),
                UnaryMinus n => factory.UnaryMinus(ToExpression(n.Expression)),
                Null _ => factory.Null(),
                Eq n => factory.Eq(ToExpression(n.Left), ToExpression(n.Right)),
                NotEq n => factory.NotEq(ToExpression(n.Left), ToExpression(n.Right)),
                ObjectMember n => factory.Member(ToExpression(n.Object), ToExpression(n.Member)),
                Bool n => factory.Bool(n.Value),
                Double n => factory.Double(n.Value),
                Int n => factory.Int(n.Value),
                Long n => factory.Long(n.Value),
                String n => factory.String(n.Value),
                DateTimeOffset n => factory.DateTimeOffset(n.Value),
                TimeSpan n => factory.TimeSpan(n.Value),
                Set n => factory.Set(n.Value),
                Value n => factory.Value(n.ValueProperty),
                _ => throw new Exception($"Expression node type not handled '{node?.Type}'"),
            };
        }

        public IExpressionNode ToExpressionNode(IExpression<T>? node)
        {
            return node switch
            {
                Ternary<T> e => new Ternary(ToExpressionNode(e.ConditionExpr), ToExpressionNode(e.TrueExpr), ToExpressionNode(e.FalseExpr)),
                LetExpr<T> e => new Let(e.Bindings.Select(binding => (binding.Item1, ToExpressionNode(binding.Item2))).ToArray(),
                    ToExpressionNode(e.Expr)),
                GuardExpr<T> e => new Guard(
                    e.Clauses.Select(clause => (ToExpressionNode(clause.Cond), ToExpressionNode(clause.Res))).ToArray(),
                    ToExpressionNode(e.Otherwise)),
                LessThan<T> e => new LessThan(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                GreaterThan<T> e => new GreaterThan(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                LessOrEqual<T> e => new LessOrEqual(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                GreaterOrEqual<T> e => new GreaterOrEqual(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                BetweenExpr<T> e => new Between(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                InSetExpr<T> e => new InSet(ToExpressionNode(e.Left), e.Set),
                InExpr<T> e => new In(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                SpreadExpr<T> e => new Spread(ToExpressionNode(e.List)),
                MatchesExpr<T> e => new Matches(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                AndExpr<T> e => new And(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                OrExpr<T> e => new Or(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                NotExpr<T> e => new Not(ToExpressionNode(e.Expr)),
                ListExpr<T> e => new List(e.List.Select(ToExpressionNode).ToArray()),
                AddExpr<T> e => new Add(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                MultExpr<T> e => new Multiply(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                SubtractExpr<T> e => new Subtract(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                DivideExpr<T> e => new Divide(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                PowExpr<T> e => new Pow(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                ParenExpr<T> e => new Paren(ToExpressionNode(e.Expr)),
                Property<T> e => new Property(e.Name),
                FunctionExpr<T> e => new Function(e.Name, e.Args.Select(ToExpressionNode).ToArray()),
                UnaryMinusExpr<T> e => new UnaryMinus(ToExpressionNode(e.Expr)),
                NullExpr<T> _ => new Null(),
                EqExpr<T> e => new Eq(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                NotEqExpr<T> e => new NotEq(ToExpressionNode(e.Left), ToExpressionNode(e.Right)),
                ObjectMember<T> e => new ObjectMember(ToExpressionNode(e.Obj), ToExpressionNode(e.Member)),
                BoolExpr<T> e => new Bool(e.Value),
                DoubleExpr<T> e => new Double(e.Value),
                IntExpr<T> e => new Int(e.Value),
                LongExpr<T> e => new Long(e.Value),
                StringExpr<T> e => new String(e.Value),
                DateTimeOffsetExpr<T> e => new DateTimeOffset(e.Value),
                TimeSpanExpr<T> e => new TimeSpan(e.Value),
                SetExpr<T> e => new Set(e.Value),
                ValueExpr<T, object> e => new Value(e.Value),                
                _ => throw new Exception($"Expression node type not handled '{node?.GetType()}'"),
            };
        }
    }
}
