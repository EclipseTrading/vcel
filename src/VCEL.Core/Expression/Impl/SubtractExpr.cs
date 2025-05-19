using System;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class SubtractExpr<T> : BinaryExprBase<T>
    {
        public SubtractExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
        {
        }

        public override T Evaluate(object? lv, object? rv)
        {
            var l = lv;
            var r = rv;

            if(lv is DateTime dt)
            {
                switch(rv)
                {
                    case DateTime rdt:
                        return Monad.Lift(dt.Subtract(rdt));
                    case TimeSpan rts:
                        return Monad.Lift(dt.Subtract(rts));
                    case DateTimeOffset rdto:
                        return Monad.Lift(dt.Subtract(rdto.DateTime));
                }
            }

            if(lv is DateTimeOffset dto)
            {
                switch (rv)
                {
                    case DateTimeOffset rdto:
                        return Monad.Lift(dto.Subtract(rdto));
                    case DateTime rdt:
                        return Monad.Lift(dto.Subtract(rdt));
                    case TimeSpan rts:
                        return Monad.Lift(dto.Subtract(rts));
                }
            } 

            if (lv?.GetType() != rv?.GetType()
                && !UpCastExtensions.UpCast(ref l!, ref r!))
            {
                return Monad.Unit;
            }

            switch (l)
            {
                case double d:
                    return Monad.Lift(d - (double)r!);
                case int i:
                    return Monad.Lift(i - (int)r!);
                case long lo:
                    return Monad.Lift(lo - (long)r!);
                case decimal de:
                    return Monad.Lift(de - (decimal)r!);
                case TimeSpan ts:
                    return Monad.Lift(ts - (TimeSpan)r!);
                case float de:
                    return Monad.Lift(de - (float)r!);
                case short de:
                    return Monad.Lift(de - (short)r!);
                case byte de:
                    return Monad.Lift(de - (byte)r!);
            }
            return Monad.Unit;
        }
    }
}
