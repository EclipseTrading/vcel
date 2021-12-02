using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VCEL.Monad;

namespace VCEL.Core.Expression
{
    internal class ToStringValueExpr<T> : IExpression<string>
    {
        private readonly T value;
        private readonly Func<T, IContext<string>, string> convert;

        public ToStringValueExpr(IMonad<string> monad, T value, Func<T, IContext<string>, string> convert)
        {
            this.value = value;
            this.convert = convert;
            Monad = monad;
        }

        public IMonad<string> Monad { get; }
        public IEnumerable<IDependency> Dependencies => ImmutableArray<IDependency>.Empty;
        
        public string Evaluate(IContext<string> context)
        {
            return convert(value, context);
        }
    }
}