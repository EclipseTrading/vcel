using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ValueExpr<TMonad, TValue> : IExpression<TMonad>
    {
        private readonly TMonad liftedValue;

        public TValue Value { get; }

        public ValueExpr(IMonad<TMonad> monad, TValue value)
        {
            Monad = monad;
            Value = value;
            liftedValue = Monad.Lift(value);
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => Enumerable.Empty<IDependency>();

        public TMonad Evaluate(IContext<TMonad> _)
        {
            return liftedValue;
        }
    }
}
