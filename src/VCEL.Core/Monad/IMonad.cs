using System;
namespace VCEL.Monad
{
    public interface IMonad<TMonad>
    {
        TMonad Unit { get; }
        TMonad Lift<TValue>(TValue value);
        TMonad Bind(TMonad m, Func<object?, TMonad> f);
        TMonad Bind(TMonad m, IContext<TMonad> context, Func<object?, IContext<TMonad>, TMonad> f);
        TMonad Bind<TValue>(TMonad m, IContext<TMonad> context, Func<object?, IContext<TMonad>, TValue, TMonad> f, TValue value);
        TMonad Bind(TMonad a, TMonad b, Func<object?, object?, TMonad> f);
        // TMonad Bind<TValue>(TMonad a, TMonad b, TValue valueA, Func<TValue, TValue, TMonad> f);
    }
}
