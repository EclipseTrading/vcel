using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJsPropertyOp : IExpression<string>
    {
        private string name;

        public ToJsPropertyOp(string name, IMonad<string> monad)
        {
            this.name = name;
            this.Monad = monad;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            if (context.TryGetAccessor(name, out var accessor))
            {
                var result = accessor.GetValue(context);
                return result;
            }

            return name;
        }
    }
}