using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL.JS;

public class JsObjectContext : IContext<string>
{
    private readonly IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc;
    
    public IMonad<string> Monad { get; }

    public object Object { get; }
    
    public JsObjectContext(IMonad<string> monad, object obj, IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc = null)
    {
        this.overridePropertyFunc = overridePropertyFunc;
        Monad = monad;
        Object = obj;
    }

    public IContext<string> OverrideName(string name, string br)
        => new OverrideContext<string>(
            this,
            ImmutableDictionary<string, string>.Empty.SetItem(name, br));

    public string Value => Monad.Lift(Object);

    public bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
    {
        accessor = new JsPropertyValueAccessor(Monad, propName, overridePropertyFunc);
        return true;
    }

    public bool TryGetContext(object o, out IContext<string> context)
    {
        context = new JsObjectContext(Monad, o, overridePropertyFunc);
        return true;
    }
}