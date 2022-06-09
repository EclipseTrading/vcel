using System.Collections.Generic;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Expression
{
    public class ListExpressionFactory<TContext> : ExpressionFactory<List<object>>
    {
        public ListExpressionFactory(
            IMonad<List<object>> monad,
            IFunctions<List<object>>? functions = null)
            : base(
                monad,
                functions ?? new DefaultFunctions<List<object>>())
        {
        }

        public override IExpression<List<object>> Property(string name)
            => new ListProperty<TContext>(name);
        public override IExpression<List<object>> List(IReadOnlyList<IExpression<List<object>>> l)
            => new ListMonadExpr<TContext>(l);

    }
}
