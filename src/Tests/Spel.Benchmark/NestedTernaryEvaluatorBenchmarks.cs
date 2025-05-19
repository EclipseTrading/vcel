using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace Spel.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class NestedTernaryEvaluatorBenchmarks
{
    private static readonly IExpression SpelExpr = Expression.Parse(Expressions.NestedTernary1);
    private static readonly IExpression<Maybe<object>> VcelExpr = VCExpression.ParseMaybe(Expressions.NestedTernary1).Expression;
    private static readonly IExpression<Maybe<object>> VcelExprLetGuard = VCExpression.ParseMaybe(Expressions.LetGuard).Expression;
    private static readonly IExpression<object?> CSharpExprLetGuard = CSharpExpression.ParseDelegate(Expressions.LetGuard).Expression;
    private static readonly CustomExpr CustomExpr = new CustomExpr();

    // 1 Branch
    private static readonly TestRow Row1 = Row(1, false);
    private static readonly TestRow RowNeg1 = Row(1, true);
    private static readonly object DynamicRow1 = Row(1, false).ToDynamic();
    private static readonly object DynamicRowNeg1 = Row(1, true).ToDynamic();
    // 4 Branches
    private static readonly TestRow Row4 = Row(4, false);
    private static readonly TestRow RowNeg4 = Row(4, true);
    private static readonly object DynamicRow4 = Row(4, false).ToDynamic();
    private static readonly object DynamicRowNeg4 = Row(4, true).ToDynamic();
    // 8 Branches
    private static readonly TestRow Row8 = Row(8, false);
    private static readonly TestRow RowNeg8 = Row(8, true);
    private static readonly object DynamicRow8 = Row(8, false).ToDynamic();
    private static readonly object DynamicRowNeg8 = Row(8, true).ToDynamic();
    // Null
    private static readonly TestRow RowNull = new TestRow
    {
        P = null,
        O = null
    };
    private static readonly object DynamicRowNull = Row(1, false).ToDynamic();

    [Benchmark(Baseline = true)]
    public void Spel1() => EvalSpel(Row1);
    [Benchmark]
    public void SpelNeg1() => EvalSpel(RowNeg1);
    [Benchmark]
    public void Spel4() => EvalSpel(Row4);
    [Benchmark]
    public void SpelNeg4() => EvalSpel(RowNeg4);
    [Benchmark]
    public void Spel8() => EvalSpel(Row8);
    [Benchmark]
    public void SpelNeg8() => EvalSpel(RowNeg8);
    [Benchmark]
    public void SpelNull() => EvalSpel(RowNull);
    [Benchmark]
    public void Vcel1() => VcelExpr.Evaluate(Row1);
    [Benchmark]
    public void VcelNeg1() => VcelExpr.Evaluate(RowNeg1);
    [Benchmark]
    public void Vcel4() => VcelExpr.Evaluate(Row4);
    [Benchmark]
    public void VcelNeg4() => VcelExpr.Evaluate(RowNeg4);
    [Benchmark]
    public void Vcel8() => VcelExpr.Evaluate(Row8);
    [Benchmark]
    public void VcelNeg8() => VcelExpr.Evaluate(RowNeg8);
    [Benchmark]
    public void VcelNull() => VcelExpr.Evaluate(RowNull);
    [Benchmark]
    public void VcelLetGuard1() => VcelExprLetGuard.Evaluate(Row1);
    [Benchmark]
    public void VcelLetGuardNeg1() => VcelExprLetGuard.Evaluate(RowNeg1);
    [Benchmark]
    public void VcelLetGuard4() => VcelExprLetGuard.Evaluate(Row4);
    [Benchmark]
    public void VcelLetGuardNeg4() => VcelExprLetGuard.Evaluate(RowNeg4);
    [Benchmark]
    public void VcelLetGuard8() => VcelExprLetGuard.Evaluate(Row8);
    [Benchmark]
    public void VcelLetGuardNeg8() => VcelExprLetGuard.Evaluate(RowNeg8);
    [Benchmark]
    public void VcelLetGuardNull() => VcelExprLetGuard.Evaluate(RowNull);
    [Benchmark]
    public void Native1() => CustomExpr.Evaluate(Row1);
    [Benchmark]
    public void NativeNeg1() => CustomExpr.Evaluate(RowNeg1);
    [Benchmark]
    public void Native4() => CustomExpr.Evaluate(Row4);
    [Benchmark]
    public void NativeNeg4() => CustomExpr.Evaluate(RowNeg4);
    [Benchmark]
    public void Native8() => CustomExpr.Evaluate(Row8);
    [Benchmark]
    public void NativeNeg8() => CustomExpr.Evaluate(RowNeg8);
    [Benchmark]
    public void NativeNull() => CustomExpr.Evaluate(RowNull);
    [Benchmark]
    public void CSharpLetGuard1() => CSharpExprLetGuard.Evaluate(DynamicRow1);
    [Benchmark]
    public void CSharpLetGuardNeg1() => CSharpExprLetGuard.Evaluate(DynamicRowNeg1);
    [Benchmark]
    public void CSharpLetGuard4() => CSharpExprLetGuard.Evaluate(DynamicRow4);
    [Benchmark]
    public void CSharpLetGuardNeg4() => CSharpExprLetGuard.Evaluate(DynamicRowNeg4);
    [Benchmark]
    public void CSharpLetGuard8() => CSharpExprLetGuard.Evaluate(DynamicRow8);
    [Benchmark]
    public void CSharpLetGuardNeg8() => CSharpExprLetGuard.Evaluate(DynamicRowNeg8);
    [Benchmark]
    public void CSharpLetGuardNull() => CSharpExprLetGuard.Evaluate(DynamicRowNull);

    private void EvalSpel(TestRow row)
    {
        try
        {
            SpelExpr.GetValue(row);
        }
        catch { }
    }

    private static TestRow Row(int b, bool negCase)
    {
        var p = -0.9286 * b * b - 5.7381 * b + 7.25;
        return new TestRow
        {
            P = negCase ? p : 100 - p,
            O = 100.0
        };
    }
}