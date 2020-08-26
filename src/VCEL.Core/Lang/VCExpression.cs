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
            => ParseDefault(exprString, CreateFunctions(funcs));

        public static ParseResult<object> ParseDefault(string exprString, IFunctions functions = null)
            => DefaultParser(functions).Parse(exprString);

        public static ParseResult<Maybe<object>> ParseMaybe(string exprString, params (string, Func<object[], object>)[] funcs)
            => ParseMaybe(exprString, CreateFunctions(funcs));

        public static ParseResult<Maybe<object>> ParseMaybe(string exprString, IFunctions functions = null)
            => MaybeParser(functions).Parse(exprString);

        public static IExpressionParser<object> DefaultParser(IFunctions functions = null)
            => new ExpressionParser<object>(
                new ExpressionFactory<object>(
                    ExprMonad.Instance,
                    functions));

        public static IExpressionParser<Maybe<object>> MaybeParser(IFunctions functions = null)
            => new ExpressionParser<Maybe<object>>(
                new MaybeExpressionFactory(
                    MaybeMonad.Instance,
                    functions));

        private static IFunctions CreateFunctions((string, Func<object[], object>)[] funcs)
        {
            var functions = new DefaultFunctions();
            foreach (var (name, f) in funcs)
            {
                functions.Register(name, f);
            }
            return functions;
        }
    }
}
