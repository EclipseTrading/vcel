using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpStringOp : IExpression<string>
{
    private readonly Func<IContext<string>, string> funcStr;

    public ToCSharpStringOp(Func<IContext<string>, string> funcStr, IMonad<string> monad)
    {
        this.funcStr = funcStr;
        this.Monad = monad;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return funcStr(context);
    }
}