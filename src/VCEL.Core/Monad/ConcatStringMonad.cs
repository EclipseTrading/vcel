using VCEL.Monad;

namespace VCEL.Expression
{
    using System;

    public class ConcatStringMonad : IMonad<string>
    {
        public string Unit => "";
        public string Bind(string m, Func<object, string> f) => f(m);
        public string Lift<TValue>(TValue value) => value?.ToString() ?? string.Empty;

        public static ConcatStringMonad Instance { get; } = new ConcatStringMonad();

        public string Bind(string m, string b, Func<object, object, string> f)
        {
            return f(m, b);
        }

        public string Bind(string m, IContext<string> context, Func<object?, IContext<string>, string> f)
        {
            return f(m, context);
        }

        public string Bind<TValue>(string m, IContext<string> context, Func<object?, IContext<string>, TValue, string> f, TValue value)
        {
            return f(m, context, value);
        }
    }
}
