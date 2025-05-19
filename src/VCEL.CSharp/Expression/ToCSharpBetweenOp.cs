using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpBetweenOp : IExpression<string>
{
    public ToCSharpBetweenOp(
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> lower,
        IExpression<string> upper)
    {
        this.Monad = monad;
        Left = left;
        Lower = lower;
        Upper = upper;
    }

    public IMonad<string> Monad { get; }
    public IExpression<string> Left { get; }
    public IExpression<string> Lower { get; }
    public IExpression<string> Upper { get; }

    public IEnumerable<IDependency> Dependencies => Left.Dependencies.Union(Lower.Dependencies).Union(Upper.Dependencies);

    public string Evaluate(IContext<string> context)
    {
        var l = Left.Evaluate(context);
        var lw = Lower.Evaluate(context);
        var up = Upper.Evaluate(context);
        return $"(CSharpHelper.IsBetween<object>({l}, {lw}, {up}))";
    }
}