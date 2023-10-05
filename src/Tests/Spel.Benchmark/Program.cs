using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace Spel.Benchmark;

class Program
{
    public static void Main(string[] args)
    {
        // new DefaultConfig()
        var config = new ManualConfig()
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.Default)
            .AddColumn(new RankColumn(NumeralSystem.Roman))
            .AddJob(Job
                .ShortRun
                .WithWarmupCount(1).WithIterationCount(1)
                .WithRuntime(CoreRuntime.Core60)
                .WithLaunchCount(1)
                .WithToolchain(InProcessNoEmitToolchain.Instance)
            )
            .AddJob(Job
                .ShortRun
                .WithWarmupCount(1).WithIterationCount(1)
                .WithRuntime(CoreRuntime.Core70)
                .WithLaunchCount(1)
                .WithToolchain(InProcessNoEmitToolchain.Instance)
            );

        _ = BenchmarkSwitcher
            .FromTypes(new[] { typeof(LogicalEvaluatorBenchmarks) })
            .Run(args);
    }
}
