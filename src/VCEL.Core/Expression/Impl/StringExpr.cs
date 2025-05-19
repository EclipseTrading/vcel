using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class StringExpr<TMonad> : ValueExpr<TMonad, string>
{
    public StringExpr(IMonad<TMonad> monad, string value) : base(monad, value)
    {
    }
}