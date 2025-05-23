﻿using BenchmarkDotNet.Attributes;
using Spring.Expressions;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace Spel.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ParserBenchmarks
{
    private static readonly ExpressionParser<object?> vcelDefaultParser = new(new ExpressionFactory<object?>(ExprMonad.Instance));
    private static readonly ExpressionParser<Maybe<object>> vcelMonadParser = new(new MaybeExpressionFactory(MaybeMonad.Instance));
    private static readonly string traderExpr = Expressions.NestedTernary1;

    [Benchmark(Baseline = true)]
    public void ParseAddExprWithSpel()
    {
        var exprString = Expressions.Add;
        var expr = Expression.Parse(exprString);
    }

    [Benchmark]
    public void ParseAddExprWithVcel()
    {
        var exprString = Expressions.Add;
        var expr = vcelDefaultParser.Parse(exprString);
    }

    [Benchmark(Baseline = true)]
    public void ParseMaxWithSpel()
    {
        var exprString = "max('C', 'B', 'A')";
        var expr = Expression.Parse(exprString);
    }

    [Benchmark]
    public void ParseMaxWithVcel()
    {
        var exprString = "max('C', 'B', 'A')";
        var expr = vcelDefaultParser.Parse(exprString);
    }

    [Benchmark]
    public void ParseTraderExprWithSpel()
    {
        var expr = Expression.Parse(traderExpr);
    }


    [Benchmark]
    public void ParseTraderExprWithVcel()
    {
        var expr = vcelMonadParser.Parse(traderExpr);
    }
}