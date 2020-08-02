using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class GuardExpr<TMonad> : IExpression<TMonad>
    {
        private readonly IReadOnlyList<(IExpression<TMonad> Cond, IExpression<TMonad> Res)> clauses;
        private readonly IExpression<TMonad> otherwise;

        public GuardExpr(
            IMonad<TMonad> monad,
            IReadOnlyList<(IExpression<TMonad>, IExpression<TMonad>)> clauses,
            IExpression<TMonad> otherwise)
        {
            Monad = monad;
            this.clauses = clauses;
            this.otherwise = otherwise;
        }

        public IMonad<TMonad> Monad { get; }

        public TMonad Evaluate(IContext<TMonad> context)
        {
            return Next(clauses.GetEnumerator());

            TMonad Eval(IEnumerator<(IExpression<TMonad> Cond, IExpression<TMonad> Res)> ce)
            {
                var result = ce.Current.Cond.Evaluate(context);
                return Monad.Bind(
                    result,
                    BindNext);
                TMonad BindNext(object r)
                    => r is bool b
                        ? (b ? ce.Current.Res.Evaluate(context) : Next(ce))
                        : Monad.Unit;
            }

            TMonad Next(IEnumerator<(IExpression<TMonad>, IExpression<TMonad>)> ce)
                => ce.MoveNext()
                    ? Eval(ce)
                    : otherwise == null
                        ? Monad.Unit
                        : otherwise.Evaluate(context);
        }
    }
}
