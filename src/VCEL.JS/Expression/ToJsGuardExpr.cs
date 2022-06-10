using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsGuardExpr : IExpression<string>
    {
        private readonly IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses;
        private readonly IExpression<string>? otherwise;

        public ToJsGuardExpr(
            IMonad<string> monad, 
            IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses, 
            IExpression<string>? otherwise)
        {
            this.Monad = monad;
            this.guardClauses = guardClauses;
            this.otherwise = otherwise;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            var guardCases = guardClauses.Select(gc => $"case {gc.Item1.Evaluate(context)}: return {gc.Item2.Evaluate(context)};").ToList();
            var otherCase = $"default: return {otherwise?.Evaluate(context) ?? "undefined"}";
            guardCases.Add(otherCase);

            var result = "(() => { switch(true) {" + $"{string.Join(" ", guardCases.ToArray())}" + "}})()";
            return result;
        }
    }
}