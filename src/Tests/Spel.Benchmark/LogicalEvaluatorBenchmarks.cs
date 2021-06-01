using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace Spel.Benchmark
{
    using SpEx = IExpression;
    using VCEx = IExpression<object>;
    using VCMaybeEx = IExpression<Maybe<object>>;
    using CSharpEx = IExpression<object>;

    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LogicalEvaluatorBenchmarks
    {
        private static readonly TestRow Row = new TestRow { O = 1.0, P = 1.0 };
        private static readonly object DynamicRow = new TestRow { O = 1.0, P = 1.0 }.ToDynamic();

        private static readonly SpEx SpelNotExpr = Expression.Parse(Expressions.Not);
        private static readonly SpEx SpelAndExpr = Expression.Parse(Expressions.And);
        private static readonly SpEx SpelOrExpr = Expression.Parse(Expressions.Or);
        private static readonly SpEx SpelTernExpr = Expression.Parse(Expressions.Tern);

        private static readonly VCEx VcelDefaultNotExpr = VCExpression.ParseDefault(Expressions.Not).Expression;
        private static readonly VCEx VcelDefaultAndExpr = VCExpression.ParseDefault(Expressions.And).Expression;
        private static readonly VCEx VcelDefaultOrExpr = VCExpression.ParseDefault(Expressions.Or).Expression;
        private static readonly VCEx VcelDefaultTernExpr = VCExpression.ParseDefault(Expressions.Tern).Expression;

        private static readonly VCMaybeEx VcelMaybeNotExpr = VCExpression.ParseMaybe(Expressions.Not).Expression;
        private static readonly VCMaybeEx VcelMaybeAndExpr = VCExpression.ParseMaybe(Expressions.And).Expression;
        private static readonly VCMaybeEx VcelMaybeOrExpr = VCExpression.ParseMaybe(Expressions.Or).Expression;
        private static readonly VCMaybeEx VcelMaybeTernExpr = VCExpression.ParseMaybe(Expressions.Tern).Expression;

        private static readonly CSharpEx CSharpNotExpr = CSharpExpression.ParseDelegate(Expressions.Not).Expression;
        private static readonly CSharpEx CSharpAndExpr = CSharpExpression.ParseDelegate(Expressions.And).Expression;
        private static readonly CSharpEx CSharpOrExpr = CSharpExpression.ParseDelegate(Expressions.Or).Expression;
        private static readonly CSharpEx CSharpTernExpr = CSharpExpression.ParseDelegate(Expressions.Tern).Expression;

        [Benchmark]
        public void SpelNot() => EvalSpel(SpelNotExpr);
        [Benchmark]
        public void SpelAnd() => EvalSpel(SpelAndExpr);
        [Benchmark]
        public void SpelOr() => EvalSpel(SpelOrExpr);
        [Benchmark]
        public void SpelTern() => EvalSpel(SpelTernExpr);

        [Benchmark]
        public void VcelDefaultNot() => VcelDefaultNotExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultAnd() => VcelDefaultAndExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultOr() => VcelDefaultOrExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultTern() => VcelDefaultTernExpr.Evaluate(Row);

        [Benchmark]
        public void VcelMaybeNot() => VcelMaybeNotExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeAnd() => VcelMaybeAndExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeOr() => VcelMaybeOrExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeTern() => VcelMaybeTernExpr.Evaluate(Row);

        [Benchmark]
        public void CSharpNot() => CSharpNotExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpAnd() => CSharpAndExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpOr() => CSharpOrExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpTern() => CSharpTernExpr.Evaluate(Row);

        private void EvalSpel(SpEx expression)
        {
            try
            {
                expression.GetValue(Row);
            }
            catch { }
        }
    }
}
