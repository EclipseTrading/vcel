using BenchmarkDotNet.Running;
using VCEL;

namespace Spel.Benchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
