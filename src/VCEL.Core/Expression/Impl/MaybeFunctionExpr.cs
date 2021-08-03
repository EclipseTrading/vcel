using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Expression.Impl
{
    public class MaybeFunctionExpr : FunctionExpr<Maybe<object>>
    {
        public MaybeFunctionExpr(
            IMonad<Maybe<object>> monad,
            string name,
            IReadOnlyList<IExpression<Maybe<object>>> args,
            Function<Maybe<object>> function)
            : base(monad, name, args, function)
        {
        }

        public override Maybe<object> Evaluate(IContext<Maybe<object>> context)
        {
            var parsedArgs = Args
                .Select(arg => arg.Evaluate(context).Value);

            return Monad.Lift(function.Func(parsedArgs.ToArray(), context));
        }
    }
}
