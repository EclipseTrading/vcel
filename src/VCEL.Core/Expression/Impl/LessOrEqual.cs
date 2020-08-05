using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class LessOrEqual<T> : BinaryExprBase<T>
    {
        public LessOrEqual(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }
        public override T Evaluate(object l, object r)
        {
            if (l is IComparable cl && r.GetType() == l?.GetType())
            {
                return Monad.Lift(cl.CompareTo(r) <= 0);
            }

            if (l == null || r == null)
            {
                return Monad.Unit;
            }

            if (UpCastEx.UpCast(ref l, ref r) && l is IComparable lc)
            {
                return Monad.Lift(lc.CompareTo(r) <= 0);
            }

            return Monad.Unit;
        }
    }
}
