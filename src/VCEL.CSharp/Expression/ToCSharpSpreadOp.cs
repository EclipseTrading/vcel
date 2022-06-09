using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpSpreadOp : IExpression<string>
    {
        private IExpression<string> expr;

        public ToCSharpSpreadOp(IMonad<string> monad, IExpression<string> expr)
        {
            Monad = monad;
            this.expr = expr;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => expr.Dependencies;

        public string Evaluate(IContext<string> context)
        {
            return expr.Evaluate(context);
        }
    }
}