using System;
using System.Collections.Generic;
using System.Threading;
using VCEL.Monad;

namespace VCEL.CSharp;

public readonly struct CSharpPropertyValueAccessor(
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

        var finalPropOrMethod = $"{context.Value}.{propName}";
        // var finalPropOrMethod = $"({context.Value}[\"{propName}\"])";
        var outVariableName = $"__outVar{Interlocked.Increment(ref atomic)}";
        var varVariableName = $"__var{atomic}";
        return monad.Lift(
            $"({nameof(CSharpHelper)}.{nameof(CSharpHelper.TryToDouble)}({finalPropOrMethod} is var {varVariableName} ? {varVariableName} : {varVariableName}, out double {outVariableName})" +
            $" ? {outVariableName}" +
            $" : {varVariableName})");
    }
}