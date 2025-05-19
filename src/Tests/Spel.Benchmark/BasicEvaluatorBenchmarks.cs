using System;
using BenchmarkDotNet.Attributes;
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

[Config(typeof(InProcessShortRunConfig))]
[RankColumn]
public class BasicEvaluatorBenchmarks
{
    private static readonly TestRow Row = new TestRow
        { O = 1.0, P = 1.0, tsA = DateTime.MinValue, tsB = DateTime.MaxValue };

    private static readonly object DynamicRow =
        new TestRow { O = 1.0, P = 1.0, tsA = DateTime.MinValue, tsB = DateTime.MaxValue }.ToDynamic();

    private static readonly SpEx SpelNowExpr = Expression.Parse(Expressions.DateTime);
    private static readonly SpEx SpelGetPropExpr = Expression.Parse(Expressions.GetProp);
    private static readonly SpEx SpelExpr0 = Expression.Parse(Expressions.TestExpr0);
    private static readonly SpEx SpelExpr1 = Expression.Parse(Expressions.TestExpr1);
    private static readonly SpEx SpelExpr2 = Expression.Parse(Expressions.TestExpr2);
    private static readonly SpEx SpelExpr3 = Expression.Parse(Expressions.TestExpr3);
    private static readonly SpEx SpelExpr4 = Expression.Parse(Expressions.TestExpr4);
    private static readonly SpEx SpelExpr5 = Expression.Parse(Expressions.TestExpr5);
    private static readonly SpEx SpelExpr8 = Expression.Parse(Expressions.TestExpr10);

    private static readonly VCEx VcelDefaultNowExpr = VCExpression.ParseDefault(Expressions.DateTime).Expression;
    private static readonly VCEx VcelDefaultGetPropExpr = VCExpression.ParseDefault(Expressions.GetProp).Expression;
    private static readonly VCEx VcelDefaultExpr0 = VCExpression.ParseDefault(Expressions.TestExpr0).Expression;
    private static readonly VCEx VcelDefaultExpr1 = VCExpression.ParseDefault(Expressions.TestExpr1).Expression;
    private static readonly VCEx VcelDefaultExpr2 = VCExpression.ParseDefault(Expressions.TestExpr2).Expression;
    private static readonly VCEx VcelDefaultExpr3 = VCExpression.ParseDefault(Expressions.TestExpr3).Expression;
    private static readonly VCEx VcelDefaultExpr4 = VCExpression.ParseDefault(Expressions.TestExpr4).Expression;
    private static readonly VCEx VcelDefaultExpr5 = VCExpression.ParseDefault(Expressions.TestExpr5).Expression;
    private static readonly VCEx VcelDefaultExpr8 = VCExpression.ParseDefault(Expressions.TestExpr10).Expression;

    private static readonly VCMaybeEx VcelMaybeNowExpr = VCExpression.ParseMaybe(Expressions.DateTime).Expression;
    private static readonly VCMaybeEx VcelMaybeGetPropExpr = VCExpression.ParseMaybe(Expressions.GetProp).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr0 = VCExpression.ParseMaybe(Expressions.TestExpr0).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr1 = VCExpression.ParseMaybe(Expressions.TestExpr1).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr2 = VCExpression.ParseMaybe(Expressions.TestExpr2).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr3 = VCExpression.ParseMaybe(Expressions.TestExpr3).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr4 = VCExpression.ParseMaybe(Expressions.TestExpr4).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr5 = VCExpression.ParseMaybe(Expressions.TestExpr5).Expression;
    private static readonly VCMaybeEx VcelMaybeExpr8 = VCExpression.ParseMaybe(Expressions.TestExpr10).Expression;

