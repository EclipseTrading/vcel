using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class NotExpr<TMonad> : IExpression<TMonad>
    {
        private readonly IExpression<TMonad> expr;

        public NotExpr(
            IMonad<TMonad> monad, 
            IExpression<TMonad> expr)
        {
            this.Monad = monad;
            this.expr = expr;
        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var result = expr.Evaluate(context);
            return Monad.Bind(result, 
                o => Monad.Lift(o is bool b ? !b : (bool?)null));
        }

        public override string ToString() => $"!{expr}";
    }
}
