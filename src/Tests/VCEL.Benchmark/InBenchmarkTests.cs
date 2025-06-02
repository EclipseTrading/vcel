using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;

namespace VCEL.Benchmark;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class InBenchmarkTests
{
    private readonly IExpression<object?> vcelExpressionFor10Items;
    private readonly IExpression<object?> vcelExpressionFor50Items;
    private readonly IExpression<object?> csharpExpressionFor10Items;
    private readonly IExpression<object?> csharpExpressionFor50Items;
    private readonly IExpression<object?> csharpNextGenExpressionFor10Items;
    private readonly IExpression<object?> csharpNextGenExpressionFor50Items;
    record Row(string a);

    private static readonly object testRow = new { a = 1 };
    public InBenchmarkTests()
    {
        var vcelString10 = $"a in {{{string.Join(", ", Enumerable.Range(1, 10).Select(i => $"'ABCDE{i}'"))}}}";
        var vcelString50 = $"a in {{{string.Join(", ", Enumerable.Range(1, 50).Select(i => $"'ABCDE{i}'"))}}}";
        vcelExpressionFor10Items = VCExpression.ParseDefault(vcelString10).Expression;
        csharpExpressionFor10Items = CSharpExpression.ParseMethod(vcelString10).Expression;
        csharpNextGenExpressionFor10Items = CSharpExpression.ParseMethodWithMembers(vcelString10).Expression;
        vcelExpressionFor50Items = VCExpression.ParseDefault(vcelString50).Expression;
        csharpExpressionFor50Items = CSharpExpression.ParseMethod(vcelString50).Expression;
        csharpNextGenExpressionFor50Items = CSharpExpression.ParseMethodWithMembers(vcelString50).Expression;
    }
        
    [Benchmark]
    public void VcelCsharp10()
    {
        csharpExpressionFor10Items.Evaluate(testRow);
    }
    
    [Benchmark]
    public void VcelCsharp50()
    {
        csharpExpressionFor50Items.Evaluate(testRow);
    }
        
    [Benchmark]
    public void VcelCsharpNextGen10()
    {
        csharpNextGenExpressionFor10Items.Evaluate(testRow);
    }
    
    [Benchmark]
    public void VcelCsharpNextGen50()
    {
        csharpNextGenExpressionFor50Items.Evaluate(testRow);
    }
    
    [Benchmark]
    public void VcelCore10()
    {
        vcelExpressionFor10Items.Evaluate(testRow);
    }
    
    [Benchmark]
    public void VcelCore50()
    {
        vcelExpressionFor50Items.Evaluate(testRow);
    }
}
