using BenchmarkDotNet.Attributes;
using VCEL;
using VCEL.Core.Expression.Impl;
using VCEL.Intrinsics;
using VCEL.Intrinsics.Expression;
using VCEL.Monad;

namespace Intrinsics.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
[RankColumn]
public class IntrinsicsBenchmarks
{
    private readonly static IntrinsicBinaryOp<float> intrinsicAdd = new(
        IntrinsicsMonad<float>.Instance,
        new Property<ReadOnlyMemory<float>>(IntrinsicsMonad<float>.Instance, "a"),
        new Property<ReadOnlyMemory<float>>(IntrinsicsMonad<float>.Instance, "b"),
        AvxGeneric.Add);

    private readonly static IntrinsicBinaryOp<float> intrinsicsMult = new IntrinsicBinaryOp<float>(
        IntrinsicsMonad<float>.Instance,
        intrinsicAdd,
        new Property<ReadOnlyMemory<float>>(IntrinsicsMonad<float>.Instance, "a"),
        AvxGeneric.Multiply);

    private readonly static AddExpr<float> addOp = new(
        ExprMonad<float>.FloatMonad,
        new Property<float>(ExprMonad<float>.FloatMonad, "a"),
        new Property<float>(ExprMonad<float>.FloatMonad, "b")
    );

    private readonly static MultExpr<float> multOp = new(
        ExprMonad<float>.FloatMonad,
        addOp,
        new Property<float>(ExprMonad<float>.FloatMonad, "a"));

    private int lastLen = 0;

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void RunIntrinsicsAdd(List<Dictionary<string, object>> rows, int iters)
    {
        var res = intrinsicsMult.Evaluate(new IntrinsicContext<float>(rows));
        lastLen = res.Length;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void RunAdd(List<Dictionary<string, object>> rows, int iters)
    {
        float res = 0;
        for (var i = 0; i < rows.Count; i++)
        {
            res = multOp.Evaluate(new DictionaryContext<float>(ExprMonad<float>.FloatMonad, rows[i]));
        }
        
        lastLen = (int)res;
    }

    
    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void RunInlineCode(List<Dictionary<string, object>> rows, int iters)
    {
        float res = 0;
        for (var i = 0; i < rows.Count; i++)
        {
            var a = (float)rows[i]["a"];
            var b = (float)rows[i]["b"];
            
            res = (a * b) + a;
        }
        
        lastLen = (int)res;
    }
    
    public IEnumerable<object[]> Data()
    {
        int[] iters = [1, 5, 10, 500];
        foreach (var iter in iters)
        {
            var rows = new List<Dictionary<string, object>>();
            for (var i = 0; i < iter; i++)
            {
                var f = new Dictionary<string, object>
                {
                    ["a"] = Random.Shared.NextSingle(),
                    ["b"] = Random.Shared.NextSingle()
                };
                rows.Add(f);
            }
            
            yield return new object[] { rows, iter };
        }
    }
}