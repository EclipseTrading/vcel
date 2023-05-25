namespace VCEL;

internal class OverrideAccessor<TMonad> : IValueAccessor<TMonad>
{
    private readonly string propName;
    private IValueAccessor<TMonad>? baseAccessor;

    public OverrideAccessor(string propName)
    {
        this.propName = propName;
    }

    public TMonad GetValue(IContext<TMonad> ctx)
    {
        var overrideContext = ctx as OverrideContext<TMonad>;
        if (overrideContext?.Overrides.TryGetValue(propName, out var v) ?? false)
        {
            return v;
        }

        IValueAccessor<TMonad>? accessor = default;
        if (baseAccessor == null && (overrideContext?.BaseContext.TryGetAccessor(propName, out accessor) ?? false))
        {
            baseAccessor = accessor;
        }

        return baseAccessor == null || overrideContext == null
            ? ctx.Monad.Unit
            : baseAccessor.GetValue(overrideContext.BaseContext);
    }
}