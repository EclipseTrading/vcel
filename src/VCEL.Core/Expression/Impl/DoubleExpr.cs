using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class DoubleExpr<TMonad> : ValueExpr<TMonad, double>
{
    public DoubleExpr(IMonad<TMonad> monad, double value) : base(monad, value)
    {
    }
}