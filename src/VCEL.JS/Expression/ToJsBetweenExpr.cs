using System.Collections.Generic;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsBetweenExpr : IExpression<string>
    {
        private readonly IExpression<string> leftExpr;
        private readonly IExpression<string> lower;
        private readonly IExpression<string> upper;

        public ToJsBetweenExpr(
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> lower,
            IExpression<string> upper)
        {
            Monad = monad;
            leftExpr = left;
            this.lower = lower;
            this.upper = upper;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();


        public string Evaluate(IContext<string> context)
        {
            var l = leftExpr.Evaluate(context);

            var rStart = lower.Evaluate(context);
            var rEnd = upper.Evaluate(context);

            return $"{l} >= {rStart} && {l} <= {rEnd}";
        }
    }
}
