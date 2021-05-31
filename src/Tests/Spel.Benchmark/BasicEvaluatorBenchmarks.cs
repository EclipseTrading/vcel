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
    public class BasicEvaluatorBenchmarks
    {
        private static readonly TestRow Row = new TestRow { O = 1.0, P = 1.0 };
        private static readonly object DynamicRow = new TestRow { O = 1.0, P = 1.0 }.ToDynamic();

        private static readonly SpEx SpelNowExpr = Expression.Parse(Expressions.DateTime);
        private static readonly SpEx SpelGetPropExpr = Expression.Parse(Expressions.GetProp);
        private static readonly SpEx SpelExpr0 = Expression.Parse(Expressions.TestExpr0);
        private static readonly SpEx SpelExpr1 = Expression.Parse(Expressions.TestExpr1);
        private static readonly SpEx SpelExpr2 = Expression.Parse(Expressions.TestExpr2);
        private static readonly SpEx SpelExpr3 = Expression.Parse(Expressions.TestExpr3);
        private static readonly SpEx SpelExpr4 = Expression.Parse(Expressions.TestExpr4);
        private static readonly SpEx SpelExpr5 = Expression.Parse(Expressions.TestExpr5);

        private static readonly IExpressionParser<object> vcelMonadParser = VCExpression.DefaultParser();

        private static readonly VCEx VcelNowExpr = vcelMonadParser.Parse(Expressions.DateTime).Expression;
        private static readonly VCEx VcelGetPropExpr = vcelMonadParser.Parse(Expressions.GetProp).Expression;
        private static readonly VCEx VcelExpr0 = vcelMonadParser.Parse(Expressions.TestExpr0).Expression;
        private static readonly VCEx VcelExpr1 = vcelMonadParser.Parse(Expressions.TestExpr1).Expression;
        private static readonly VCEx VcelExpr2 = vcelMonadParser.Parse(Expressions.TestExpr2).Expression;
        private static readonly VCEx VcelExpr3 = vcelMonadParser.Parse(Expressions.TestExpr3).Expression;
        private static readonly VCEx VcelExpr4 = vcelMonadParser.Parse(Expressions.TestExpr4).Expression;
        private static readonly VCEx VcelExpr5 = vcelMonadParser.Parse(Expressions.TestExpr5).Expression;

        private static readonly CSharpEx CSharpNowExpr = CSharpExpression.ParseNative(Expressions.DateTime).Expression;
        private static readonly CSharpEx CSharpGetPropExpr = CSharpExpression.ParseNative(Expressions.GetProp).Expression;
        private static readonly CSharpEx CSharpExpr0 = CSharpExpression.ParseNative(Expressions.TestExpr0).Expression;
        private static readonly CSharpEx CSharpExpr1 = CSharpExpression.ParseNative(Expressions.TestExpr1).Expression;
        private static readonly CSharpEx CSharpExpr2 = CSharpExpression.ParseNative(Expressions.TestExpr2).Expression;
        private static readonly CSharpEx CSharpExpr3 = CSharpExpression.ParseNative(Expressions.TestExpr3).Expression;
        private static readonly CSharpEx CSharpExpr4 = CSharpExpression.ParseNative(Expressions.TestExpr4).Expression;
        private static readonly CSharpEx CSharpExpr5 = CSharpExpression.ParseNative(Expressions.TestExpr5).Expression;

        [Benchmark]
        public void SpelNow() => EvalSpel(SpelNowExpr);
        [Benchmark]
        public void SpelGetProp() => EvalSpel(SpelGetPropExpr);
        [Benchmark]
        public void SpelExp0() => EvalSpel(SpelExpr0);
        [Benchmark]
        public void SpelExp1() => EvalSpel(SpelExpr1);
        [Benchmark]
        public void SpelExp2() => EvalSpel(SpelExpr2);
        [Benchmark]
        public void SpelExp3() => EvalSpel(SpelExpr3);
        [Benchmark]
        public void SpelExp4() => EvalSpel(SpelExpr4);
        [Benchmark]
        public void SpelExp5() => EvalSpel(SpelExpr5);


        [Benchmark]
        public void VcelNow() => VcelNowExpr.Evaluate(Row);
        [Benchmark]
        public void VcelGetProp() => VcelGetPropExpr.Evaluate(Row);
        [Benchmark]
        public void VcelExp0() => VcelExpr0.Evaluate(Row);
        [Benchmark]
        public void VcelExp1() => VcelExpr1.Evaluate(Row);
        [Benchmark]
        public void VcelExp2() => VcelExpr2.Evaluate(Row);
        [Benchmark]
        public void VcelExp3() => VcelExpr3.Evaluate(Row);
        [Benchmark]
        public void VcelExp4() => VcelExpr4.Evaluate(Row);
        [Benchmark]
        public void VcelExp5() => VcelExpr5.Evaluate(Row);

        [Benchmark]
        public void CSharpNow() => CSharpNowExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpGetProp() => CSharpGetPropExpr.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp0() => CSharpExpr0.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp1() => CSharpExpr1.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp2() => CSharpExpr2.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp3() => CSharpExpr3.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp4() => CSharpExpr4.Evaluate(DynamicRow);
        [Benchmark]
        public void CSharpExp5() => CSharpExpr5.Evaluate(DynamicRow);

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
