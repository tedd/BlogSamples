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

namespace Tedd.DynamicBindingBenchmark
{
    public class TestConfig1 : ManualConfig
    {
        public TestConfig1()
        {
            Add(TargetMethodColumn.Method, StatisticColumn.OperationsPerSecond, StatisticColumn.Iterations, StatisticColumn.Min, StatisticColumn.Max);

            Add(Job.ShortRun
                .WithLaunchCount(10)
                .WithIterationCount(10000)

                .With(Platform.AnyCpu)
                .With(Jit.RyuJit)
                .With(Runtime.Clr)
                .WithGcForce(true)
                .WithId("Clr-X64-RyuJit-Clr"));

        }
    }
}
