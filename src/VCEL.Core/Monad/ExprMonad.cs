using System;

namespace VCEL.Monad
{
    public class ExprMonad : IMonad<object?>
    {
        public object? Unit { get; }
        public object? Lift<TValue>(TValue value)
        {
            return value;
        }
        public object? Bind(object? m, Func<object?, object?> f)
        {
            return f(m);
        }

        public object? Bind(object? m, IContext<object?> context, Func<object?, IContext<object?>, object?> f)
        {
            return f(m, context);
        }

        public object? Bind<TValue>(object? m, IContext<object?> context, Func<object?, IContext<object?>, TValue, object?> f, TValue value)
        {
            return f(m, context, value);
        }

        public object? Bind(object? m, object? b, Func<object?, object?, object?> f)
        {
            return f(m, b);
        }

        public static ExprMonad Instance { get; } = new ExprMonad();
    }
}
