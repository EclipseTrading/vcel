using System.Collections;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class SpreadExpr<TMonad> : IExpression<TMonad>
    {
        public SpreadExpr(IMonad<TMonad> monad, IExpression<TMonad> list)
        {
            Monad = monad;
            List = list;
        }

        public IMonad<TMonad> Monad { get;}
        public IExpression<TMonad> List { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var value = List.Evaluate(context);
            return Monad.Bind(value, v => v is ICollection c ? Monad.Lift(c) : Monad.Unit);
        }
    }
}
