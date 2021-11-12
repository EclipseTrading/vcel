using VCEL.Monad;

namespace VCEL.Expression
{
    using System;

    public class ConcatStringMonad : IMonad<string?>
    {
        public string Unit => "";
        public string? Bind(string? m, Func<object?, string?> f) => f(m);
        public string? Lift(object? value) => value?.ToString();

        public static ConcatStringMonad Instance { get; } = new ConcatStringMonad();

        public string? Bind(string? a, string? b, Func<object?, object?, string?> f)
        {
            return f(a, b);
        }
    }
}
