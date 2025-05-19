using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpMemberOp : IExpression<string>
{
    public ToCSharpMemberOp(
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> right)
    {
        Monad = monad;
        Left = left;
        Right = right;
    }

    private IExpression<string> Left { get; }
    private IExpression<string> Right { get; }
    public IMonad<string> Monad { get; }
    public IEnumerable<IDependency> Dependencies
        => Left.Dependencies.Union(Right.Dependencies).Distinct();

    public string Evaluate(IContext<string> context)
    {
        var l = Left.Evaluate(context);
        var r = Right.Evaluate(context);
        return r.Replace(context.Value, l);
    }
}