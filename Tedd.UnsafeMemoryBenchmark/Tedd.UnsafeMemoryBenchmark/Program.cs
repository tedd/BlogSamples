// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;

using Tedd.UnsafeMemoryBenchmark;
using Tedd.UnsafeMemoryBenchmark.Tests;

var summary1 = BenchmarkRunner.Run<NormalFixedUnsafeArray>(new TestConfig1());

if (System.Diagnostics.Debugger.IsAttached)
    Console.WriteLine("Done.");
