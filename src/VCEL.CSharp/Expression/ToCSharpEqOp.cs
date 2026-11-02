using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpEqOp : BinaryExprBase<string>
{
    public ToCSharpEqOp(
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> right)
        : base(monad, left, right)
    {
    }

    public override string Evaluate(object? lv, object? rv)
    {
        // Keep the call statically bound so dynamic operands do not box its Boolean result.
        return $"TypeOperation.EqualsChecked((object)({lv}), (object)({rv}))";
    }
}