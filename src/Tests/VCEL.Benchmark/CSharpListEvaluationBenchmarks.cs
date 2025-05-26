using BenchmarkDotNet.Attributes;
using VCEL.Core.Helper;
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
    record Row(string p, string d);

    private static readonly IExpression<Maybe<object>> VcelExprListGuard = VCExpression.ParseMaybe(Expressions.setGuard).Expression;
    private static readonly IExpression<object?> CSharpExprListGuard = CSharpExpression.ParseDelegate(Expressions.setGuard).Expression;
    private static readonly IExpression<object?> CSharpExprListGuard_Members = CSharpExpression.ParseDelegateWithMembers(Expressions.setGuard).Expression;

    private static readonly Row testRow = new("p1", "d1");
    private static readonly dynamic dynamicRow = testRow.ToDynamic();

    [Benchmark(Baseline = true)]
    public void VcelListGuard() => VcelExprListGuard.Evaluate(testRow);
    [Benchmark]
    public void CSharpListGuard() => ObjectContextExtensions.Evaluate(CSharpExprListGuard, dynamicRow);
    [Benchmark]
    public void CSharpListGuard_Members() => ObjectContextExtensions.Evaluate(CSharpExprListGuard_Members, dynamicRow);
}
