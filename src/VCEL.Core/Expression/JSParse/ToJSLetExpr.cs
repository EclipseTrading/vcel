using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.JSParse.Context;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJSLetExpr : IExpression<string>
    {
        private IReadOnlyList<(string, IExpression<string>)> bindings;
        private IExpression<string> expr;

        public ToJSLetExpr(IMonad<string> monad, IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
        {
            this.Monad = monad;
            this.bindings = bindings;
            this.expr = expr;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            var propFuncs = this.bindings
                .Select(b => b.Item1)
                .Distinct()
                .ToDictionary(p => p, p => new Func<string>(() => p));

            var jsContext = new JSObjectContext(Monad, context.Value, propFuncs);

            var letCauses = this.bindings.Select(b => $"let {b.Item1} = {b.Item2.Evaluate(jsContext)}\n");
            var returnCause = this.expr.Evaluate(jsContext);

            var result = "(() => {" + $" {string.Join(" ", letCauses)} return {returnCause}" + "})()";
            return result;
        }
    }
}