using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public abstract class BinaryExprBase<T> : IExpression<T>
    {
        public BinaryExprBase(
          IMonad<T> monad,
          IExpression<T> left,
          IExpression<T> right)
        {
            Monad = monad;
            Left = left;
            Right = right;
        }
        public IExpression<T> Left { get; }
        public IExpression<T> Right { get; }

        public IMonad<T> Monad { get; }

        public virtual T Evaluate(IContext<T> context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);

            return Monad.Bind(l, BindL);

            T BindL(object lv)
            {
                return Monad.Bind(r, BindR);
                T BindR(object rv) => Evaluate(lv, rv);
            }
        }

        protected abstract T Evaluate(object lv, object rv);

    }
}
