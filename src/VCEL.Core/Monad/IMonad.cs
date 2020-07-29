using System;

namespace VCEL.Monad
{
    public interface IMonad<TMonad>
    {
        TMonad Unit { get; }
        TMonad Lift(object value);
        TMonad Bind(TMonad m, Func<object, TMonad> f);
    }
}
