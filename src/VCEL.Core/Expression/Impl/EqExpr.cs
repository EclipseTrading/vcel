using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class EqExpr<T> : BinaryExprBase<T>
    {
        public EqExpr(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }

        public override T Evaluate(object lv, object rv)
        {
            return Monad.Lift(TypeOperation.EqualsChecked(lv, rv));
        }
    }
}
