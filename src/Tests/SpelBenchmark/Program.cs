using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
