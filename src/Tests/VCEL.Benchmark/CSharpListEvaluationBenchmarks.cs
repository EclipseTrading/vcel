using BenchmarkDotNet.Attributes;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace VCEL.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class CSharpListEvaluationBenchmarks
{

    private static readonly IExpression<Maybe<object>> VcelExprListGuard = VCExpression.ParseMaybe(Expressions.setGuard).Expression;
    private static readonly IExpression<object?> CSharpExprListGuard = CSharpExpression.ParseMethod(Expressions.setGuard).Expression;
    private static readonly IExpression<object?> CSharpExprListGuard_Members = CSharpExpression.ParseMethodWithMembers(Expressions.setGuard).Expression;

    private static readonly object testRow = new{ p = "p1", d = "d1"};

    [Benchmark(Baseline = true)]
    public void VcelListGuard() => VcelExprListGuard.Evaluate(testRow);
    [Benchmark]
    public void CSharpListGuard() => CSharpExprListGuard.Evaluate(testRow);
    [Benchmark]
    public void CSharpListGuard_Members() => CSharpExprListGuard_Members.Evaluate(testRow);
}
