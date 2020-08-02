using System.Collections.Generic;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Core.Expression.Op;
using VCEL.Monad;

namespace VCEL.Expression
{
    public class ListExpressionFactory<TContext> : ExpressionFactory<List<object>>
    {
        public ListExpressionFactory(
            IMonad<List<object>> monad,
            IOperators operators = null,
            IFunctions functions = null)
            : base(
                  monad,
                  operators ?? new DefaultOperators(),
                  functions ?? new DefaultFunctions())
        {
        }

        public override IExpression<List<object>> Property(string name)
            => new ListProperty<TContext>(name);
        public override IExpression<List<object>> List(IReadOnlyList<IExpression<List<object>>> l)
            => new ListMonadExpr<TContext>(l);

    }
}
