using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;

namespace Tedd.UnsafeMemoryBenchmark;
public class TestConfig1 : ManualConfig
{
    public TestConfig1()
    {
        AddColumn(TargetMethodColumn.Method, BaselineColumn.Default, RankColumn.Arabic, BaselineRatioColumn.RatioStdDev, BaselineRatioColumn.RatioStdDev);
        AddColumn(StatisticColumn.Iterations, StatisticColumn.Min, StatisticColumn.Max, StatisticColumn.Mean, StatisticColumn.Median, StatisticColumn.StdDev, StatisticColumn.StdErr, StatisticColumn.Skewness, StatisticColumn.Error,  StatisticColumn.OperationsPerSecond, StatisticColumn.Kurtosis);
        AddExporter(RPlotExporter.Default, HtmlExporter.Default, AsciiDocExporter.Default, MarkdownExporter.GitHub);
        AddDiagnoser(new InliningDiagnoser(), MemoryDiagnoser.Default, new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(printSource: true, printInstructionAddresses: true, exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true)));
        WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared));
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());

        SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);

        var job = Job.ShortRun
                .WithLaunchCount(1)
                .WithIterationCount(1000)
                .WithWarmupCount(1000)

                .WithPlatform(Platform.X64)
                .WithJit(Jit.RyuJit)
                .WithRuntime(CoreRuntime.Core60)
                .WithGcForce(true)
                .WithOutlierMode(Perfolizer.Mathematics.OutlierDetection.OutlierMode.RemoveAll)
                .WithEvaluateOverhead(true)
                

                .WithId("Clr-X64-RyuJit-Core60");



        AddJob(job);

    }
}
