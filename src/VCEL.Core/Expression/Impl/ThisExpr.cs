using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    internal class ThisExpr<T> : IExpression<T>
    {
        public ThisExpr(IMonad<T> monad)
        {
            this.Monad = monad;
        }

        public IMonad<T> Monad { get; }

        public T Evaluate(IContext<T> context)
        {
            return context.Value;
        }
    }
}