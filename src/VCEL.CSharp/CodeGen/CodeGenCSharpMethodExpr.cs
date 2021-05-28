using System;
using System.Collections.Generic;
using System.Reflection;
using VCEL.Core.Helper;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpMethodExpr : IExpression<object>
    {
        private readonly MethodInfo csharpMethod;

        public CSharpMethodExpr(IMonad<object> monad, IExpression<string> expression)
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
            return csharpMethod.Invoke(null, new[] {((ObjectContext<object>)context).Object.ToDynamic()});
        }
    }
}