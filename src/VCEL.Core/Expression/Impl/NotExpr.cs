using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class NotExpr<TMonad> : IExpression<TMonad>
{
    public IExpression<TMonad> Expr { get; }

    public NotExpr(
        IMonad<TMonad> monad,
        IExpression<TMonad> expr)
    {
        Monad = monad;
        this.Expr = expr;
    }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies => Expr.Dependencies;

    public TMonad Evaluate(IContext<TMonad> context)
    {
        var result = Expr.Evaluate(context);
        return Monad.Bind(result,
            o => Monad.Lift(o is bool b ? !b : (bool?)null));
    }

    public override string ToString() => $"!{Expr}";
}