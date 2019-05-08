using BenchmarkDotNet.Running;
using System;
using CacheBench;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BigVsSmallStruct>();

            Console.ReadKey();

            Console.ReadLine();
        }
    }
}