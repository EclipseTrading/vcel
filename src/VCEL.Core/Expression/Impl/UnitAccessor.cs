using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    internal class UnitAccessor<TMonad> : IValueAccessor<TMonad>
    {
        private IMonad<TMonad> monad;

        public UnitAccessor(IMonad<TMonad> monad)
        {
            this.monad = monad;
        }

        public TMonad GetValue(IContext<TMonad> o)
        {
            return monad.Unit;
        }
    }
}
