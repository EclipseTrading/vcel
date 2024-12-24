using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class BetweenExpr<T> : IExpression<T>
    {
        public BetweenExpr(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> lower,
            IExpression<T> upper)
        {
            Monad = monad;
            Left = left;
            Lower = lower;
            Upper = upper;
        }
        public IExpression<T> Left { get; }
        public IExpression<T> Lower { get; }
        public IExpression<T> Upper { get; }
        public IMonad<T> Monad { get; }
        public IEnumerable<IDependency> Dependencies
            => Left.Dependencies.Union(Lower.Dependencies).Union(Upper.Dependencies).Distinct();

        public T Evaluate(IContext<T> context)
        {
            var lv = Left.Evaluate(context);
            var lower = Lower.Evaluate(context);
            var upper = Upper.Evaluate(context);
            return Monad.Bind(lv, left => Monad.Bind(lower, upper, (l, u) => EvaluateBetween(left, l, u)));
        }

        private T EvaluateBetween(object? l, object? first, object? second)
        {
            if (l is IComparable left)
            {
                int frCmp = -1;
                int toCmp = 1;

                if (first?.GetType() == l?.GetType() || UpCastExtensions.UpCast(ref first!, ref l!))
                {
                    frCmp = left.CompareTo(first);
                }

                if (second?.GetType() == l?.GetType() || UpCastExtensions.UpCast(ref second!, ref l!))
                {
                    toCmp = left.CompareTo(second);
                }

                return Monad.Lift(frCmp >= 0 && toCmp <= 0);
            }
            return Monad.Lift(false);
        }
    }
}
