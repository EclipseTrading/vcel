﻿using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL;

public readonly struct ObjectContext<TMonad> : IContext<TMonad>
{
    public IMonad<TMonad> Monad { get; }

    public object Object { get; }

    public ObjectContext(IMonad<TMonad> monad, object obj)
    {
        Monad = monad;
        Object = obj;
    }

    public IContext<TMonad> OverrideName(string name, TMonad br)
        => new OverrideContext<TMonad>(
            this,
            ImmutableDictionary<string, TMonad>.Empty.SetItem(name, br));

    public bool TryGetAccessor(string propName, out IValueAccessor<TMonad> accessor)
    {
        accessor = new PropertyValueAccessor<TMonad>(Monad, propName);
        return true;
    }
    public bool TryGetContext(object o, out IContext<TMonad> context)
    {
        context = new ObjectContext<TMonad>(Monad, o);
        return true;
    }
    public TMonad Value => Monad.Lift(Object);
}