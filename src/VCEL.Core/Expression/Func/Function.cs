using System;
using System.Collections.Generic;

namespace VCEL.Core.Expression.Func;

public class Function<TMonad>
{
    public Function(Func<object?[], IContext<TMonad>, object?> func, params IDependency[] dependencies)
    {
        this.Func = func;
        this.Dependencies = dependencies;
    }

    public Func<object?[], IContext<TMonad>, object?> Func { get; }
    public IEnumerable<IDependency> Dependencies { get; }
}