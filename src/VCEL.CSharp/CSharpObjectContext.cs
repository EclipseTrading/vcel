using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL.CSharp;

public readonly struct CSharpObjectContext(
    IMonad<string> monad,
    object obj,
    IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc = null
) : IContext<string>
{
    public IMonad<string> Monad { get; } = monad;

    public object Object { get; } = obj;

    public IContext<string> OverrideName(string name, string br)
        => new OverrideContext<string>(
            this,
            ImmutableDictionary<string, string>.Empty.SetItem(name, br));

    public string Value => Monad.Lift(Object);

    public bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
    {
        accessor = new CSharpPropertyValueAccessor(Monad, propName, overridePropertyFunc);
        return true;
    }

    public bool TryGetContext(object o, out IContext<string> context)
    {
        context = new CSharpObjectContext(Monad, o, overridePropertyFunc);
        return true;
    }
}