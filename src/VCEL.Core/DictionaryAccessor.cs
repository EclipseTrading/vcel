using VCEL.Core;

namespace VCEL;

public class DictionaryAccessor<T> : IValueAccessor<T>
{
    private readonly string propName;

    public DictionaryAccessor(string propName)
    {
        this.propName = propName;
    }

    public T GetValue(IContext<T> o)
    {
        var dictContext = (DictionaryContext<T>)o;
        return !dictContext.Dict.TryGetValue(propName, out var value) 
            ? dictContext.Monad.Unit! 
            : dictContext.Monad.Lift(value);
    }
}