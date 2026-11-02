using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpInOp : BinaryExprBase<string>
{
    public ToCSharpInOp(
        IMonad<string> monad,
        IExpression<string> l,
        IExpression<string> r)
        : base(monad, l, r)
    {
    }
    public override string Evaluate(object? lv, object? rv)
    {
        return Equals(rv, "null") ? "null" : $@"{rv}.Any(elem => TypeOperation.EqualsChecked({lv}, elem))";
    }

    public override string Evaluate(IContext<string> context)
        => Right is ToCSharpLiteralSetOp set
            ? set.Membership(Left.Evaluate(context))
            : base.Evaluate(context);
}