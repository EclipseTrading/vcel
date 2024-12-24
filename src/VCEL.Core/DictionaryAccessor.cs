namespace VCEL.Core;

public readonly struct DictionaryAccessor<T> : IValueAccessor<T>
{
    private readonly string propName;

    public DictionaryAccessor(string propName)
    {
        this.propName = propName;
    }

    public T GetValue(IContext<T> context)
    {
        var dictContext = (DictionaryContext<T>)context;
        if (!dictContext.Dict.TryGetValue(propName, out var value))
        {
            return context.Monad.Unit;
        }
        return context.Monad.Lift(value);
    }
}