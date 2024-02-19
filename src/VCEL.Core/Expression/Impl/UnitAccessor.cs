using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

internal readonly struct UnitAccessor<TMonad> : IValueAccessor<TMonad>
{
    private readonly IMonad<TMonad> monad;

    public UnitAccessor(IMonad<TMonad> monad)
    {
        this.monad = monad;
    }

    public TMonad GetValue(IContext<TMonad> o)
    {
        return monad.Unit;
    }
}