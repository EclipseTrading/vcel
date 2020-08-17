using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class UnaryMinusExpr<TMonad> : IExpression<TMonad>
    {
        private readonly IExpression<TMonad> expr;
        public UnaryMinusExpr(
           IMonad<TMonad> monad,
           IExpression<TMonad> expr)
        {
            Monad = monad;
            this.expr = expr;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => expr.Dependencies;

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var result = expr.Evaluate(context);
            return Monad.Bind(result, Bind);
            TMonad Bind(object o)
                => TryNegate(o, out var r) ? Monad.Lift(r) : Monad.Unit;

            bool TryNegate(object o, out object r)
            {
                switch(o)
                {
                    case byte b:
                        r = -b;
                        return true;
                    case short s:
                        r = -s;
                        return true;
                    case int i:
                        r = -i;
                        return true;
                    case long l:
                        r = -l;
                        return true;
                    case float f:
                        r = -f;
                        return true;
                    case double d:
                        r = -d;
                        return true;
                    case decimal de:
                        r = -de;
                        return true;
                    default:
                        r = null;
                        return false;
                }
            }
        }
    }
}
