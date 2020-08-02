using System;
using VCEL.Core.Expression.Op;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class BooleanExpr<TMonad> : IExpression<TMonad>
    {
        private readonly Func<TMonad, TMonad> castBool;

        public BooleanExpr(
            BooleanOperator op,
            IMonad<TMonad> monad,
            IExpression<TMonad> left,
            IExpression<TMonad> right,
            Func<TMonad, TMonad> castBool)
        {
            Op = op;
            Monad = monad;
            Left = left;
            Right = right;
            this.castBool = castBool;
        }

        public BooleanOperator Op { get; }
        public IExpression<TMonad> Left { get; }
        public IExpression<TMonad> Right { get; }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);
            return Monad.Bind(l, BindL);

            TMonad BindL(object lv)
            {
                return lv is bool lb ? Monad.Bind(r, BindR) : Monad.Unit;
                TMonad BindR(object rv)
                {
                    return rv is bool rb
                        ? Monad.Lift(Op.Evaluate(lb, rb))
                        : Monad.Unit;
                }
            }
        }

        public override string ToString() => $"{Left} {Op} {Right}";
    }
}
