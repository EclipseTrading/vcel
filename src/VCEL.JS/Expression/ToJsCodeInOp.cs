using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal class ToJsCodeInOp : BinaryExprBase<string>
{
    public ToJsCodeInOp(
        IMonad<string> monad,
        IExpression<string> l,
        IExpression<string> r)
        : base(monad, l, r)
    {
    }

    public override string Evaluate(object? lv, object? rv)
    {
        return $"{rv}.includes({lv})";
    }
}