using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class LetExpr<TMonad> : IExpression<TMonad>
{
    private readonly HashSet<string> bindingNames;
    public IReadOnlyList<(string, IExpression<TMonad>)> Bindings { get; }
    public IExpression<TMonad> Expr { get; }

    public LetExpr(
        IMonad<TMonad> monad,
        IReadOnlyList<(string, IExpression<TMonad>)> bindings,
        IExpression<TMonad> expr)
    {
        Monad = monad;
        Bindings = bindings;
        Expr = expr;
        bindingNames = new HashSet<string>(bindings.Select(b => b.Item1));
    }

    public IMonad<TMonad> Monad { get; }

    public IEnumerable<IDependency> Dependencies
        => Bindings
            .SelectMany(b => b.Item2.Dependencies)
            .Union(Expr
                .Dependencies
                .Where(d => !(d is PropDependency p && bindingNames.Contains(p.Name))))
            .Distinct();

    public TMonad Evaluate(IContext<TMonad> context)
    {
        var ctx = context;
        foreach (var (name, exp) in Bindings)
        {
            var br = exp.Evaluate(ctx);
            ctx = ctx.OverrideName(name, br);
        }
        return Expr.Evaluate(ctx);
    }
}