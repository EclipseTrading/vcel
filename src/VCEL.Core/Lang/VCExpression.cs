﻿using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Lang
{
    public static class VCExpression
    {
        public static ParseResult<object> ParseDefault(string exprString)
        {
            var parser = DefaultParser();
            return parser.Parse(exprString);
        }

        public static ParseResult<Maybe<object>> ParseMaybe(string exprString)
            => MaybeParser().Parse(exprString);

        public static IExpressionParser<object> DefaultParser()
            => new ExpressionParser<object>(
                new ExpressionFactory<object>(
                    ExprMonad.Instance));

        public static IExpressionParser<Maybe<object>> MaybeParser()
            => new ExpressionParser<Maybe<object>>(
                new MaybeExpressionFactory(
                    MaybeMonad.Instance));
    }
}
