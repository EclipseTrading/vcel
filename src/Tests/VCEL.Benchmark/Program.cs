using BenchmarkDotNet.Running;
using VCEL.Benchmark;

namespace VcelBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run(new BenchmarkRunInfo[]
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(InBenchmarkTests))
            });
        }
    }
}
