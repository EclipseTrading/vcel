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
    public class ArithmeticEvaluatorBenchmarks
    {
        private static readonly TestRow Row = new TestRow { O = 1.0, P = 1.0 };
        private static readonly object DynamicRow = new TestRow { O = 1.0, P = 1.0 }.ToDynamic();

        private static readonly SpEx SpelAddExpr = Expression.Parse(Expressions.Add);
        private static readonly SpEx SpelSubExpr = Expression.Parse(Expressions.Subtract);
        private static readonly SpEx SpelDivExpr = Expression.Parse(Expressions.Divide);
        private static readonly SpEx SpelDiv0Expr = Expression.Parse(Expressions.Div0);
        private static readonly SpEx SpelDivNullExpr = Expression.Parse(Expressions.DivNull);
        private static readonly SpEx SpelAbsExpr = Expression.Parse(Expressions.Abs);

        private static readonly VCEx VcelDefaultAddExpr = VCExpression.ParseDefault(Expressions.Add).Expression;
        private static readonly VCEx VcelDefaultSubExpr = VCExpression.ParseDefault(Expressions.Subtract).Expression;
        private static readonly VCEx VcelDefaultDivExpr = VCExpression.ParseDefault(Expressions.Divide).Expression;
        private static readonly VCEx VcelDefaultDiv0Expr = VCExpression.ParseDefault(Expressions.Div0).Expression;
        private static readonly VCEx VcelDefaultDivNullExpr = VCExpression.ParseDefault(Expressions.DivNull).Expression;
        private static readonly VCEx VcelDefaultAbsExpr = VCExpression.ParseDefault(Expressions.Abs).Expression;

        private static readonly VCMaybeEx VcelMaybeAddExpr = VCExpression.ParseMaybe(Expressions.Add).Expression;
        private static readonly VCMaybeEx VcelMaybeSubExpr = VCExpression.ParseMaybe(Expressions.Subtract).Expression;
        private static readonly VCMaybeEx VcelMaybeDivExpr = VCExpression.ParseMaybe(Expressions.Divide).Expression;
        private static readonly VCMaybeEx VcelMaybeDiv0Expr = VCExpression.ParseMaybe(Expressions.Div0).Expression;
        private static readonly VCMaybeEx VcelMaybeDivNullExpr = VCExpression.ParseMaybe(Expressions.DivNull).Expression;
        private static readonly VCMaybeEx VcelMaybeAbsExpr = VCExpression.ParseMaybe(Expressions.Abs).Expression;

        private static readonly CSharpEx CSharpAddExpr = CSharpExpression.ParseDelegate(Expressions.Add).Expression;
        private static readonly CSharpEx CSharpSubExpr = CSharpExpression.ParseDelegate(Expressions.Subtract).Expression;
        private static readonly CSharpEx CSharpDivExpr = CSharpExpression.ParseDelegate(Expressions.Divide).Expression;
        private static readonly CSharpEx CSharpAbsExpr = CSharpExpression.ParseDelegate(Expressions.Abs).Expression;

        [Benchmark(Baseline = true)]
        public void SpelPlus() => EvalSpel(SpelAddExpr);
        [Benchmark]
        public void SpelSubtract() => EvalSpel(SpelSubExpr);
        [Benchmark]
        public void SpelDivide() => EvalSpel(SpelDivExpr);
        [Benchmark]
        public void SpelDiv0() => EvalSpel(SpelDiv0Expr);
        [Benchmark]
        public void SpelDivNull() => EvalSpel(SpelDivNullExpr);
        [Benchmark]
        public void SpelAbs() => EvalSpel(SpelAbsExpr);

        [Benchmark]
        public void VcelDefaultPlus() => VcelDefaultAddExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultSubtract() => VcelDefaultSubExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultDivide() => VcelDefaultDivExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultDiv0() => VcelDefaultDiv0Expr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultDivNull() => VcelDefaultDivNullExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDefaultAbs() => VcelDefaultAbsExpr.Evaluate(Row);

        [Benchmark]
        public void VcelMaybePlus() => VcelMaybeAddExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeSubtract() => VcelMaybeSubExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeDivide() => VcelMaybeDivExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeDiv0() => VcelMaybeDiv0Expr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeDivNull() => VcelMaybeDivNullExpr.Evaluate(Row);
        [Benchmark]
        public void VcelMaybeAbs() => VcelMaybeAbsExpr.Evaluate(Row);

        [Benchmark]
        public void CSharpPlus() => CSharpAddExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpSubtract() => CSharpSubExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpDivide() => CSharpDivExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpAbs() => CSharpAbsExpr.Evaluate(DynamicRow);

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
