using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Expression.Impl
{
    public class MaybeNotEqExpr<TMonad> : IExpression<TMonad> where TMonad : Maybe<object>
    {
        public MaybeNotEqExpr(
            IMonad<TMonad> monad,
            IExpression<TMonad> left,
            IExpression<TMonad> right)
        {
            Monad = monad;
            Left = left;
            Right = right;
        }

        public IExpression<TMonad> Left { get; }
        public IExpression<TMonad> Right { get; }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies
            => Left.Dependencies.Union(Right.Dependencies).Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);

            if ((!l.HasValue || l.Value == null) && (!r.HasValue || r.Value == null))
            {
                return Monad.Lift(false);
            }

            return Monad.Lift(!Equals(l.Value, r.Value));
        }
    }
}
