using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class TimeSpanExpr<TMonad> : ValueExpr<TMonad, TimeSpan>
{
    public TimeSpanExpr(IMonad<TMonad> monad, TimeSpan value) : base(monad, value)
    {
    }
}