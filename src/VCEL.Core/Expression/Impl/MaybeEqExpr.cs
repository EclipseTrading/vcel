﻿using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Expression.Impl;

public class MaybeEqExpr<TMonad> : IExpression<TMonad> where TMonad : struct, IMaybe<object?>
{
    public MaybeEqExpr(
        IMonad<TMonad> monad,
        IExpression<TMonad> left,
        IExpression<TMonad> right)
    {
        Monad = monad;
        Left = left;
        Right = right;
    }

    public IExpression<TMonad> Left { get; }
    public IExpression<TMonad> Right { get; }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies
        => Left.Dependencies.Union(Right.Dependencies).Distinct();

    public TMonad Evaluate(IContext<TMonad> context)
    {
        var l = Left.Evaluate(context);
        var r = Right.Evaluate(context);

        if ((!l.HasValue || l.Value == null) && (!r.HasValue || r.Value == null))
        {
            return Monad.Lift(true);
        }

        return Monad.Lift(TypeOperation.EqualsChecked(l.Value, r.Value));
    }
}