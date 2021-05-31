using System.Collections.Generic;
using VCEL.Core.Lang;
using VCEL.CSharp;

namespace VCEL.Test.Shared
{
    public static class CompositeExpression
    {
        public static List<ParseResult<object>> ParseMultiple(string exprString)
        {
            return new List<ParseResult<object>>
            {
                VCExpression.ParseDefault(exprString),
                CSharpExpression.ParseNativeDynamic(exprString)
            };
        }
    }
}
