using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;
public class VariableExpr<TMonad> : IExpression<TMonad>
{
    private readonly TMonad liftedValue;

    public VariableExpr(IMonad<TMonad> monad, string name)
    {
        Monad = monad;
        Name = name;
        liftedValue = Monad.Lift(name);
    }

    public IMonad<TMonad> Monad { get; }
    public string Name { get; }

    public IEnumerable<IDependency> Dependencies => Enumerable.Empty<IDependency>();

    public TMonad Evaluate(IContext<TMonad> _)
    {
        return liftedValue;
    }
}