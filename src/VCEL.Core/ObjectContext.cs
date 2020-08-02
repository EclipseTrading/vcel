using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL
{
    public class ObjectContext<TMonad> : IContext<TMonad>
    {
        public IMonad<TMonad> Monad { get; }

        public object Object { get; }

        public ObjectContext(IMonad<TMonad> monad, object obj)
        {
            Monad = monad;
            Object = obj;
        }

        public virtual IContext<TMonad> OverrideName(string name, TMonad br)
            => new OverrideContext<TMonad>(
                this,
                ImmutableDictionary<string, TMonad>.Empty.SetItem(name, br));

        public virtual bool TryGetAccessor(string propName, out IValueAccessor<TMonad> accessor)
        {
            var type = Object?.GetType();
            var prop = type?.GetProperty(propName);
            if(prop == null)
            {
                accessor = null;
                return false;
            }

            accessor = new PropertyValueAccessor<TMonad>(Monad, prop);
            return true;
        }
    }
}
