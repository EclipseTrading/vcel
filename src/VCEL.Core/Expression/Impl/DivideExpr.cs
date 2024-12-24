using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class DivideExpr<T> : BinaryExprBase<T>
    {
        public DivideExpr(IMonad<T> monad, IExpression<T> left, IExpression<T> right) : base(monad, left, right)
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
                    return Monad.Lift(d / (double)r!);
                case int i:
                    return Monad.Lift(i / (double)(int)r!);
                case long lo:
                    return Monad.Lift(lo / (double)(long)r!);
                case decimal de:
                    return Monad.Lift(de / (decimal)r!);
                case float de:
                    return Monad.Lift(de / (float)r!);
                case short de:
                    return Monad.Lift(de / (double)(short)r!);
                case byte de:
                    return Monad.Lift(de / (double)(byte)r!);
            }
            return Monad.Unit;
        }
    }
}
