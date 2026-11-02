using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpNotOp(IMonad<string> monad, IExpression<string> operand) : IExpression<string>
{
    internal IExpression<string> Operand { get; } = operand;
    public IMonad<string> Monad { get; } = monad;
    public IEnumerable<IDependency> Dependencies => [];
    public string Evaluate(IContext<string> context) => $"!{Operand.Evaluate(context)}";
}
