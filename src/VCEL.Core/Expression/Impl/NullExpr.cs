using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class NullExpr<TMonad> : IExpression<TMonad>
    {
        public NullExpr(IMonad<TMonad> monad)
        {
            Monad = monad;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => Enumerable.Empty<IDependency>();

        public TMonad Evaluate(IContext<TMonad> context)
            => Monad.Unit;
    }
}
