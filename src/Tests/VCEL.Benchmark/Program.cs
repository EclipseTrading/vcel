using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace VcelBenchmark;

class Program
{
    static void Main(string[] args)
    {
        var config = ManualConfig
            .CreateEmpty()
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.Default)
            .AddLogger(new ConsoleLogger())
            .AddColumn(new RankColumn(NumeralSystem.Roman))
            .AddJob(Job.ShortRun
                .WithWarmupCount(3)
                .WithIterationCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(200))
            );

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
    }
}