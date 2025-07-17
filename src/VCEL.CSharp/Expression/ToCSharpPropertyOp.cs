using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpPropertyOp : IExpression<string>
{

    public ToCSharpPropertyOp(string name, IMonad<string> monad)
    {
        Name = name;
        this.Monad = monad;
    }

    public string Name { get; }
    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => [];

    public string Evaluate(IContext<string> context)
    {
        if (context.TryGetAccessor(Name, out var accessor))
        {
            var result = accessor.GetValue(context);
            return result;
        }

        return Name;
    }
}