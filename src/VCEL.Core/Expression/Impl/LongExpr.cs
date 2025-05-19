using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class LongExpr<TMonad> : ValueExpr<TMonad, long>
{
    public LongExpr(IMonad<TMonad> monad, long value) : base(monad, value)
    {
    }
}