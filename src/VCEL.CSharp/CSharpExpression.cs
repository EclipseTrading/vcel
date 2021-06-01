using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;
using VCEL.CSharp.CodeGen;
using VCEL.CSharp.Expression.Func;
using VCEL.Expression;

namespace VCEL.CSharp
{
    public static class CSharpExpression
    {
        // For functional testing
        public static ParseResult<object> ParseMethod(string exprString, IFunctions<string> functions = null)
        {
            var parseResult = CSharpParser(functions).Parse(exprString);

            return parseResult.Success
                ? new ParseResult<object>(new CSharpMethodExpr(null, parseResult.Expression))
                : new ParseResult<object>(parseResult.ParseErrors);
        }

        // For performance testing
        public static ParseResult<object> ParseDelegate(string exprString, IFunctions<string> functions = null)
        {
            var parseResult = CSharpParser(functions).Parse(exprString);

            return parseResult.Success
                ? new ParseResult<object>(new CSharpDelegateExpr(null, parseResult.Expression))
                : new ParseResult<object>(parseResult.ParseErrors);
        }

        private static IExpressionParser<string> CSharpParser(IFunctions<string> functions = null)
            => new ExpressionParser<string>(
                new ToCSharpCodeFactory(ConcatStringMonad.Instance, functions ?? new DefaultCSharpFunctions()));
    }
}
