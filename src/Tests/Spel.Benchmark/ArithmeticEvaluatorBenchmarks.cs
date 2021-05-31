using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Test.Shared;

namespace Spel.Benchmark
{
    using SpEx = IExpression;
    using VCEx = IExpression<object>;
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

        private static readonly IExpressionParser<object> vcelMonadParser = VCExpression.DefaultParser();

        private static readonly VCEx VcelAddExpr = vcelMonadParser.Parse(Expressions.Add).Expression;
        private static readonly VCEx VcelSubExpr = vcelMonadParser.Parse(Expressions.Subtract).Expression;
        private static readonly VCEx VcelDivExpr = vcelMonadParser.Parse(Expressions.Divide).Expression;
        private static readonly VCEx VcelDiv0Expr = vcelMonadParser.Parse(Expressions.Div0).Expression;
        private static readonly VCEx VcelDivNullExpr = vcelMonadParser.Parse(Expressions.DivNull).Expression;
        private static readonly VCEx VcelAbsExpr = vcelMonadParser.Parse(Expressions.Abs).Expression;

        private static readonly CSharpEx CSharpAddExpr = CSharpExpression.ParseNative(Expressions.Add).Expression;
        private static readonly CSharpEx CSharpSubExpr = CSharpExpression.ParseNative(Expressions.Subtract).Expression;
        private static readonly CSharpEx CSharpDivExpr = CSharpExpression.ParseNative(Expressions.Divide).Expression;
        private static readonly CSharpEx CSharpAbsExpr = CSharpExpression.ParseNative(Expressions.Abs).Expression;

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
        public void VcelPlus() => VcelAddExpr.Evaluate(Row);
        [Benchmark]
        public void VcelSubtract() => VcelSubExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDivide() => VcelDivExpr.Evaluate(Row);
        [Benchmark]
        public void VcelDiv0() => VcelDiv0Expr.Evaluate(Row);
        [Benchmark]
        public void VcelDivNull() => VcelDivNullExpr.Evaluate(Row);
        [Benchmark]
        public void VcelAbs() => VcelAbsExpr.Evaluate(Row);

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
