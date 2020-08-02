using System.Collections;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class InExpr<T> : BinaryExprBase<T>
    {
        public InExpr(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
            : base(monad, left, right)
        {
        }

        protected override T Evaluate(object lv, object rv)
        {
            if(rv is IList rl)
            {
                return Monad.Lift(rl.Contains(lv));
            }
            return Monad.Lift(false);
        }
    }
}
