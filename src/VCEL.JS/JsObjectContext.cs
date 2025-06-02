using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VCEL.Monad;

namespace VCEL.JS;

public sealed class JsObjectContext(
    IMonad<string> monad,
    object obj,
    IReadOnlyDictionary<string, Func<string>>? propertyFunc = null
) : IContext<string>
{
    public IMonad<string> Monad { get; } = monad;

    public object Object { get; } = obj;

    public JsObjectContext WithOverrides(IReadOnlyDictionary<string, Func<string>> overridePropertyFunc)
    {
        if (propertyFunc is null)
        {
            return new JsObjectContext(Monad, Object, overridePropertyFunc);
        }

        // Combine the existing overrides with the new ones
        var combinedOverrides = propertyFunc.ToDictionary();
        foreach (var kvp in overridePropertyFunc)
        {
            combinedOverrides[kvp.Key] = kvp.Value;
        }

        return new JsObjectContext(Monad, Object, combinedOverrides);
    }

    public IContext<string> OverrideName(string name, string br)
        => new OverrideContext<string>(
            this,
            ImmutableDictionary<string, string>.Empty.SetItem(name, br));

    public string Value => Monad.Lift(Object);

    public bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
    {
        accessor = new JsPropertyValueAccessor(Monad, propName, propertyFunc);
        return true;
    }

    public bool TryGetContext(object o, out IContext<string> context)
    {
        context = new JsObjectContext(Monad, o, propertyFunc);
        return true;
    }
}