using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpCompareOp(
    string opName,
    IMonad<string> monad,
    IExpression<string> left,
    IExpression<string> right)
    : BinaryExprBase<string>(monad, left, right)
{
    public override string Evaluate(object? lv, object? rv) => opName switch
    {
        ">" => $"{nameof(CSharpHelper)}.{nameof(CSharpHelper.UpCastCompareGreaterThan)}({lv}, {rv})",
        ">=" => $"{nameof(CSharpHelper)}.{nameof(CSharpHelper.UpCastCompareGreaterThanOrEqual)}({lv}, {rv})",
        "<" => $"{nameof(CSharpHelper)}.{nameof(CSharpHelper.UpCastCompareLessThan)}({lv}, {rv})",
        "<=" => $"{nameof(CSharpHelper)}.{nameof(CSharpHelper.UpCastCompareLessThanOrEqual)}({lv}, {rv})",
        _ => "false",
    };
}