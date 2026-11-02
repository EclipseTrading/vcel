using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpMatchesOp : BinaryExprBase<string>
{
    public ToCSharpMatchesOp(
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> right)
        : base(monad, left, right)
    {
    }

    public override string Evaluate(object? lv, object? rv)
    {
        return $"(CSharpHelper.Matches({lv}, {rv}))";
    }
}