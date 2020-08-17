using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class Ternary<TMonad> : IExpression<TMonad>
    {
        public Ternary(
            IMonad<TMonad> factory,
            IExpression<TMonad> conditionExpr,
            IExpression<TMonad> trueExpr,
            IExpression<TMonad> falseExpr)
        {
            Monad = factory;
            ConditionExpr = conditionExpr;
            TrueExpr = trueExpr;
            FalseExpr = falseExpr;
        }

        public IExpression<TMonad> ConditionExpr { get; }
        public IExpression<TMonad> TrueExpr { get; }
        public IExpression<TMonad> FalseExpr { get; }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies
            => ConditionExpr
                .Dependencies
                .Union(TrueExpr.Dependencies)
                .Union(FalseExpr.Dependencies)
                .Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var c = ConditionExpr.Evaluate(context);
            return Monad.Bind(c, BindC);

            TMonad BindC(object res)
            {
                return res is bool b
                    ? (b ? TrueExpr.Evaluate(context) : FalseExpr.Evaluate(context))
                    : Monad.Unit;
            }
        }
    }
}
