using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class IntExpr<TMonad> : ValueExpr<TMonad, int>
{
    public IntExpr(IMonad<TMonad> monad, int value) : base(monad, value)
    {
    }
}