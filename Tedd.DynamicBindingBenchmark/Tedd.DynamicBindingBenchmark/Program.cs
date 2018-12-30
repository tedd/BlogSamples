using System;
using BenchmarkDotNet.Running;
using Tedd.DynamicBindingBenchmark.Tests;

namespace Tedd.DynamicBindingBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary1 = BenchmarkRunner.Run<CallTests>();

            //var callTests = new CallTests();
            //callTests.Call_NormalDynamicMethod();
            //callTests.Call_BaseClass1_SelfNotOverrideAbstract();

            Console.ReadKey();
        }
    }
}
                                                                                                                                                                                                  