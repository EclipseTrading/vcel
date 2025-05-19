using System.Collections.Generic;
using VCEL.Core.Lang;
using VCEL.CSharp;

namespace VCEL.Test.Shared;

public static class CompositeExpression
{
    public static IEnumerable<ParseResult<object?>> ParseMultiple(string exprString)
    {
        // yield return VCExpression.ParseDefault(exprString);
        yield return CSharpExpression.ParseMethod(exprString);
    }
}