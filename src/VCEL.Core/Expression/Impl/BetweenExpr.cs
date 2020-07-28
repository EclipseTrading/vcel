using System;
using System.Collections;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class BetweenExpr<T> : BinaryExprBase<T>
    {
        public BetweenExpr(
            IMonad<T> monad,
            IExpression<T> left, 
            IExpression<T> right) 
            : base(monad, left, right)
        {
        }

        protected override T Evaluate(object lv, object rv)
        {
            if(lv is IComparable v
                && rv is IList l
                && l.Count == 2
                && l[0] is IComparable from
                && l[1] is IComparable to)
            {
                var frCmp = v.CompareTo(from);
                var toCmp = v.CompareTo(to);
                return Monad.Lift(frCmp >= 0 && toCmp <= 0);
            }
            return Monad.Lift(false);
        }
    }
}
