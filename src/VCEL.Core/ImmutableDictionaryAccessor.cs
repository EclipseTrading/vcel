namespace VCEL
{
    internal class OverrideAccessor<TMonad> : IValueAccessor<TMonad>
    {
        private readonly OverrideContext<TMonad> overrideContext;
        private readonly string propName;

        public OverrideAccessor(OverrideContext<TMonad> overrideContext, string propName)
        {
            this.overrideContext = overrideContext;
            this.propName = propName;
        }

        public TMonad GetValue(IContext<TMonad> _)
        {
            return overrideContext.Overrides[propName];
        }
    }
}