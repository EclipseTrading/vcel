using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal class ToJsDateTimeOffSet : IExpression<string>
{
    private DateTimeOffset dateTimeOffset;

    public ToJsDateTimeOffSet(IMonad<string> monad, DateTimeOffset dateTimeOffset)
    {
        this.Monad = monad;
        this.dateTimeOffset = dateTimeOffset;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        var timeSinceEpoch = dateTimeOffset.ToUnixTimeMilliseconds();
        return $"(new Date({timeSinceEpoch}))";
    }
}