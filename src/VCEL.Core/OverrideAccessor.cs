using System;

namespace VCEL;

internal struct OverrideAccessor<TMonad> : IValueAccessor<TMonad>
{
    private readonly string propName;
    private IValueAccessor<TMonad>? baseAccessor;

    public OverrideAccessor(string propName)
    {
        this.propName = propName;
        baseAccessor = null;
    }

    public TMonad GetValue(IContext<TMonad> ctx)
    {
        TMonad? v;
        if (ctx is not OverrideContext<TMonad> overrideContext)
        {
            throw new InvalidOperationException("OverrideAccessor must be used within an OverrideContext");
        }

        if (overrideContext.Overrides.TryGetValue(propName, out v))
        {
            return v;
        }

        if (baseAccessor == null && overrideContext.BaseContext.TryGetAccessor(propName, out var accessor))
        {
            baseAccessor = accessor;
        }
        return baseAccessor == null
            ? ctx.Monad.Unit
            : baseAccessor.GetValue(overrideContext.BaseContext);
    }
}