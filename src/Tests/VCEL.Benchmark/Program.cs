using BenchmarkDotNet.Running;

namespace VcelBenchmark;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
    }
}