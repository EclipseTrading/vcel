using VCEL.Monad;

namespace VCEL.Intrinsics;

public class IntrinsicsMonad<T> : IMonad<ReadOnlyMemory<T>>
    where T : struct
{
    public ReadOnlyMemory<T> Unit => ReadOnlyMemory<T>.Empty;
    public ReadOnlyMemory<T> Lift(object? value)
    {
        return value switch
        {
            ReadOnlyMemory<T> arr => arr,
            _ => throw new ArgumentException()
        };
    }

    public ReadOnlyMemory<T> Bind(ReadOnlyMemory<T> a, Func<object?, ReadOnlyMemory<T>> f)
    {
        return f(a);
    }

    public ReadOnlyMemory<T> Bind(ReadOnlyMemory<T> a, ReadOnlyMemory<T> b, Func<object?, object?, ReadOnlyMemory<T>> f)
    {
        return f(a, b);
    }
    
    
    public static IntrinsicsMonad<T> Instance { get; } = new();
}