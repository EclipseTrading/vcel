using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Expression
{
    public class ToStringExpressionFactory<TContext> : ExpressionFactory<string>
    {
        public ToStringExpressionFactory(IMonad<string> monad) : base(monad)
        {
        }

        public override IExpression<string> Property(string name)
            => new ValueExpr<string>(Monad, name);
        public override IExpression<string> Add(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "+", l, r);
        public override IExpression<string> Subtract(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "-", l, r);
        public override IExpression<string> Pow(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "^", l, r);
        public override IExpression<string> Divide(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "/", l, r);
        public override IExpression<string> Multiply(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "*", l, r);
        public override IExpression<string> In(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "in", l, r);
        public override IExpression<string> Between(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(Monad, "between", l, r);
    }
}
