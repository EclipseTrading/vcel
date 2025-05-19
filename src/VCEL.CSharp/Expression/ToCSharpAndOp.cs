using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpAndOp : BinaryExprBase<string>
{
    public ToCSharpAndOp(
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> right)
        : base(monad, left, right)
    {
    }

    public override string Evaluate(object? lv, object? rv)
    {
        return Equals(lv, "null")
            ? "null"
            : $"({lv} == true) ? {rv} : false";
    }
}