    private static readonly CSharpEx CSharpNowExpr = CSharpExpression.ParseDelegate(Expressions.DateTime).Expression;
    private static readonly CSharpEx CSharpGetPropExpr = CSharpExpression.ParseDelegate(Expressions.GetProp).Expression;
    private static readonly CSharpEx CSharpExpr0 = CSharpExpression.ParseDelegate(Expressions.TestExpr0).Expression;
    private static readonly CSharpEx CSharpExpr1 = CSharpExpression.ParseDelegate(Expressions.TestExpr1).Expression;
    private static readonly CSharpEx CSharpExpr2 = CSharpExpression.ParseDelegate(Expressions.TestExpr2).Expression;
    private static readonly CSharpEx CSharpExpr3 = CSharpExpression.ParseDelegate(Expressions.TestExpr3).Expression;
    private static readonly CSharpEx CSharpExpr4 = CSharpExpression.ParseDelegate(Expressions.TestExpr4).Expression;
    private static readonly CSharpEx CSharpExpr5 = CSharpExpression.ParseDelegate(Expressions.TestExpr5).Expression;
    private static readonly CSharpEx CSharpExpr8 = CSharpExpression.ParseDelegate(Expressions.TestExpr10).Expression;

    // [Benchmark]
    // public void SpelNow() => EvalSpel(SpelNowExpr);
    //
    // [Benchmark]
    // public void SpelGetProp() => EvalSpel(SpelGetPropExpr);
    //
    // [Benchmark]
    // public void SpelExp0() => EvalSpel(SpelExpr0);
    //
    // [Benchmark]
    // public void SpelExp1() => EvalSpel(SpelExpr1);
    //
    // [Benchmark]
    // public void SpelExp2() => EvalSpel(SpelExpr2);
    //
    // [Benchmark]
    // public void SpelExp3() => EvalSpel(SpelExpr3);
    //
    // [Benchmark]
    // public void SpelExp4() => EvalSpel(SpelExpr4);
    //
    // [Benchmark]
    // public void SpelExp5() => EvalSpel(SpelExpr5);
    //
    // [Benchmark]
    // public void SpelExp8() => EvalSpel(SpelExpr8);

    //
    // [Benchmark]
    // public void VcelDefaultNow() => VcelDefaultNowExpr.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultGetProp() => VcelDefaultGetPropExpr.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp0() => VcelDefaultExpr0.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp1() => VcelDefaultExpr1.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp2() => VcelDefaultExpr2.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp3() => VcelDefaultExpr3.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp4() => VcelDefaultExpr4.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelDefaultExp5() => VcelDefaultExpr5.Evaluate(Row);
    //
    [Benchmark]
    public void VcelDefaultExp8() => VcelDefaultExpr8.Evaluate(Row);


    // [Benchmark]
    // public void VcelMaybeNow() => VcelMaybeNowExpr.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeGetProp() => VcelMaybeGetPropExpr.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp0() => VcelMaybeExpr0.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp1() => VcelMaybeExpr1.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp2() => VcelMaybeExpr2.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp3() => VcelMaybeExpr3.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp4() => VcelMaybeExpr4.Evaluate(Row);
    //
    // [Benchmark]
    // public void VcelMaybeExp5() => VcelMaybeExpr5.Evaluate(Row);

    [Benchmark]
    public void VcelMaybeExp8() => VcelMaybeExpr8.Evaluate(Row);
    //
    //
    // [Benchmark]
    // public void CSharpNow() => CSharpNowExpr.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpGetProp() => CSharpGetPropExpr.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp0() => CSharpExpr0.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp1() => CSharpExpr1.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp2() => CSharpExpr2.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp3() => CSharpExpr3.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp4() => CSharpExpr4.Evaluate(DynamicRow);
    //
    // [Benchmark]
    // public void CSharpExp5() => CSharpExpr5.Evaluate(DynamicRow);

    [Benchmark]
    public void CSharpExp8() => CSharpExpr8.Evaluate(DynamicRow);

    private void EvalSpel(SpEx expression)
    {
        try
        {
            expression.GetValue(Row);
        }
        catch
        {
        }
    }
}