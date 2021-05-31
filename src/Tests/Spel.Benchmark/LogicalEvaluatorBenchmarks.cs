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
    public class LogicalEvaluatorBenchmarks
    {
        private static readonly TestRow Row = new TestRow { O = 1.0, P = 1.0 };
        private static readonly object DynamicRow = new TestRow { O = 1.0, P = 1.0 }.ToDynamic();

        private static readonly SpEx SpelNotExpr = Expression.Parse(Expressions.Not);
        private static readonly SpEx SpelAndExpr = Expression.Parse(Expressions.And);
        private static readonly SpEx SpelOrExpr = Expression.Parse(Expressions.Or);
        private static readonly SpEx SpelTernExpr = Expression.Parse(Expressions.Tern);

        private static readonly IExpressionParser<object> vcelMonadParser = VCExpression.DefaultParser();

        private static readonly VCEx VcelNotExpr = vcelMonadParser.Parse(Expressions.Not).Expression;
        private static readonly VCEx VcelAndExpr = vcelMonadParser.Parse(Expressions.And).Expression;
        private static readonly VCEx VcelOrExpr = vcelMonadParser.Parse(Expressions.Or).Expression;
        private static readonly VCEx VcelTernExpr = vcelMonadParser.Parse(Expressions.Tern).Expression;

        private static readonly CSharpEx CSharpNotExpr = CSharpExpression.ParseNative(Expressions.Not).Expression;
        private static readonly CSharpEx CSharpAndExpr = CSharpExpression.ParseNative(Expressions.And).Expression;
        private static readonly CSharpEx CSharpOrExpr = CSharpExpression.ParseNative(Expressions.Or).Expression;
        private static readonly CSharpEx CSharpTernExpr = CSharpExpression.ParseNative(Expressions.Tern).Expression;

        [Benchmark]
        public void SpelNot() => EvalSpel(SpelNotExpr);
        [Benchmark]
        public void SpelAnd() => EvalSpel(SpelAndExpr);
        [Benchmark]
        public void SpelOr() => EvalSpel(SpelOrExpr);
        [Benchmark]
        public void SpelTern() => EvalSpel(SpelTernExpr);

        [Benchmark]
        public void VcelNot() => VcelNotExpr.Evaluate(Row);
        [Benchmark]
        public void VcelAnd() => VcelAndExpr.Evaluate(Row);
        [Benchmark]
        public void VcelOr() => VcelOrExpr.Evaluate(Row);
        [Benchmark]
        public void VcelTern() => VcelTernExpr.Evaluate(Row);

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
