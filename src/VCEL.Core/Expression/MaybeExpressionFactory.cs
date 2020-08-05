﻿using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Expression
{
    public class MaybeExpressionFactory : ExpressionFactory<Maybe<object>>
    {
        public MaybeExpressionFactory(
            IMonad<Maybe<object>> monad,
            IFunctions functions = null)
            : base(monad, functions ?? new DefaultFunctions())
        {
        }

        public override IExpression<Maybe<object>> Divide(IExpression<Maybe<object>> l, IExpression<Maybe<object>> r)
            => new MaybeDivide(Monad, l, r);
    }
}
