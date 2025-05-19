using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class MultExpr<T> : BinaryExprBase<T>
    {
        public MultExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
        {
        }

        public override T Evaluate(object? lv, object? rv)
        {
            var l = lv;
            var r = rv;

            if (lv?.GetType() != rv?.GetType()
                && !UpCastExtensions.UpCast(ref l!, ref r!))
            {
                return Monad.Unit;
            }

            switch (l)
            {
                case double d:
                    return Monad.Lift(d * (double)r!);
                case int i:
                    return Monad.Lift(i * (int)r!);
                case long lo:
                    return Monad.Lift(lo * (long)r!);
                case decimal de:
                    return Monad.Lift(de * (decimal)r!);
                case float de:
                    return Monad.Lift(de * (float)r!);
                case short de:
                    return Monad.Lift(de * (short)r!);
                case byte de:
                    return Monad.Lift(de * (byte)r!);
            }
            return Monad.Unit;
        }
    }
}
