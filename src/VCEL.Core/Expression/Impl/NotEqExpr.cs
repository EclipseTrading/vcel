using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class NotEqExpr<T> : BinaryExprBase<T>
{
    public NotEqExpr(
        IMonad<T> monad,
        IExpression<T> left,
        IExpression<T> right)
        : base(monad, left, right)
    {
    }

    public override T Evaluate(object? lv, object? rv)
        => Monad.Lift(!TypeOperation.EqualsChecked(lv, rv));
}