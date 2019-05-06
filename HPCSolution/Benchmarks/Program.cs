using System;
using System.Linq;
using MatrixMultiplication.NormalWrapper;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 2_500;
            var nPow = n * n;

            var a = new float[nPow];
            var b = new float[nPow];
            var c = new float[nPow];

            for (int i = 0; i < nPow; i += n)
            {
                a[i] = 1.0f;
                b[i] = 1.0f;
            }

            MatrixMultiplicationWrapper.ParallelMultiply(
                    a,
                    b,
                    c,
                    n
                );


            //var summary = BenchmarkRunner.Run<MatrixMultiplicationBenchmark>();

            Console.ReadKey();
        }
    }
}
