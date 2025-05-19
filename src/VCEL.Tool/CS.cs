using System.Collections.Immutable;
using VCEL.CSharp;
using VCEL.Monad;

namespace VCEL.Tool;

public sealed class CSharpKeyAccessContext(
    IMonad<string> monad,
    object obj,
    IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc = null) : IContext<string>
{
    public bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
    {
        accessor = new CSharpKeyValueAccessor(Monad, propName, overridePropertyFunc);
        return true;
    }

    public IContext<string> OverrideName(string name, string br)
    {
        return new OverrideContext<string>(
            this,
            ImmutableDictionary<string, string>.Empty.SetItem(name, br));
    }

    public bool TryGetContext(object o, out IContext<string> context)
    {
        context = new CSharpKeyAccessContext(Monad, o, overridePropertyFunc);
        return true;
    }

    public IMonad<string> Monad { get; } = monad;
    public string Value { get; } = monad.Lift(obj);
}

public sealed class CSharpKeyValueAccessor(
    IMonad<string> monad,
    string propName,
    IReadOnlyDictionary<string, Func<string>>? overridePropFunc = null)
    : IValueAccessor<string>
{
    private static volatile int atomic;

    public string GetValue(IContext<string> context)
    {
        if (overridePropFunc != null && overridePropFunc.TryGetValue(propName, out var func))
        {
            return monad.Lift(func());
        }

        var finalPropOrMethod = $"({context.Value}[\"{propName}\"])";
        var outVariableName = $"outVar{Interlocked.Increment(ref atomic)}";
        return monad.Lift(
            $"({nameof(CSharpHelper)}.{nameof(CSharpHelper.TryToDouble)}({finalPropOrMethod}, out double {outVariableName})" +
            $" ? {outVariableName}" +
            $" : {finalPropOrMethod})");
    }
}