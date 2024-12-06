using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using VCEL.Core.Lang;
using VCEL.CSharp;

namespace VCEL.Benchmark;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class InBenchmarkTests
{
    private static readonly ObjectContext<object> context = new ();
    private readonly IExpression<object?> vcelExpressionFor10Items;
    private readonly IExpression<object?> vcelExpressionFor50Items;
    private readonly IExpression<object?> vcelExpressionFor100Items;
    private readonly IExpression<object?> csharpExpressionFor10Items;
    private readonly IExpression<object?> csharpExpressionFor50Items;
    private readonly IExpression<object?> csharpExpressionFor100Items;

    public InBenchmarkTests()
    {
        var vcelString10 = $"{{{string.Join(", ", Enumerable.Range(1, 10).Select(i => $"'ABCDE{i}'"))}}}";
        var vcelString50 = $"{{{string.Join(", ", Enumerable.Range(1, 50).Select(i => $"'ABCDE{i}'"))}}}";
        var vcelString100 = $"{{{string.Join(", ", Enumerable.Range(1, 100).Select(i => $"'ABCDE{i}'"))}}}";
        vcelExpressionFor10Items = VCExpression.ParseDefault(vcelString10).Expression;
        csharpExpressionFor10Items = CSharpExpression.ParseDelegate(vcelString10).Expression;
        vcelExpressionFor50Items = VCExpression.ParseDefault(vcelString50).Expression;
        csharpExpressionFor50Items = CSharpExpression.ParseDelegate(vcelString50).Expression;
        vcelExpressionFor100Items = VCExpression.ParseDefault(vcelString100).Expression;
        csharpExpressionFor100Items = CSharpExpression.ParseDelegate(vcelString100).Expression;
    }
        
    [Benchmark]
    public void VcelCsharp10()
    {
        csharpExpressionFor10Items.Evaluate(context);
    }
    
    [Benchmark]
    public void VcelCsharp50()
    {
        csharpExpressionFor50Items.Evaluate(context);
    }
    
    [Benchmark]
    public void VcelCsharp100()
    {
        csharpExpressionFor100Items.Evaluate(context);
    }
    
    [Benchmark]
    public void VcelCore10()
    {
        vcelExpressionFor10Items.Evaluate(context);
    }
    
    [Benchmark]
    public void VcelCore50()
    {
        vcelExpressionFor50Items.Evaluate(context);
    }
    
    [Benchmark]
    public void VcelCore100()
    {
        vcelExpressionFor100Items.Evaluate(context);
    }
}
