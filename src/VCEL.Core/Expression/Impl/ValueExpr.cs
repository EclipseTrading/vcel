﻿using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ValueExpr<TMonad> : IExpression<TMonad>
    {
        private readonly TMonad value;

        public ValueExpr(IMonad<TMonad> monad, object value)
        {
            Monad = monad;
            this.value = Monad.Lift(value);
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => Enumerable.Empty<IDependency>();        

        public TMonad Evaluate(IContext<TMonad> _)
        {
            return value;
        }
    }
}
