using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ListExpr<TMonad> : IExpression<TMonad>
    {
        private readonly List<IExpression<TMonad>> list;

        public ListExpr(IMonad<TMonad> monad, IReadOnlyList<IExpression<TMonad>> values)
        {
            list = new List<IExpression<TMonad>>(values);
            Monad = monad;
        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
            => Monad.Lift(list.Select(e => e.Evaluate(context)).ToList());
    }
}
