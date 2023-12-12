using System.Diagnostics.CodeAnalysis;
using VCEL.Monad;

namespace VCEL.Intrinsics;

public class IntrinsicContext<T>(IReadOnlyList<Dictionary<string, object>> rows) : IContext<ReadOnlyMemory<T>>
    where T : struct
{
    public bool TryGetAccessor(string propName, out IValueAccessor<ReadOnlyMemory<T>> accessor)
    {
        var extra = rows.Count % 8 == 0 ? 0 : 8 - rows.Count % 8; 
        
        var data = new T[rows.Count + extra];
        for (var i = 0; i < rows.Count; i++)
        {
            data[i] = (T)rows[i][propName];
        }
        
        accessor = new IntrinsicsAccessor<T>(data);
        return true;
    }

    public IContext<ReadOnlyMemory<T>> OverrideName(string name, ReadOnlyMemory<T> br)
    {
        throw new NotImplementedException();
    }

    public IMonad<ReadOnlyMemory<T>> Monad => IntrinsicsMonad<T>.Instance;
    public bool TryGetContext(object o, [UnscopedRef] out IContext<ReadOnlyMemory<T>> context)
    {
        context = null!;
        return false;
    }

    public ReadOnlyMemory<T> Value => ReadOnlyMemory<T>.Empty;
}

public class IntrinsicsAccessor<T>(ReadOnlyMemory<T> data) : IValueAccessor<ReadOnlyMemory<T>>
    where T : struct
{
    public ReadOnlyMemory<T> GetValue(IContext<ReadOnlyMemory<T>> _)
    {
        return data;
    }
}