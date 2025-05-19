using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpDateTimeOffSet : IExpression<string>
{
    private readonly long ticks;
    private readonly double offsetHour;

    public ToCSharpDateTimeOffSet(IMonad<string> monad, DateTimeOffset dateTimeOffset)
    {
        this.Monad = monad;
        this.ticks = dateTimeOffset.Ticks;
        this.offsetHour = dateTimeOffset.Offset.TotalHours;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return $"(new DateTimeOffset({this.ticks}, TimeSpan.FromHours({this.offsetHour})))";
    }
}