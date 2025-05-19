using VCEL.Monad;

namespace VCEL.Expression;

using System;

public sealed class ConcatStringMonad : IMonad<string>
{
    public string Unit => "";
    public string Bind(string m, Func<object, string> f) => f(m);
    public string Lift<TValue>(TValue value) => value?.ToString() ?? string.Empty;

    private ConcatStringMonad()
    {
    }

    public static ConcatStringMonad Instance { get; } = new();

    public string Bind(string m, string b, Func<object, object, string> f)
    {
        return f(m, b);
    }

    public string Bind(string m, IContext<string> context, Func<object?, IContext<string>, string> f)
    {
        return f(m, context);
    }

    public string Bind<TValue>(string m, IContext<string> context, Func<object?, IContext<string>, TValue, string> f,
        TValue value)
    {
        return f(m, context, value);
    }
}

public sealed class ConcatCSharpMonad : IMonad<string>
{
    public string Unit => "";
    public string Bind(string m, Func<object, string> f) => f(m);

    public string Lift<TValue>(TValue value) => value switch
    {
        null => string.Empty,
        string str => str,
        int => $"{value}d",
        long => $"{value}d",
        double => $"{value}d",
        float => $"{value}d",
        decimal => $"{value}d",
        // ValueType => $"({value.GetType().Name}){value}",
        _ => value.ToString() ?? string.Empty,
    };
    
    private ConcatCSharpMonad()
    {
    }

    public static ConcatCSharpMonad Instance { get; } = new();

    public string Bind(string a, string b, Func<object, object, string> f)
    {
        return f(a, b);
    }

    public string Bind(string m, IContext<string> context, Func<object?, IContext<string>, string> f)
    {
        return f(m, context);
    }

    public string Bind<TValue>(string m, IContext<string> context, Func<object?, IContext<string>, TValue, string> f,
        TValue value)
    {
        return f(m, context, value);
    }
}