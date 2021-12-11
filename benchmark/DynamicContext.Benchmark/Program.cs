using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DynamicContext.Benchmark;

var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOption(ConfigOptions.JoinSummary, true);

#if DEBUG
await ExecuteManually();
#else
var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
#endif

static async Task ExecuteManually()
{
    var bm = new Benchmarks();
    //bm.GetDynamicDbSet();
    //eh.GetEntities_Batched_NoTracking();
}
