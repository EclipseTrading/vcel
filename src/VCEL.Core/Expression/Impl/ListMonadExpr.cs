using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Monad.List;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ListMonadExpr<TContext> : IExpression<List<object>>
    {
        private readonly List<IExpression<List<object>>> list;

        public ListMonadExpr(IReadOnlyList<IExpression<List<object>>> values)
        {
            this.list = new List<IExpression<List<object>>>(values);
        }

        public IMonad<List<object>> Monad => ListMonad<object>.Instance;

        public List<object> Evaluate(IContext<List<object>> context) 
            => this.list.SelectMany(e => e.Evaluate(context)).ToList();
    }
}
