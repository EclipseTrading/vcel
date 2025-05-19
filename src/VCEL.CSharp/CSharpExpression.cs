using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp.CodeGen;
using VCEL.CSharp.Expression.Func;
using VCEL.Expression;

namespace VCEL.CSharp;

public static class CSharpExpression
{
    // For functional testing
    public static ParseResult<string> ParseCode(string exprString, IFunctions<string>? functions = null)
    {
        var parseResult = CSharpParser(functions).Parse(exprString);
        if (!parseResult.Success)
            return new ParseResult<string>(parseResult.ParseErrors);

        var csharpString =  parseResult.Expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
        return new ParseResult<string>(new CSharpStringExpr(null!, csharpString));
    }

    // For functional testing
    public static ParseResult<object?> ParseMethod(string exprString, IFunctions<string>? functions = null)
    {
        var parseResult = CSharpParser(functions).Parse(exprString);
        if (!parseResult.Success)
            return new ParseResult<object?>(parseResult.ParseErrors);

        var expr = parseResult.Expression;
        var csharpExpr = expr.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
        var (type, emitResult) = CodeGenCSharpClass.Generate("VcelTesting", csharpExpr);
        if (type == null)
            return new ParseResult<object?>(emitResult!.Diagnostics.Select(
                x => new ParseError(
                    $"{x.GetMessage(CultureInfo.CurrentCulture)} ({csharpExpr})",
                    x.Id,
                    x.Location.GetLineSpan().StartLinePosition.Line,
                    x.Location.GetLineSpan().StartLinePosition.Line,
                    x.Location.GetLineSpan().EndLinePosition.Line)).ToList());

        var method = type.GetMethod("Evaluate");
        if (method == null)
            throw new Exception("Method not found");
        return new ParseResult<object?>(new CSharpMethodExpr(null!, method));
    }

    // For performance testing
    public static ParseResult<object?> ParseDelegate(string exprString, IFunctions<string>? functions = null)
    {
        var parseResult = CSharpParser(functions).Parse(exprString);
        var expr = parseResult.Expression;

        var csharpExpr = expr.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
        var (type, emitResult) = CodeGenCSharpClass.Generate("VcelTesting", csharpExpr);
        if (type == null)
            return  new ParseResult<object?>(emitResult!.Diagnostics.Select(
                x => new ParseError(
                    x.GetMessage(CultureInfo.CurrentCulture),
                    x.Id,
                    x.Location.GetLineSpan().StartLinePosition.Line,
                    x.Location.GetLineSpan().StartLinePosition.Line,
                    x.Location.GetLineSpan().EndLinePosition.Line)).ToList());

        var method = type.GetMethod("Evaluate");
        Debug.Assert(method != null, nameof(method) + " != null");
        var func = (Func<object?, object?>) Delegate.CreateDelegate(typeof(Func<object?, object?>), null, method);
        return new ParseResult<object?>(new CSharpDelegateExpr(null!, func));
    }

    private static ExpressionParser<string> CSharpParser(IFunctions<string>? functions = null)
        => new(new ToCSharpCodeFactory(ConcatStringMonad.Instance, functions ?? new DefaultCSharpFunctions()));
}