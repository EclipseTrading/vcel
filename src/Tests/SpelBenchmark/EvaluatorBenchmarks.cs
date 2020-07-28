using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad.Maybe;
using VCEL;
using VCEL.Test.Shared;

namespace SpelBenchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class EvaluatorBenchmarks
    {
        private static readonly ExpressionParser<Maybe<object>> vcelMonadParser =
            new ExpressionParser<Maybe<object>>(new MaybeExpressionFactory(MaybeMonad.Instance));
        private static readonly IExpression SpelExpr = Expression.Parse(Expressions.NestedTernary1);
        private static readonly IExpression<Maybe<object>> VcelExpr = vcelMonadParser.Parse(Expressions.NestedTernary1);
        private static readonly IExpression<Maybe<object>> VcelExprLetGuard = vcelMonadParser.Parse(Expressions.LetGuard);
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
            PosSwimDelta = null,
            OptionEquivalentSplitPosition = null
        };

        [Benchmark(Baseline = true)]
        public void DeltaBucketSpel1() => EvalSpel(Row1);
        [Benchmark]
        public void DeltaBucketSpelNeg1() => EvalSpel(RowNeg1);
        [Benchmark]
        public void DeltaBucketSpel4() => EvalSpel(Row4);
        [Benchmark]
        public void DeltaBucketSpelNeg4() => EvalSpel(RowNeg4);
        [Benchmark]
        public void DeltaBucketSpel8() => EvalSpel(Row8);
        [Benchmark]
        public void DeltaBucketSpelNeg8() => EvalSpel(RowNeg8);
        [Benchmark]
        public void DeltaBucketSpelNull() => EvalSpel(RowNull);
        [Benchmark]
        public void DeltaBucketVcel1() => VcelExpr.Evaluate(Row1);
        [Benchmark]
        public void DeltaBucketVcelNeg1() => VcelExpr.Evaluate(RowNeg1);
        [Benchmark]
        public void DeltaBucketVcel4() => VcelExpr.Evaluate(Row4);
        [Benchmark]
        public void DeltaBucketVcelNeg4() => VcelExpr.Evaluate(RowNeg4);
        [Benchmark]
        public void DeltaBucketVcel8() => VcelExpr.Evaluate(Row8);
        [Benchmark]
        public void DeltaBucketVcelNeg8() => VcelExpr.Evaluate(RowNeg8);
        [Benchmark]
        public void DeltaBucketVcelNull() => VcelExpr.Evaluate(RowNull);
        [Benchmark]
        public void DeltaBucketVcel1LetGuard() => VcelExprLetGuard.Evaluate(Row1);
        [Benchmark]
        public void DeltaBucketVcelNeg1LetGuard() => VcelExprLetGuard.Evaluate(RowNeg1);
        [Benchmark]
        public void DeltaBucketVcel4LetGuard() => VcelExprLetGuard.Evaluate(Row4);
        [Benchmark]
        public void DeltaBucketVcelNeg4LetGuard() => VcelExprLetGuard.Evaluate(RowNeg4);
        [Benchmark]
        public void DeltaBucketVcel8LetGuard() => VcelExprLetGuard.Evaluate(Row8);
        [Benchmark]
        public void DeltaBucketVcelNeg8LetGuard() => VcelExprLetGuard.Evaluate(RowNeg8);
        [Benchmark]
        public void DeltaBucketVcelNullLetGuard() => VcelExprLetGuard.Evaluate(RowNull);
        [Benchmark]
        public void DeltaBucketNative1() => customExpr.Evaluate(Row1);
        [Benchmark]
        public void DeltaBucketNativeNeg1() => customExpr.Evaluate(RowNeg1);
        [Benchmark]
        public void DeltaBucketNative4() => customExpr.Evaluate(Row4);
        [Benchmark]
        public void DeltaBucketNativeNeg4() => customExpr.Evaluate(RowNeg4);
        [Benchmark]
        public void DeltaBucketNative8() => customExpr.Evaluate(Row8);
        [Benchmark]
        public void DeltaBucketNativeNeg8() => customExpr.Evaluate(RowNeg8);
        [Benchmark]
        public void DeltaBucketNativeNull() => customExpr.Evaluate(RowNull);

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
                PosSwimDelta = negCase ? p : 100 - p,
                OptionEquivalentSplitPosition = 100.0
            };
        }

    }
}
