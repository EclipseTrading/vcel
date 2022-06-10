using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.CodeGen
{
    public class CSharpStringExpr : IExpression<string>
    {
        private readonly string str;

        public CSharpStringExpr(IMonad<string> monad, string str)
        {
            this.Monad = monad;
            this.str = str;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            return str;
        }
    }
}