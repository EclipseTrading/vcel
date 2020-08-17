using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class LetExpr<TMonad> : IExpression<TMonad>
    {
        private readonly HashSet<string> bindingNames;
        private readonly IReadOnlyList<(string, IExpression<TMonad>)> bindings;
        private readonly IExpression<TMonad> expr;

        public LetExpr(
            IMonad<TMonad> monad,
            IReadOnlyList<(string, IExpression<TMonad>)> bindings,
            IExpression<TMonad> expr)
        {
            this.Monad = monad;
            this.bindings = bindings;
            this.expr = expr;
            this.bindingNames = new HashSet<string>(bindings.Select(b => b.Item1));
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies
            => bindings
                .SelectMany(b => b.Item2.Dependencies)
                .Union(expr
                    .Dependencies
                    .Where(d => !(d is PropDependency p && bindingNames.Contains(p.Name))))
                .Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var ctx = context;
            foreach (var (name, exp) in bindings)
            {
                var br = exp.Evaluate(ctx);
                ctx = ctx.OverrideName(name, br);
            }
            return expr.Evaluate(ctx);
        }
    }
}
