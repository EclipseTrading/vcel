using System.Reflection;
using VCEL.Monad;

namespace VCEL
{
    public class PropertyValueAccessor<TMonad> : IValueAccessor<TMonad>
    {
        private readonly PropertyInfo prop;
        private readonly IMonad<TMonad> monad;

        public PropertyValueAccessor(IMonad<TMonad> monad, PropertyInfo prop)
        {
            this.prop = prop;
            this.monad = monad;
        }

        public TMonad GetValue(IContext<TMonad> context)
        {
            var oc = context as ObjectContext<TMonad>;
            var value = prop.GetValue(oc.Object);
            return monad.Lift(value);
        }

    }
}
