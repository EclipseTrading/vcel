using System;
using System.Collections.Generic;
using System.Reflection;
using VCEL.Core.Helper;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpDynamicMethodExpr : IExpression<object>
    {
        private readonly MethodInfo csharpMethod;

        public CSharpDynamicMethodExpr(IMonad<object> monad, IExpression<string> expression)
        {
            this.Monad = monad;
            var csharpExpr = expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
            var type = CodeGenCSharpClass.Generate("VcelTesting", csharpExpr);
            this.csharpMethod = type.GetMethod("Evaluate");
        }

        public IMonad<object> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public object Evaluate(IContext<object> context)
        {
            // converts context object into dynamic (expando) objects before invoking the mehod
            // this is just for the convenience of unit testing
            // This expression should not be used for performance testing since ToDynamic call is costly
            // and we won't do this in actual production environment.
            return csharpMethod.Invoke(null, new[] {((ObjectContext<object>)context).Object.ToDynamic()});
        }
    }
}