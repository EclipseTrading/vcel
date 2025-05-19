using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public abstract class BooleanExpr<TMonad> : IExpression<TMonad>
{
    public BooleanExpr(
        IMonad<TMonad> monad,
        IExpression<TMonad> left,
        IExpression<TMonad> right)
    {
        Monad = monad;
        Left = left;
        Right = right;
        BindFunc = Bind;
        BindRightFunc = BindRight;
    }

    private Func<object?, IContext<TMonad>, TMonad> BindFunc { get; }
    private Func<object?,IContext<TMonad>,bool,TMonad> BindRightFunc { get; }

    public IExpression<TMonad> Left { get; }
    public IExpression<TMonad> Right { get; }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies
        => Left.Dependencies.Union(Right.Dependencies).Distinct();

    public TMonad Evaluate(IContext<TMonad> context)
    {
        var l = Left.Evaluate(context);
        return Monad.Bind(l, context, BindFunc);
    }

    private TMonad Bind(object? lv, IContext<TMonad> context)
    {
        return lv is bool lb
            ? ShortCircuit(lb, out var res)
                ? Monad.Lift(res)
                : Monad.Bind(
                    Right.Evaluate(context),
                    context,
                    BindRightFunc,
                    lb)
            : Monad.Unit;
    }

    private TMonad BindRight(object? rv, IContext<TMonad> context, bool lb)
    {
        return rv is bool rb
            ? Monad.Lift(Evaluate(lb, rb))
            : Monad.Unit;
    }

    public abstract bool Evaluate(bool l, bool r);
    public abstract bool ShortCircuit(bool l, out bool res);
}