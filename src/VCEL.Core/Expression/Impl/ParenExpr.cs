using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ParenExpr<TMonad> : IExpression<TMonad>
    {
        private readonly IExpression<TMonad> expr;

        public ParenExpr(IMonad<TMonad> monad, IExpression<TMonad> expr)
        {
            Monad = monad;
            this.expr = expr;
        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            return expr.Evaluate(context);
        }
    }
}
