namespace VCEL
{
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
            if (overrideContext.Overrides.TryGetValue(propName, out var v))
            {
                return v;
            }
            if(baseAccessor == null && overrideContext.BaseContext.TryGetAccessor(propName, out var accessor))
            {
                baseAccessor = accessor;
            }
            return baseAccessor == null
                ? ctx.Monad.Unit
                : baseAccessor.GetValue(overrideContext.BaseContext);
        }
    }
}