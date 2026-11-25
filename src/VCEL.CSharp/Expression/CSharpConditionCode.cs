using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal static class CSharpConditionCode
{
    internal static string Evaluate(IExpression<string> expression, IContext<string> context)
    {
        // Only conditions discard the original result type; values retain C# binding semantics.
        return expression switch
        {
            ToCSharpEqOp equal => equal.EvaluateCondition(context),
            ToCSharpNotEqOp notEqual => notEqual.EvaluateCondition(context),
            ToCSharpCompareOp comparison => comparison.EvaluateCondition(context),
            ToCSharpInStringSetOp membership => membership.EvaluateCondition(context),
            ParenExpr<string> paren => Evaluate(paren.Expr, context),
            ToCSharpNotOp not => $"!{Evaluate(not.Operand, context)}",
            _ => expression.Evaluate(context)
        };
    }
}
