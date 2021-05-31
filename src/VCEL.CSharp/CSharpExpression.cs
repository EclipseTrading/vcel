using System;
using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;
using VCEL.CSharp.CodeGen;
using VCEL.CSharp.Expression.Func;
using VCEL.Expression;

namespace VCEL.CSharp
{
    public static class CSharpExpression
    {
        public static ParseResult<string> ParseCode(string exprString, params (string, Func<object[], string>)[] funcs)
            => ParseCode(exprString, CreateFunctions(funcs));

        public static ParseResult<string> ParseCode(string exprString, IFunctions<string> functions = null)
            => CSharpParser(functions).Parse(exprString);

        public static ParseResult<object> ParseNativeDynamic(string exprString, params (string, Func<object[], string>)[] funcs)
            => ParseNativeDynamic(exprString, CreateFunctions(funcs));

        public static ParseResult<object> ParseNativeDynamic(string exprString, IFunctions<string> functions = null)
        {
            var parseResult = CSharpParser(functions).Parse(exprString);

            return parseResult.Success
                ? new ParseResult<object>(new CSharpDynamicMethodExpr(null, parseResult.Expression))
                : new ParseResult<object>(parseResult.ParseErrors);
        }

        public static ParseResult<object> ParseNative(string exprString, IFunctions<string> functions = null)
        {
            var parseResult = CSharpParser(functions).Parse(exprString);

            return parseResult.Success
                ? new ParseResult<object>(new CSharpMethodExpr(null, parseResult.Expression))
                : new ParseResult<object>(parseResult.ParseErrors);
        }

        public static IExpressionParser<string> CSharpParser(IFunctions<string> functions = null)
            => new ExpressionParser<string>(
                new ToCSharpCodeFactory(ConcatStringMonad.Instance, functions ?? new DefaultCSharpFunctions()));

        private static IFunctions<string> CreateFunctions((string, Func<object[], string>)[] funcs)
        {
            var functions = new DefaultCSharpFunctions();
            foreach (var (name, f) in funcs)
            {
                functions.Register(name, f);
            }
            return functions;
        }
    }
}
