namespace VCEL.Core;

public readonly struct DictionaryAccessor<T> : IValueAccessor<T>
{
    private readonly string propName;

    public DictionaryAccessor(string propName)
    {
        this.propName = propName;
    }

    public T GetValue(IContext<T> o)
    {
        var dictContext = (DictionaryContext<T>)o;
        if (!dictContext.Dict.TryGetValue(propName, out var value))
        {
            return o.Monad.Unit;
        }
        return o.Monad.Lift(value);
    }
}