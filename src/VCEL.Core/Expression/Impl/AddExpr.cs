using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class AddExpr<T> : BinaryExprBase<T>
    {
        public AddExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
        {
        }

        public override T Evaluate(object lv, object rv)
        {
            var l = lv;
            var r = rv;

            if(lv is DateTime dt)
            {
                switch(rv)
                {
                    case TimeSpan rts:
                        return Monad.Lift(dt.Add(rts));
                    case int days:
                        return Monad.Lift(dt.AddDays(days));
                    case double days:
                        return Monad.Lift(dt.AddDays(days));
                }
            }

            if (lv is DateTimeOffset dto)
            {
                switch (rv)
                {
                    case TimeSpan rts:
                        return Monad.Lift(dto.Add(rts));
                    case int days:
                        return Monad.Lift(dto.AddDays(days));
                    case double days:
                        return Monad.Lift(dto.AddDays(days));
                }
            }

            if (l is string || r is string)
            {
                return Monad.Lift("" + l + r);
            }

            if (lv?.GetType() != rv?.GetType()
                && !UpCastEx.UpCast(ref l, ref r))
            {
                return Monad.Unit;
            }

            switch (l)
            {
                case double d:
                    return Monad.Lift(d + (double)r);
                case int i:
                    return Monad.Lift(i + (int)r);
                case long lo:
                    return Monad.Lift(lo + (long)r);
                case decimal de:
                    return Monad.Lift(de + (decimal)r);
                case TimeSpan ts:
                    return Monad.Lift(ts + (TimeSpan)r);
                case float de:
                    return Monad.Lift(de + (float)r);
                case short de:
                    return Monad.Lift(de + (short)r);
                case byte de:
                    return Monad.Lift(de + (byte)r);
            }

            return Monad.Unit;
        }
    }
}
