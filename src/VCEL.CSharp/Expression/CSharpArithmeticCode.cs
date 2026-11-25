using System.Globalization;
using VCEL.Core.Expression.Impl;

namespace VCEL.CSharp.Expression;

internal static class CSharpArithmeticCode
{
    public static string Evaluate(IExpression<string> expression, IContext<string> context)
    {
        var result = Build(expression, context);
        return result.HasUnboxedValue ? $"((dynamic)({result.Value}).ToObject())" : result.Original;
    }

    private static (string Original, string Value, bool HasUnboxedValue, bool IsNumericLiteral) Build(
        IExpression<string> expression, IContext<string> context)
    {
        if (expression is ParenExpr<string> paren)
        {
            return Build(paren.Expr, context);
        }

        if (expression is ToCSharpPropertyOp property && property.TryGetRawValue(context, out var raw))
        {
            return (property.Evaluate(context),
                $"(CSharpHelper.IsNumber((object)({raw})) ? CSharpArithmeticValue.Literal((double)({raw})) : CSharpArithmeticValue.Result((object)({raw})))",
                true, false);
        }

        if (expression is ToCSharpUnaryMinus unary)
        {
            var operand = Build(unary.Operand, context);
            var original = $"-{operand.Original}";
            return (original, operand.HasUnboxedValue
                ? $"CSharpArithmeticValue.Negate({operand.Value})"
                : operand.IsNumericLiteral
                    ? $"CSharpArithmeticValue.Literal({original})"
                    : $"CSharpArithmeticValue.Result((object)({original}))",
                operand.HasUnboxedValue, operand.IsNumericLiteral);
        }

        if (expression is BinaryExprBase<string> binary &&
            (expression is ToCSharpBinaryOp || expression is ToCSharpDivideOp))
        {
            var left = Build(binary.Left, context);
            var right = Build(binary.Right, context);
            var original = binary.Evaluate(left.Original, right.Original);
            if ((left.HasUnboxedValue || right.HasUnboxedValue) &&
                (left.HasUnboxedValue || left.IsNumericLiteral) && (right.HasUnboxedValue || right.IsNumericLiteral))
            {
                var operation = expression is ToCSharpBinaryOp op ? op.Operation : "Divide";
                var leftName = context is CSharpObjectContext leftContext
                    ? leftContext.GetFallbackParameterName("__vcelLeft") : "__vcelLeft";
                var rightName = context is CSharpObjectContext rightContext
                    ? rightContext.GetFallbackParameterName("__vcelRight") : "__vcelRight";
                var fallbackLeft = left.IsNumericLiteral ? left.Original : $"(dynamic){leftName}";
                var fallbackRight = right.IsNumericLiteral ? right.Original : $"(dynamic){rightName}";
                var fallback = binary.Evaluate(fallbackLeft, fallbackRight);
                var leftValue = expression is ToCSharpDivideOp
                    ? $"CSharpArithmeticValue.DivisionOperand({left.Value})"
                    : left.Value;
                return (original,
                    $"CSharpArithmeticValue.{operation}({leftValue}, {right.Value}, static ({leftName}, {rightName}) => (object)({fallback}))",
                    true, false);
            }

            var numeric = left.IsNumericLiteral && right.IsNumericLiteral;
            return (original, numeric
                ? $"CSharpArithmeticValue.Literal({original})"
                : $"CSharpArithmeticValue.Result((object)({original}))", false, numeric);
        }

        var code = expression.Evaluate(context);
        var isLiteral = (expression is ToCSharpStringOp or IntExpr<string> or LongExpr<string>) &&
                        double.TryParse(code.TrimEnd('f', 'F', 'l', 'L'),
                            NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        return (code,
            isLiteral ? $"CSharpArithmeticValue.Literal({code})" : $"CSharpArithmeticValue.Result((object)({code}))",
            false, isLiteral);
    }
}
