using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ListExpr<TMonad> : IExpression<TMonad>
    {
        public List<IExpression<TMonad>> List { get; }

        public ListExpr(IMonad<TMonad> monad, IReadOnlyList<IExpression<TMonad>> values)
        {
            List = new List<IExpression<TMonad>>(values);
            Monad = monad;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => List.SelectMany(i => i.Dependencies).Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
            => Monad.Lift(List.Select(e => e.Evaluate(context)).ToList());
    }
}
