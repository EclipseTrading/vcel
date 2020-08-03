using System;
using VCEL.Core.Expression.Op;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Expression.Impl
{
    public class MaybeDivide : IExpression<Maybe<object>>
    {
        public MaybeDivide(
            IBinaryOperator op,
            IExpression<Maybe<object>> left,
            IExpression<Maybe<object>> right)
        {
            Op = op;
            Left = left;
            Right = right;
        }

        public IExpression<Maybe<object>> Left { get; }
        public IExpression<Maybe<object>> Right { get; }
        public IBinaryOperator Op { get; }

        public IMonad<Maybe<object>> Monad => MaybeMonad.Instance;

        public Maybe<object> Evaluate(IContext<Maybe<object>> context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);

            return Monad.Bind(
                r,
                rv => IsZeroOrNonNumeric(rv) 
                        ? Maybe<object>.None
                        : Monad.Bind(
                            l,
                            lv => new Maybe<object>(Op.Evaluate(lv, rv))));
        }

        private bool IsZeroOrNonNumeric(object rv)
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
