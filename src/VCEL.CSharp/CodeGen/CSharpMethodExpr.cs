using System;
using System.Collections.Generic;
using System.Reflection;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpMethodExpr : IExpression<object>
    {
        private readonly MethodInfo method;

        public CSharpMethodExpr(IMonad<object> monad, MethodInfo method)
        {
            this.Monad = monad;
            this.method = method;
        }

        public IMonad<object> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public object Evaluate(IContext<object> context)
        {
            // converts context object into dynamic (expando) objects before invoking the mehod
            // this is just for the convenience of unit testing
            // This expression should not be used for performance testing since ToDynamic call is costly
            // and we won't do this in actual production environment.
            return method.Invoke(null, new[] {((ObjectContext<object>)context).Object.ToDynamic()});
        }
    }
}