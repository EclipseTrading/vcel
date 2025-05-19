using System;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class GreaterOrEqual<T> : BinaryExprBase<T>
{
    public GreaterOrEqual(
        IMonad<T> monad,
        IExpression<T> left,
        IExpression<T> right)
        : base(monad, left, right)
    {
    }

    public override T Evaluate(object? lv, object? rv)
    {
        if(lv is IComparable cl && rv?.GetType() == lv?.GetType())
        {
            return Monad.Lift(cl.CompareTo(rv) >= 0);
        }

        if (lv == null || rv == null)
        {
            return Monad.Unit;
        }

        if (UpCastExtensions.UpCast(ref lv, ref rv) && lv is IComparable lc)
        {
            return Monad.Lift(lc.CompareTo(rv) >= 0);
        }

        return Monad.Unit;
    }
}