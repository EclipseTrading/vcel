using VCEL.Core.Expression.Op;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class BinaryExpr<T> : BinaryExprBase<T>
    {
        public BinaryExpr(
            IBinaryOperator op,
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
            this.Op = op;
        }

        public IBinaryOperator Op { get; }

        protected override T Evaluate(object lv, object rv)
            => Monad.Lift(Op.Evaluate(lv, rv));

        public override string ToString() => $"{Left} {Op} {Right}";
    }
}
