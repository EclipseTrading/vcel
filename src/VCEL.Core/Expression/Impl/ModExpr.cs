using System;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ModExpr<T> : BinaryExprBase<T>
    {
        public ModExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
        {
        }

        public override T Evaluate(object? lv, object? rv)
        {
            var l = lv;
            var r = rv;

            if (lv?.GetType() != rv?.GetType()
                && !UpCastEx.UpCast(ref l!, ref r!))
            {
                return Monad.Unit;
            }
            
            switch (l)
            {
                case double d:
                    return Monad.Lift(d % (double)r!);
                case int i:
                    return Monad.Lift(i % (int)r!);
                case long lo:
                    return Monad.Lift(lo % (long)r!);
                case decimal de:
                    return Monad.Lift(de % (decimal)r!);
                case float f:
                    return Monad.Lift(f % (float)r!);
                case short s:
                    return Monad.Lift(s % (short)r!);
                case byte b:
                    return Monad.Lift(b % (byte)r!);
            }
            return Monad.Unit;
        }
    }
}