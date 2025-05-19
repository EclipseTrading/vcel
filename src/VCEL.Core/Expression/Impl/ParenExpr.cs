using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class ParenExpr<TMonad> : IExpression<TMonad>
{
    public IExpression<TMonad> Expr { get; }

    public ParenExpr(IMonad<TMonad> monad, IExpression<TMonad> expr)
    {
        Monad = monad;
        Expr = expr;
    }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies => Expr.Dependencies;

    public TMonad Evaluate(IContext<TMonad> context)
    {
        return Expr.Evaluate(context);
    }
}