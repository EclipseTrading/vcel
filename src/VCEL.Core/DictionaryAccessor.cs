namespace VCEL.Core
{
    public class DictionaryAccessor<T> : IValueAccessor<T>
    {
        private string propName;

        public DictionaryAccessor(string propName)
        {
            this.propName = propName;
        }

        public T GetValue(IContext<T> o)
        {
            var dictContext = (DictionaryContext<T>)o;
            return o.Monad.Lift(dictContext.Dict[propName]);
        }
    }
}
