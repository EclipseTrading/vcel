using System.Collections.Generic;
using VCEL.Core.Monad.List;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class ListProperty<TContext> : IExpression<List<object>>
{
    private IValueAccessor<List<object>>? valueAccessor;
    private readonly string propName;

    public ListProperty(string propName)
    {
        this.propName = propName;
        this.Dependencies = new[] { new PropDependency(propName) };
    }

    public List<object> Evaluate(IContext<List<object>> context)
    {
        if(valueAccessor == null)
        {
            if(!context.TryGetAccessor(propName, out valueAccessor))
            {
                valueAccessor = new UnitAccessor<List<object>>(Monad);
            }
        }
        return valueAccessor.GetValue(context);
    }
    public IMonad<List<object>> Monad => ListMonad<object>.Instance;

    public IEnumerable<IDependency> Dependencies { get; }
}