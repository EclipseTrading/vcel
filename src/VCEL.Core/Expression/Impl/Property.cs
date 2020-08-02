using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class Property<TMonad> : IExpression<TMonad>
    {
        private readonly string propName;
        private IValueAccessor<TMonad> accessor;

        public Property(IMonad<TMonad> monad, string propName)
        {
            this.propName = propName;
            Monad = monad;

        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            if(accessor == null)
            {
                if(!context.TryGetAccessor(propName, out accessor))
                {
                    accessor = new UnitAccessor<TMonad>(Monad);
                }
            }

            return accessor.GetValue(context);
        }

        public override string ToString() => propName;
    }
}
