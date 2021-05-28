using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpPropertyOp : IExpression<string>
    {
        private readonly string name;

        public ToCSharpPropertyOp(string name, IMonad<string> monad)
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