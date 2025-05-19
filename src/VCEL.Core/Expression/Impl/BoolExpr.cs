using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class BoolExpr<TMonad> : ValueExpr<TMonad, bool>
{
    public BoolExpr(IMonad<TMonad> monad, bool value) : base(monad, value)
    {
    }
}