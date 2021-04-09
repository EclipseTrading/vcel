using BenchmarkDotNet.Running;

namespace SpelBenchmark
{
    class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run(new BenchmarkRunInfo[]
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(BasicEvaluatorBenchmarks)),
                BenchmarkConverter.TypeToBenchmarks(typeof(EvaluatorBenchmarks))
            });
        }
    }
}
