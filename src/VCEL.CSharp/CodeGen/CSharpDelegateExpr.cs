using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpDelegateExpr : IExpression<object>
    {
        private readonly Func<object, object> func;

        public CSharpDelegateExpr(IMonad<object> monad, Func<object, object> func)
        {
            this.Monad = monad;
            this.func = func;
        }

        public IMonad<object> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public object Evaluate(IContext<object> context)
        {
            return func(((ObjectContext<object>)context).Object);
        }
    }
}