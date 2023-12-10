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
    private readonly IntrinsicAddOp intrinsicAdd = new(
        IntrinsicsMonad.Instance,
        new Property<float[]>(IntrinsicsMonad.Instance, "a"),
        new Property<float[]>(IntrinsicsMonad.Instance, "b"));

    private readonly AddExpr<float> addOp = new(
        ExprMonad<float>.FloatMonad,
        new Property<float>(ExprMonad<float>.FloatMonad, "a"),
        new Property<float>(ExprMonad<float>.FloatMonad, "b")
    );

    private int lastLen = 0;

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void RunIntrinsicsAdd(List<Dictionary<string, object>> rows, int iters)
    {
        var res = intrinsicAdd.Evaluate(new IntrinsicContext(rows));
        lastLen = res.Length;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void RunAdd(List<Dictionary<string, object>> rows, int iters)
    {
        float res = 0;
        for (var i = 0; i < rows.Count; i++)
        {
            res = addOp.Evaluate(new DictionaryContext<float>(ExprMonad<float>.FloatMonad, rows[i]));
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
            
            res = a + b;
        }
        
        lastLen = (int)res;
    }
    
    public IEnumerable<object[]> Data()
    {
        int[] iters = [1, 5, 10, 100, 1000, 10000];
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