using System;

namespace VCEL.Monad;

public class ExprMonad<T> : IMonad<T?>
{
    public T? Unit => default;
    public T? Lift(object? value)
    {
        return value is T t ? t : default;
    }

    public T? Bind(T? m, Func<object?, T?> f)
    {
        return m == null ? default : f(m);
    }

    public T? Bind(T? a, T? b, Func<object?, object?, T?> f)
    {
        return a == null || b == null ? default : f(a, b);
    }

    public static ExprMonad<float> FloatMonad { get; } = new();
}

public class ExprMonad : IMonad<object?>
{
    public object? Unit { get; } = null;
    public object? Lift(object? value) 
    {
        return value; 
    }
    public object? Bind(object? m, Func<object?, object?> f)
    {
        return f(m);
    }

    public object? Bind(object? a, object? b, Func<object?, object?, object?> f)
    {
        return f(a, b);
    }

    public static ExprMonad Instance { get; } = new ExprMonad();
}