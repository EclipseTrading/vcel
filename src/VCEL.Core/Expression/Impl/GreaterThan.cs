using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class GreaterThan<T> : ComparableExpr<T>
    {
        public GreaterThan(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }

        protected override T Evaluate(IComparable lv, IComparable rv)
            => Monad.Lift(lv.CompareTo(rv) > 0);
    }
}
