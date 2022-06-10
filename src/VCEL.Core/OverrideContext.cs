using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
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

        public bool TryGetAccessor(string propName, [NotNullWhen(true)] out IValueAccessor<TMonad>? accessor)
        {
            accessor = new OverrideAccessor<TMonad>(propName);
            return true;
        }

        public bool TryGetContext(object o, [NotNullWhen(true)] out IContext<TMonad>? context)
        {
            context = this.context.TryGetContext(o, out var c)
                ? new OverrideContext<TMonad>(c, this.Overrides)
                : null;
            return context != null;
        }


        public TMonad Value => context.Value;

        public IContext<TMonad> BaseContext => context;
    }
}
