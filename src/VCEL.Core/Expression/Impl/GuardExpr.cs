using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class GuardExpr<TMonad> : IExpression<TMonad>
{
    public IReadOnlyList<(IExpression<TMonad> Cond, IExpression<TMonad> Res)> Clauses { get; }
    public IExpression<TMonad>? Otherwise { get; }

    public GuardExpr(
        IMonad<TMonad> monad,
        IReadOnlyList<(IExpression<TMonad>, IExpression<TMonad>)> clauses,
        IExpression<TMonad>? otherwise)
    {
        Monad = monad;
        Clauses = clauses;
        Otherwise = otherwise;
    }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies
        => Clauses
            .SelectMany(c => c.Cond.Dependencies.Union(c.Res.Dependencies))
            .Union(Otherwise?.Dependencies ?? Array.Empty<IDependency>())
            .Distinct();


    public TMonad Evaluate(IContext<TMonad> context)
    {
        return Next(Clauses.GetEnumerator());

        TMonad Eval(IEnumerator<(IExpression<TMonad> Cond, IExpression<TMonad> Res)> ce)
        {
            var result = ce.Current.Cond.Evaluate(context);
            return Monad.Bind(
                result,
                BindNext);
            TMonad BindNext(object? r)
                => r is bool b
                    ? (b ? ce.Current.Res.Evaluate(context) : Next(ce))
                    : Monad.Unit;
        }

        TMonad Next(IEnumerator<(IExpression<TMonad>, IExpression<TMonad>)> ce)
            => ce.MoveNext()
                ? Eval(ce)
                : Otherwise == null
                    ? Monad.Unit
                    : Otherwise.Evaluate(context);
    }
}