using BenchmarkDotNet.Running;

namespace Spel.Benchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(new[]
            {
                // BenchmarkConverter.TypeToBenchmarks(typeof(ParserBenchmarks)),
                BenchmarkConverter.TypeToBenchmarks(typeof(ArithmeticEvaluatorBenchmarks)),
                // BenchmarkConverter.TypeToBenchmarks(typeof(LogicalEvaluatorBenchmarks)),
                // BenchmarkConverter.TypeToBenchmarks(typeof(BasicEvaluatorBenchmarks)),
                // BenchmarkConverter.TypeToBenchmarks(typeof(NestedTernaryEvaluatorBenchmarks))
            });
        }
    }
}
