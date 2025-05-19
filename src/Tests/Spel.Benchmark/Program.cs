using BenchmarkDotNet.Running;

namespace Spel.Benchmark;

class Program
{
    public static void Main(string[] args)
    {
        _ = BenchmarkSwitcher
            .FromTypes([
                typeof(LogicalEvaluatorBenchmarks), typeof(ArithmeticEvaluatorBenchmarks),
                typeof(BasicEvaluatorBenchmarks), typeof(NestedTernaryEvaluatorBenchmarks), typeof(ParserBenchmarks),
            ])
            .Run(args);
    }
}