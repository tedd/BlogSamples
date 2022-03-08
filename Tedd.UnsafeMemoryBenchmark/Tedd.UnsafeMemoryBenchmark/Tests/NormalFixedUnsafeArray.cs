using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Microsoft.Diagnostics.Runtime.Interop;

using Tedd.RandomUtils;

namespace Tedd.UnsafeMemoryBenchmark.Tests;

public class NormalFixedUnsafeArray
{
    //[Params(1_000)]
    //[Params(1_000, 1_000_000, 100_000_000)]
    [Params(1_000_000)]
    public int ArraySize = 1;
    public int[] Array = new int[1];
    public static FastRandom Random = new FastRandom(1);
    public long Sum = 0;
    public readonly int Iterations = 10_000_000;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Console.WriteLine($"// GlobalSetup: ArraySize: {ArraySize}");
        Array = new int[ArraySize];
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        // Disposing logic
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Console.WriteLine($"// IterationSetup: ArraySize: {ArraySize}");
        if (Array.Length != ArraySize)
            throw new Exception("Array setup not correct.");
        Random = new FastRandom(1);
        Sum = 0;

        for (int i = 0; i < Array.Length; i++)
            Array[i] = i;
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Console.WriteLine($"// IterationCleanup: Sum: {Sum}");
    }

    [Benchmark(Description = "Normal", Baseline = true)]
    public void NormalSum()
    {
        for (int i = 0; i < Iterations; i++)
            Sum += Array[Random.Next(ArraySize)];
    }

    [Benchmark(Description = "Fixed")]
    public unsafe void FixedSum()
    {
        fixed (int* p = &Array[0])
        {
            for (int i = 0; i < Iterations; i++)
                Sum += *(p + Random.Next(ArraySize));
        }
    }


    [Benchmark(Description = "Unsafe")]
    public unsafe void UnsafeSum()
    {
        var p = Unsafe.AsPointer(ref Array[0]);
        for (int i = 0; i < Iterations; i++)
            Sum += Unsafe.Read<int>(Unsafe.Add<int>(p, Random.Next(ArraySize)));
    }

}