using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class InExpr<T> : BinaryExprBase<T>
{
    public InExpr(
        IMonad<T> monad,
        IExpression<T> left,
        IExpression<T> right)
        : base(monad, left, right)
    {
    }

    public override T Evaluate(object? lv, object? rv)
        => rv switch
        {
            ICollection<object?> list => Monad.Lift(list.Contains(lv)),
            string s when lv is string ls => Monad.Lift(s.Contains(ls)),
            _ => Monad.Unit
        };
}