using System;

namespace VCEL.Monad
{
    public class ExprMonad : IMonad<object>
    {
        public object Unit { get; } = null;
        public object Lift(object value) 
        {
            return value; 
        }
        public object Bind(object m, Func<object, object> f)
        {
            return f(m);
        }

        public object Bind(object a, object b, Func<object, object, object> f)
        {
            return f(a, b);
        }

        public static ExprMonad Instance { get; } = new ExprMonad();
    }
}
