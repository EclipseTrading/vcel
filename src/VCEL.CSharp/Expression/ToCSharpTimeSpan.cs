using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpTimeSpan : IExpression<string>
{
    private TimeSpan timeSpan;

    public ToCSharpTimeSpan(IMonad<string> monad, TimeSpan timeSpan)
    {
        this.Monad = monad;
        this.timeSpan = timeSpan;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return $"new TimeSpan({timeSpan.Ticks})";
    }
}