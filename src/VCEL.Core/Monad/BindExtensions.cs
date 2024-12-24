using System;
namespace VCEL.Monad
{
    public static class BindExtensions
    {
        public static TMonad Bind<TMonad>(TMonad a, TMonad b, Func<object?, object?, TMonad> f, IMonad<TMonad> m)
        {
            return m.Bind(a, Bind);

            TMonad Bind(object? av)
            {
                return m.Bind(b, Bind2);

                TMonad Bind2(object? bv)
                {
                    return f(av, bv);
                }
            }
        }
    }
}
