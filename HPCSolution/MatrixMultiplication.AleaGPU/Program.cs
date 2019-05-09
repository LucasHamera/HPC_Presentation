using System;
using Alea;
using BenchmarkDotNet.Running;

namespace MatrixMultiplication.AleaGPU
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<AleaMatrixMultiplicationBenchmark>();

            Console.ReadKey();

            Console.ReadLine();
        }
    }
}
