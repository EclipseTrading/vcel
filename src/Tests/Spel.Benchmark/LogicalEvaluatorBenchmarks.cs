using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spring.Expressions;
using VCEL;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace Spel.Benchmark;

using SpEx = IExpression;
using VCEx = IExpression<object?>;
using VCMaybeEx = IExpression<Maybe<object>>;
using CSharpEx = IExpression<object?>;

// [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[SimpleJob(RuntimeMoniker.Net60, warmupCount: 3, iterationCount: 3, baseline: true)]
// [SimpleJob(RuntimeMoniker.Net80, warmupCount: 1, iterationCount: 1)]
[MemoryDiagnoser]
// [InProcess]
public class LogicalEvaluatorBenchmarks
{
    private static readonly TestRow Row = new() { O = 1.0, P = 1.0 };
    private static readonly object DynamicRow = new TestRow { O = 1.0, P = 1.0 }.ToDynamic();
    private static readonly IDictionary<object, object> DictionaryRow = new Dictionary<object, object> { ["O"] = 1.0, ["P"] = 1.0 };

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
    public void SpelNotRow() => SpelNotExpr.GetValue(Row);

    [Benchmark]
    public void SpelNotDynamicRow() => SpelNotExpr.GetValue(DynamicRow);

    [Benchmark]
    public void SpelNotDictionaryRow() => SpelNotExpr.GetValue(DictionaryRow);

    [Benchmark]
    public void SpelAndRow() => SpelAndExpr.GetValue(Row);

    [Benchmark]
    public void SpelAndDynamicRow() => SpelAndExpr.GetValue(DynamicRow);

    [Benchmark]
    public void SpelAndDictionaryRow() => SpelAndExpr.GetValue(DictionaryRow);

    [Benchmark]
    public void SpelOrRow() => SpelOrExpr.GetValue(Row);

    [Benchmark]
    public void SpelOrDynamicRow() => SpelOrExpr.GetValue(DynamicRow);

    [Benchmark]
    public void SpelOrDictionaryRow() => SpelOrExpr.GetValue(DictionaryRow);

    [Benchmark]
    public void SpelTernRow() => SpelTernExpr.GetValue(Row);

    [Benchmark]
    public void SpelTernDynamicRow() => SpelTernExpr.GetValue(DynamicRow);

    [Benchmark]
    public void SpelTernDictionaryRow() => SpelTernExpr.GetValue(DictionaryRow);

    [Benchmark]
    public void VcelDefaultNotRow() => VcelDefaultNotExpr.Evaluate(Row);

    [Benchmark]
    public void VcelDefaultNotDynamicRow() => VcelDefaultNotExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelDefaultNotDictionaryRow() => VcelDefaultNotExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelDefaultAndRow() => VcelDefaultAndExpr.Evaluate(Row);

    [Benchmark]
    public void VcelDefaultAndDynamicRow() => VcelDefaultAndExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelDefaultAndDictionaryRow() => VcelDefaultAndExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelDefaultOrRow() => VcelDefaultOrExpr.Evaluate(Row);

    [Benchmark]
    public void VcelDefaultOrDynamicRow() => VcelDefaultOrExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelDefaultOrDictionaryRow() => VcelDefaultOrExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelDefaultTernRow() => VcelDefaultTernExpr.Evaluate(Row);

    [Benchmark]
    public void VcelDefaultTernDynamicRow() => VcelDefaultTernExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelDefaultTernDictionaryRow() => VcelDefaultTernExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelMaybeNotRow() => VcelMaybeNotExpr.Evaluate(Row);

    [Benchmark]
    public void VcelMaybeNotDynamicRow() => VcelMaybeNotExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelMaybeNotDictionaryRow() => VcelMaybeNotExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelMaybeAndRow() => VcelMaybeAndExpr.Evaluate(Row);

    [Benchmark]
    public void VcelMaybeAndDynamicRow() => VcelMaybeAndExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelMaybeAndDictionaryRow() => VcelMaybeAndExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelMaybeOrRow() => VcelMaybeOrExpr.Evaluate(Row);

    [Benchmark]
    public void VcelMaybeOrDynamicRow() => VcelMaybeOrExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelMaybeOrDictionaryRow() => VcelMaybeOrExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void VcelMaybeTernRow() => VcelMaybeTernExpr.Evaluate(Row);

    [Benchmark]
    public void VcelMaybeTernDynamicRow() => VcelMaybeTernExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void VcelMaybeTernDictionaryRow() => VcelMaybeTernExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void CSharpNotRow() => CSharpNotExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpNotDynamicRow() => CSharpNotExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpNotDictionaryRow() => CSharpNotExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void CSharpAndRow() => CSharpAndExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpAndDynamicRow() => CSharpAndExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpAndDictionaryRow() => CSharpAndExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void CSharpOrRow() => CSharpOrExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpOrDynamicRow() => CSharpOrExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpOrDictionaryRow() => CSharpOrExpr.Evaluate(DictionaryRow);

    [Benchmark]
    public void CSharpTernRow() => CSharpTernExpr.Evaluate(Row);

    [Benchmark]
    public void CSharpTernDynamicRow() => CSharpTernExpr.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpTernDictionaryRow() => CSharpTernExpr.Evaluate(DictionaryRow);
}