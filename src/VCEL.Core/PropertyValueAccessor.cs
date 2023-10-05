using System.Reflection;
using VCEL.Monad;

namespace VCEL
{
    public class PropertyValueAccessor<TMonad> : IValueAccessor<TMonad>
    {
        private bool propSet = false;
        private PropertyInfo? prop;
        private readonly IMonad<TMonad> monad;
        private readonly string propName;

        public PropertyValueAccessor(IMonad<TMonad> monad, string propName)
        {
            this.monad = monad;
            this.propName = propName;
        }

        public TMonad GetValue(IContext<TMonad> context)
        {
            if (context is not ObjectContext<TMonad> { Object: { } obj } oc)
            {
                return monad.Unit;
            }

            var type = obj.GetType();
            if (propSet && prop?.DeclaringType == type)
            {
                return prop == null
                    ? monad.Unit
                    : monad.Lift(prop?.GetValue(oc.Object));
            }

            prop = type.GetProperty(propName);
            propSet = true;
            return prop == null
                ? monad.Unit
                : monad.Lift(prop?.GetValue(oc.Object));
        }
    }
}
