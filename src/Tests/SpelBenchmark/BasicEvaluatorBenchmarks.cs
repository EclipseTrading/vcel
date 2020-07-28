using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad.Maybe;
using VCEL;
using VCEL.Test.Shared;

namespace SpelBenchmark
{
    using VCEx = IExpression<Maybe<object>>;
    using SpEx = IExpression;

    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class BasicEvaluatorBenchmarks
    {
        private static readonly ExpressionParser<Maybe<object>> vcelMonadParser =
            new ExpressionParser<Maybe<object>>(new MaybeExpressionFactory(MaybeMonad.Instance));
        private static readonly TestRow Row = new TestRow { O = 1.0, P = 1.0 };

        private static readonly VCEx VcelAddExpr = vcelMonadParser.Parse(Expressions.Add);
        private static readonly VCEx VcelSubExpr = vcelMonadParser.Parse(Expressions.Subtract);
        private static readonly VCEx VcelDivExpr = vcelMonadParser.Parse(Expressions.Divide);
        private static readonly VCEx VcelDiv0Expr = vcelMonadParser.Parse(Expressions.Div0);
        private static readonly VCEx VcelDivNullExpr = vcelMonadParser.Parse(Expressions.DivNull);
        private static readonly VCEx VcelAbsExpr = vcelMonadParser.Parse(Expressions.Abs);
        private static readonly VCEx VcelNowExpr = vcelMonadParser.Parse(Expressions.DateTime);
        private static readonly VCEx VcelNotExpr = vcelMonadParser.Parse(Expressions.Not);
        private static readonly VCEx VcelAndExpr = vcelMonadParser.Parse(Expressions.And);
        private static readonly VCEx VcelOrExpr = vcelMonadParser.Parse(Expressions.Or);
        private static readonly VCEx VcelTernExpr = vcelMonadParser.Parse(Expressions.Tern);
        private static readonly VCEx VcelGetPropExpr = vcelMonadParser.Parse(Expressions.GetProp);
        private static readonly VCEx VcelExpr0 = vcelMonadParser.Parse(Expressions.TestExpr0);
        private static readonly VCEx VcelExpr1 = vcelMonadParser.Parse(Expressions.TestExpr1);
        private static readonly VCEx VcelExpr2 = vcelMonadParser.Parse(Expressions.TestExpr2);
        private static readonly VCEx VcelExpr3 = vcelMonadParser.Parse(Expressions.TestExpr3);
        private static readonly VCEx VcelExpr4 = vcelMonadParser.Parse(Expressions.TestExpr4);
        private static readonly VCEx VcelExpr5 = vcelMonadParser.Parse(Expressions.TestExpr5);

        private static readonly SpEx SpelAddExpr = Expression.Parse(Expressions.Add);
        private static readonly SpEx SpelSubExpr = Expression.Parse(Expressions.Subtract);
        private static readonly SpEx SpelDivExpr = Expression.Parse(Expressions.Divide);
        private static readonly SpEx SpelDiv0Expr = Expression.Parse(Expressions.Div0);
        private static readonly SpEx SpelDivNullExpr = Expression.Parse(Expressions.DivNull);
        private static readonly SpEx SpelAbsExpr = Expression.Parse(Expressions.Abs);
        private static readonly SpEx SpelNowExpr = Expression.Parse(Expressions.DateTime);
        private static readonly SpEx SpelNotExpr = Expression.Parse(Expressions.Not);
        private static readonly SpEx SpelAndExpr = Expression.Parse(Expressions.And);
        private static readonly SpEx SpelOrExpr = Expression.Parse(Expressions.Or);
        private static readonly SpEx SpelTernExpr = Expression.Parse(Expressions.Tern);
        private static readonly SpEx SpelGetPropExpr = Expression.Parse(Expressions.GetProp);
        private static readonly SpEx SpelExpr0 = Expression.Parse(Expressions.TestExpr0);
        private static readonly SpEx SpelExpr1 = Expression.Parse(Expressions.TestExpr1);
        private static readonly SpEx SpelExpr2 = Expression.Parse(Expressions.TestExpr2);
        private static readonly SpEx SpelExpr3 = Expression.Parse(Expressions.TestExpr3);
        private static readonly SpEx SpelExpr4 = Expression.Parse(Expressions.TestExpr4);
        private static readonly SpEx SpelExpr5 = Expression.Parse(Expressions.TestExpr5);

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
        public void VcelNow() => VcelNowExpr.Evaluate(Row);
        [Benchmark]
        public void VcelNot() => VcelNotExpr.Evaluate(Row);
        [Benchmark]
        public void VcelAnd() => VcelAndExpr.Evaluate(Row);
        [Benchmark]
        public void VcelOr() => VcelOrExpr.Evaluate(Row);
        [Benchmark]
        public void VcelTern() => VcelTernExpr.Evaluate(Row);
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
        public void SpelNow() => EvalSpel(SpelNowExpr);
        [Benchmark]
        public void SpelNot() => EvalSpel(SpelNotExpr);
        [Benchmark]
        public void SpelAnd() => EvalSpel(SpelAndExpr);
        [Benchmark]
        public void SpelOr() => EvalSpel(SpelOrExpr);
        [Benchmark]
        public void SpelTern() => EvalSpel(SpelTernExpr);
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
        public void SpelExp5() => EvalSpel(SpelExpr4);

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
