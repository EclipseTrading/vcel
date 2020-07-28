using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class GreaterOrEqual<T> : ComparableExpr<T>
    {
        public GreaterOrEqual(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }

        protected override T Evaluate(IComparable lv, IComparable rv)
            => Monad.Lift(lv.CompareTo(rv) >= 0);
    }
}
