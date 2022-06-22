using System.Diagnostics.CodeAnalysis;
using VCEL.Monad;

namespace VCEL
{
    public interface IContext<T>
    {
        bool TryGetAccessor(string propName, [NotNullWhen(true)] out IValueAccessor<T>? accessor);
        IContext<T> OverrideName(string name, T br);
        IMonad<T> Monad { get; }
        bool TryGetContext(object o, [NotNullWhen(true)] out IContext<T>? context);
        T Value { get; }
    }
}