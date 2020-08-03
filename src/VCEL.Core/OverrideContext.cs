using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL
{
    public class OverrideContext<TMonad> : IContext<TMonad>
    {
        private readonly IContext<TMonad> context;

        public OverrideContext(IContext<TMonad> context, ImmutableDictionary<string, TMonad> overrides)
        {
            this.context = context;
            Overrides = overrides;
        }

        public IMonad<TMonad> Monad => context.Monad;

        public ImmutableDictionary<string, TMonad> Overrides { get; }

        public IContext<TMonad> OverrideName(string name, TMonad br)
        {
            return new OverrideContext<TMonad>(
                context,
                Overrides.SetItem(name, br));
        }

        public bool TryGetAccessor(string propName, out IValueAccessor<TMonad> accessor)
            => Overrides.ContainsKey(propName)
                ? TryGetOverride(propName, out accessor)
                : context.TryGetAccessor(propName, out accessor);

        private bool TryGetOverride(string propName, out IValueAccessor<TMonad> accessor)
        {
            accessor = new OverrideAccessor<TMonad>(this, propName);
            return true;
        }

        public TMonad Value => context.Value;
    }
}
