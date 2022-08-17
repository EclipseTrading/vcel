using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsModOp : IExpression<string>
    {
        private IExpression<string> l;
        private IExpression<string> r;

        public ToJsModOp(IMonad<string> monad, IExpression<string> l, IExpression<string> r)
        {
            this.Monad = monad;
            this.l = l;
            this.r = r;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            return $"({l.Evaluate(context)} % {r.Evaluate(context)})";
        }
    }
}