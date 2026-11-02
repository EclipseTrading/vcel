using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpUnaryMinus(IMonad<string> monad, IExpression<string> operand) : IExpression<string>
{
    public IMonad<string> Monad { get; } = monad;
    internal IExpression<string> Operand { get; } = operand;
    public IEnumerable<IDependency> Dependencies => Operand.Dependencies;

    public string Evaluate(IContext<string> context) => CSharpArithmeticCode.Evaluate(this, context);
}
