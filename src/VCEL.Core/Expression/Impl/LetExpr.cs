using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class LetExpr<TMonad> : IExpression<TMonad>
    {
        private readonly IReadOnlyList<(string, IExpression<TMonad>)> bindings;
        private readonly IExpression<TMonad> expr;

        public LetExpr(
            IMonad<TMonad> monad,
            IReadOnlyList<(string, IExpression<TMonad>)> bindings, 
            IExpression<TMonad> expr)
        {
            Monad = monad;
            this.bindings = bindings;
            this.expr = expr;
        }

        public IMonad<TMonad> Monad { get; }

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
