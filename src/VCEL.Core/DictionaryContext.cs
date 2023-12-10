using System.Collections.Generic;
using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL;

public class DictionaryContext<T> : IContext<T>
{
    public DictionaryContext(IMonad<T> monad, IReadOnlyDictionary<string, object> dict)
    {
        this.Dict = dict;
        this.Monad = monad;
    }
    public IMonad<T> Monad { get; }

    public T Value => Monad.Lift(Dict);

    public IReadOnlyDictionary<string, object> Dict { get; }

    public virtual IContext<T> OverrideName(string name, T br)
        => new OverrideContext<T>(
            this,
            ImmutableDictionary<string, T>.Empty.SetItem(name, br));

    public bool TryGetAccessor(string propName, out IValueAccessor<T> accessor)
    {
        accessor = new DictionaryAccessor<T>(propName);
        return true;
    }

    public bool TryGetContext(object o, out IContext<T> context)
    {
        if (o is IReadOnlyDictionary<string, object> dic)
        {
            context = new DictionaryContext<T>(Monad, dic);
            return true;
        }

        context = new ObjectContext<T>(Monad, o);
        return true;
    }
}