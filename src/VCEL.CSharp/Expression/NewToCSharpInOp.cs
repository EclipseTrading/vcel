using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class NewToCSharpInOp : BinaryExprBase<string>
{
    public NewToCSharpInOp(
        IMonad<string> monad,
        IExpression<string> l,
        IExpression<string> r)
        : base(monad, l, r)
    {
    }
    public override string Evaluate(object? lv, object? rv)
    {
        return Equals(rv, "null") ? "null" : $@"{rv}.Contains({lv})";
    }
}