using System;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Expression.Impl
{
    public class MaybeDivide : DivideExpr<Maybe<object>>
    {
        public MaybeDivide(
            IMonad<Maybe<object>> monad,
            IExpression<Maybe<object>> left,
            IExpression<Maybe<object>> right)
            : base(monad, left, right)
        {
        }

        public override Maybe<object> Evaluate(IContext<Maybe<object>> context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);

            return Monad.Bind(l, r, Eval);

            Maybe<object> Eval(object? lv, object? rv)
            {
                return IsZeroOrNonNumeric(rv)
                    ? Maybe<object>.None
                    : Evaluate(lv, rv);
            }
        }

        private bool IsZeroOrNonNumeric(object? rv)
        {
            return !(rv is IConvertible)
                || rv is double d && d == 0.0
                || rv is int i && i == 0
                || rv is float f && f == 0
                || rv is short s && s == 0
                || rv is byte b && b == 0;
        }
    }
}
