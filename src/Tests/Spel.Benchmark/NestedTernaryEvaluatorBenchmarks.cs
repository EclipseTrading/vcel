using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Expression;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace Spel.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class NestedTernaryEvaluatorBenchmarks
    {
        private static readonly ExpressionParser<Maybe<object>> vcelMonadParser =
            new ExpressionParser<Maybe<object>>(new MaybeExpressionFactory(MaybeMonad.Instance));
        private static readonly IExpression SpelExpr = Expression.Parse(Expressions.NestedTernary1);
        private static readonly IExpression<Maybe<object>> VcelExpr = vcelMonadParser.Parse(Expressions.NestedTernary1).Expression;
        private static readonly IExpression<Maybe<object>> VcelExprLetGuard = vcelMonadParser.Parse(Expressions.LetGuard).Expression;
        private static readonly IExpression<object> CSharpExprLetGuard = CSharpExpression.ParseNative(Expressions.LetGuard).Expression;
        private static readonly CustomExpr customExpr = new CustomExpr();

        // 1 Branch
        private static readonly TestRow Row1 = Row(1, false);
        private static readonly TestRow RowNeg1 = Row(1, true);
        // 4 Branches
        private static readonly TestRow Row4 = Row(4, false);
        private static readonly TestRow RowNeg4 = Row(4, true);
        // 8 Branches
        private static readonly TestRow Row8 = Row(8, false);
        private static readonly TestRow RowNeg8 = Row(8, true);
        // Null
        private static readonly TestRow RowNull = new TestRow
        {
            P = null,
            O = null
        };

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
        public void Native1() => customExpr.Evaluate(Row1);
        [Benchmark]
        public void NativeNeg1() => customExpr.Evaluate(RowNeg1);
        [Benchmark]
        public void Native4() => customExpr.Evaluate(Row4);
        [Benchmark]
        public void NativeNeg4() => customExpr.Evaluate(RowNeg4);
        [Benchmark]
        public void Native8() => customExpr.Evaluate(Row8);
        [Benchmark]
        public void NativeNeg8() => customExpr.Evaluate(RowNeg8);
        [Benchmark]
        public void NativeNull() => customExpr.Evaluate(RowNull);
        [Benchmark]
        public void CSharpLetGuard1() => CSharpExprLetGuard.Evaluate(Row1);
        [Benchmark]
        public void CSharpLetGuardNeg1() => CSharpExprLetGuard.Evaluate(RowNeg1);
        [Benchmark]
        public void CSharpLetGuard4() => CSharpExprLetGuard.Evaluate(Row4);
        [Benchmark]
        public void CSharpLetGuardNeg4() => CSharpExprLetGuard.Evaluate(RowNeg4);
        [Benchmark]
        public void CSharpLetGuard8() => CSharpExprLetGuard.Evaluate(Row8);
        [Benchmark]
        public void CSharpLetGuardNeg8() => CSharpExprLetGuard.Evaluate(RowNeg8);
        [Benchmark]
        public void CSharpLetGuardNull() => CSharpExprLetGuard.Evaluate(RowNull);

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
}
