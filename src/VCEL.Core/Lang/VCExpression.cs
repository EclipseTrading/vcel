using System;
using VCEL.Core.Expression.Func;
using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Core.Lang
{
    public static class VCExpression
    {
        public static ParseResult<object> ParseDefault(string exprString, params (string, Func<object[], object>)[] funcs)
            => ParseDefault(exprString, CreateFunctions<object>(funcs));

        public static ParseResult<object> ParseDefault(string exprString, IFunctions<object>? functions = null)
            => DefaultParser(functions).Parse(exprString);

        public static ParseResult<Maybe<object>> ParseMaybe(string exprString, params (string, Func<object[], object>)[] funcs)
            => ParseMaybe(exprString, CreateFunctions<Maybe<object>>(funcs));

        public static ParseResult<Maybe<object>> ParseMaybe(string exprString, IFunctions<Maybe<object>>? functions = null)
            => MaybeParser(functions).Parse(exprString);

        public static IExpressionParser<object> DefaultParser(IFunctions<object>? functions = null)
            => new ExpressionParser<object>(
                new ExpressionFactory<object>(
                    ExprMonad.Instance,
                    functions));

        public static IExpressionParser<Maybe<object>> MaybeParser(IFunctions<Maybe<object>>? functions = null)
            => new ExpressionParser<Maybe<object>>(
                new MaybeExpressionFactory(
                    MaybeMonad.Instance,
                    functions));

        private static IFunctions<T> CreateFunctions<T>((string, Func<object[], object>)[] funcs)
        {
            var functions = new DefaultFunctions<T>();
            foreach (var (name, f) in funcs)
            {
                functions.Register(name, f);
            }
            return functions;
        }
    }
}
