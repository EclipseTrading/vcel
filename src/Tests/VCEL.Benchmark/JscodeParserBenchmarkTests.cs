using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.JS;
using VCEL.Test.Shared;

namespace VCEL.Benchmark
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    public class JscodeParserBenchmarkTests
    {
        private readonly static IExpressionParser<string> jsCodeparser = new ExpressionParser<string>(new ToJsCodeFactory<string>(ConcatStringMonad.Instance));
        private readonly static IExpressionParser<object?> vcelParser = VCExpression.DefaultParser();


        [Benchmark(Baseline = true)]

        public void BaseExpressionVCELParser() => vcelParser.Parse(Expressions.Add);


        [Benchmark]
        public void BaseExpressionJSParser() => jsCodeparser.Parse(Expressions.Add);
        [Benchmark]
        public void InExpressionJSParser() => jsCodeparser.Parse(Expressions.InEnd);
        [Benchmark]
        public void MatchesExpressionJSParser() => jsCodeparser.Parse(Expressions.MatchesComplex);
        [Benchmark]
        public void BetweenExpressionJSParser() => jsCodeparser.Parse(Expressions.Between);
        [Benchmark]
        public void TernaryArith1ExpressionJSParser() => jsCodeparser.Parse(Expressions.TernaryArith1);
        [Benchmark]
        public void TernaryArith2ExpressionJSParser() => jsCodeparser.Parse(Expressions.TernaryArith2);
        [Benchmark]
        public void NestedTernary1ExpressionJSParser() => jsCodeparser.Parse(Expressions.NestedTernary1);
        [Benchmark]
        public void LetGuardExpressionJSParser() => jsCodeparser.Parse(Expressions.LetGuard);
        [Benchmark]
        public void FunctionExpressionJSParser() => jsCodeparser.Parse(Expressions.FunctionExpr);
        [Benchmark]
        public void TestExpression6JSParser() => jsCodeparser.Parse(Expressions.TestExpr6);
        [Benchmark]
        public void TestExpression7JSParser() => jsCodeparser.Parse(Expressions.TestExpr7);
    }
}
