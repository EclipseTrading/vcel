using System;

namespace VCEL.Monad
{
    public class ExprMonad : IMonad<object>
    {
        public object Unit { get; } = null;

        public object Bind(object m, Func<object, object> f)
            => f(m);

        public object Lift(object value) => value;

        public static ExprMonad Instance { get; } = new ExprMonad();
    }
}
