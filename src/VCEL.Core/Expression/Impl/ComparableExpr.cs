using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public abstract class ComparableExpr<T> : BinaryExprBase<T>
    {
        public ComparableExpr(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }

        protected override T Evaluate(object lv, object rv)
        {
            if(lv?.GetType() != rv?.GetType()
                && !TryUpCast(lv, rv, out lv, out rv))
            {
                return Monad.Unit;
            }

            return lv is IComparable cl
                && rv is IComparable cr
                ? Evaluate(cl, cr)
                : Monad.Unit;
        }

        protected abstract T Evaluate(IComparable l, IComparable r);

        protected bool TryUpCast(
            object l,
            object r,
            out object ol,
            out object or)
            => TryUpCastOneSide(l, r, out ol, out or)
                || TryUpCastOneSide(r, l, out or, out ol);

        private bool TryUpCastOneSide(
            object l,
            object r,
            out object ol,
            out object or)
        {
            ol = l;
            or = r;

            switch(l)
            {
                case double _ when r is decimal dr:
                    or = (double)dr;
                    return true;
                case double _ when r is float fr:
                    or = (double)fr;
                    return true;
                case double _ when r is long lr:
                    or = (double)lr;
                    return true;
                case double _ when r is int ir:
                    or = (double)ir;
                    return true;
                case double _ when r is short sr:
                    or = (double)sr;
                    return true;
                case double _ when r is byte br:
                    or = (double)br;
                    return true;
                case decimal _ when r is float dr:
                    or = (decimal)dr;
                    return true;
                case decimal _ when r is long lr:
                    or = (decimal)lr;
                    return true;
                case decimal _ when r is int ir:
                    or = (decimal)ir;
                    return true;
                case decimal _ when r is short sr:
                    or = (decimal)sr;
                    return true;
                case decimal _ when r is byte br:
                    or = (decimal)br;
                    return true;
                case float fl when r is long lr:
                    ol = (double)fl;
                    or = (double)lr;
                    return true;
                case float _ when r is int ir:
                    or = (float)ir;
                    return true;
                case float _ when r is short sr:
                    or = (float)sr;
                    return true;
                case float _ when r is byte br:
                    or = (float)br;
                    return true;
                case long _ when r is int ir:
                    or = (long)ir;
                    return true;
                case long _ when r is short sr:
                    or = (long)sr;
                    return true;
                case long _ when r is byte br:
                    or = (long)br;
                    return true;
                case int _ when r is short sr:
                    or = (int)sr;
                    return true;
                case int _ when r is byte br:
                    or = (int)br;
                    return true;
                case short _ when r is byte br:
                    or = (short)br;
                    return true;
            }

            return false;
        }
    }
}
