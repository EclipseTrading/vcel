using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL;

public interface IExpression<TMonad>
{
    TMonad Evaluate(IContext<TMonad> context);
    IMonad<TMonad> Monad { get; }
    IEnumerable<IDependency> Dependencies { get; }
}