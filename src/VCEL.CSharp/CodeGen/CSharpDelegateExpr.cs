using System;
using System.Collections.Generic;
using System.Reflection;
using VCEL.Core.Helper;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpDelegateExpr : IExpression<object>
    {
        private readonly Func<object, object> csharpFunc;

        public CSharpDelegateExpr(IMonad<object> monad, IExpression<string> expression)
        {
            this.Monad = monad;
            var csharpExpr = expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
            var type = CodeGenCSharpClass.Generate("VcelTesting", csharpExpr);
            var csharpMethod = type.GetMethod("Evaluate");
            this.csharpFunc = (Func<object, object>) Delegate.CreateDelegate(typeof(Func<object, object>), null, csharpMethod);
        }

        public IMonad<object> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public object Evaluate(IContext<object> context)
        {
            return csharpFunc(((ObjectContext<object>)context).Object);
        }
    }
}