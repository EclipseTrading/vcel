using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpModOp : IExpression<string>
{
    private readonly IExpression<string> l;
    private readonly IExpression<string> r;

    public ToCSharpModOp(IMonad<string> monad, IExpression<string> l, IExpression<string> r)
    {
        this.Monad = monad;
        this.l = l;
        this.r = r;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => l.Dependencies.Union(r.Dependencies);

    public string Evaluate(IContext<string> context)
    {
        return $"(VcelMath.Mod({l.Evaluate(context)}, {r.Evaluate(context)}))";
    }
}