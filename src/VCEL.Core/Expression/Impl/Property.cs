using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class Property<TMonad> : IExpression<TMonad>
    {
        private readonly string propName;
        private readonly IDictionary<Type, IValueAccessor<TMonad>> accessors = new Dictionary<Type, IValueAccessor<TMonad>>();
        private Type lastType;
        private IValueAccessor<TMonad> lastAccessor;

        public Property(IMonad<TMonad> monad, string propName)
        {
            this.propName = propName;
            Monad = monad;

        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var cType = context.GetType();
            if(cType == lastType)
            {
                return lastAccessor.GetValue(context);
            }

            if(!accessors.TryGetValue(cType, out var accessor))
            {
                if(!context.TryGetAccessor(propName, out accessor))
                {
                    accessor = new UnitAccessor<TMonad>(Monad);
                }
                accessors[cType] = accessor;
            }
            lastType = cType;
            lastAccessor = accessor;

            return accessor.GetValue(context);
        }

        public override string ToString() => propName;
    }
}
