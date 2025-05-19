using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class Property<TMonad> : IExpression<TMonad>
    {
        private readonly Dictionary<Type, IValueAccessor<TMonad>> accessors = new();
        private Type? lastType;
        private IValueAccessor<TMonad>? lastAccessor;

        public Property(IMonad<TMonad> monad, string propName)
        {
            this.Name = propName;
            this.Monad = monad;
            this.Dependencies = new[] { new PropDependency(propName) };
        }

        public IMonad<TMonad> Monad { get; }

        public string Name { get; }

        public IEnumerable<IDependency> Dependencies { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var cType = context.GetType();
            if(cType == lastType)
            {
                return lastAccessor!.GetValue(context);
            }

            if(!accessors.TryGetValue(cType, out var accessor))
            {
                if(!context.TryGetAccessor(Name, out accessor))
                {
                    accessor = new UnitAccessor<TMonad>(Monad);
                }
                accessors[cType] = accessor;
            }
            lastType = cType;
            lastAccessor = accessor;

            return accessor.GetValue(context);
        }

        public override string ToString() => Name;
    }
}
