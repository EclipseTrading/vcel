using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpPowOp : IExpression<string>
{
    private readonly IExpression<string> l;
    private readonly IExpression<string> r;

    public ToCSharpPowOp(IMonad<string> monad, IExpression<string> l, IExpression<string> r)
    {
        this.Monad = monad;
        this.l = l;
        this.r = r;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return $"(VcelMath.Pow({l.Evaluate(context)}, {r.Evaluate(context)}))";
    }
}