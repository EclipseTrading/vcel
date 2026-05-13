using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class Variable<TMonad> : IExpression<TMonad>
{
    private readonly Dictionary<Type, IValueAccessor<TMonad>> accessors = new();
    private Type? lastType;
    private IValueAccessor<TMonad>? lastAccessor;

    public Variable(IMonad<TMonad> monad, string variableName)
    {
        this.VariableName = variableName.TrimStart('#');
        Monad = monad;
        this.Dependencies = [];
    }

    public IMonad<TMonad> Monad { get; }

    public string VariableName { get; }

    public IEnumerable<IDependency> Dependencies { get; }

    public TMonad Evaluate(IContext<TMonad> context)
    {
        var cType = context.GetType();
        if (cType == lastType)
        {
            return lastAccessor!.GetValue(context);
        }

        if (!accessors.TryGetValue(cType, out var accessor))
        {
            if (!context.TryGetAccessor(VariableName, out accessor))
            {
                accessor = new UnitAccessor<TMonad>(Monad);
            }
            accessors[cType] = accessor;
        }
        lastType = cType;
        lastAccessor = accessor;

        return accessor.GetValue(context);
    }

    public override string ToString() => $"#{VariableName}";
}