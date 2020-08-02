using VCEL.Core.Expression.Impl;
using VCEL.Core.Expression.Op;
using VCEL.Monad;

namespace VCEL.Expression
{
    using Monad = M<string>;
    public class ToStringExpressionFactory<TContext> : ExpressionFactory<Monad>
    {
        public ToStringExpressionFactory(IMonad<M<string>> monad)
            : base(monad, new ToStringOperators())
        {
        }

        public override IExpression<Monad> Property(string name)
            => new ValueExpr<Monad>(Monad, name);
    }
}
