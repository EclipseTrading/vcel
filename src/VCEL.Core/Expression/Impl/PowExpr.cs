using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class PowExpr<T> : BinaryExprBase<T>
    {
        public PowExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
        {
        }

        public override T Evaluate(object lv, object rv)
        {
            var l = lv;
            var r = rv;

            if (lv?.GetType() != rv?.GetType()
                && !UpCastEx.UpCast(ref l, ref r))
            {
                return Monad.Unit;
            }

            switch (l)
            {
                case double d:
                    return Monad.Lift(Math.Pow(d, (double)r));
                case int i:
                    return Monad.Lift(Math.Pow(i, (int)r));
                case long lo:
                    return Monad.Lift(Math.Pow(lo, (long)r));
                case decimal de:
                    return Monad.Lift(Math.Pow((double)de, (double)(decimal)r));
                case float f:
                    return Monad.Lift(Math.Pow(f, (float)r));
                case short s:
                    return Monad.Lift(Math.Pow(s, (short)r));
                case byte b:
                    return Monad.Lift(Math.Pow(b, (byte)r));
            }
            return Monad.Unit;
        }
    }
}
