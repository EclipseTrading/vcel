using System.Collections.Generic;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsBetweenExpr : IExpression<string>
    {
        private readonly IExpression<string> leftExpr;
        private readonly ListExpr<string>? rightExpr;

        public ToJsBetweenExpr(
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
        {
            Monad = monad;
            leftExpr = left;
            rightExpr = right as ListExpr<string>;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();


        public string Evaluate(IContext<string> context)
        {
            var l = leftExpr.Evaluate(context);
            if(rightExpr == null) 
            {
                // Preserve the expression form to help 
                // with debugging
                return $"{l} >= undefined && {l} <= undefined";
            }

            var rStart = rightExpr.List[0].Evaluate(context);
            var rEnd = rightExpr.List[1].Evaluate(context);

            return $"{l} >= {rStart} && {l} <= {rEnd}";
        }
    }
}
