using System;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class DateTimeOffsetExpr<TMonad> : ValueExpr<TMonad, DateTimeOffset>
{
    public DateTimeOffsetExpr(IMonad<TMonad> monad, DateTimeOffset value) : base(monad, value)
    {
    }
}