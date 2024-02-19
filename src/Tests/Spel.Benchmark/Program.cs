using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace Spel.Benchmark;

class Program
{
    public static void Main(string[] args)
    {
        var config = ManualConfig
            .CreateEmpty()
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.Default)
            .AddLogger(new ConsoleLogger())
            .AddColumn(new RankColumn(NumeralSystem.Roman))
            .AddJob(Job.ShortRun
                .AsBaseline()
                .WithRuntime(CoreRuntime.Core60)
                // .WithToolchain(InProcessNoEmitToolchain.Instance)
                .WithWarmupCount(3)
                .WithIterationCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(200))
            )
            .AddJob(Job.ShortRun
                .WithRuntime(CoreRuntime.Core80)
                // .WithToolchain(InProcessNoEmitToolchain.Instance)
                .WithWarmupCount(3)
                .WithIterationCount(3)
                .WithIterationTime(TimeInterval.FromMilliseconds(200))
            );

        _ = BenchmarkSwitcher
            .FromTypes(new[] { typeof(LogicalEvaluatorBenchmarks) })
            .Run(args, config);
    }
}
