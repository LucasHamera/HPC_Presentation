using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using MatrixMultiplication.NormalSharp;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var n in new []{10, 100, 250, 500, 1000})
            {
                var random = new Random();

                var a = new float[n * n];
                var b = new float[n * n];
                var c = new float[n * n];
                for (int i = 0; i < n * n; i++)
                {
                    a[i] = (float)random.NextDouble();
                    b[i] = (float)random.NextDouble();
                }

                var ar = new float[n, n];
                var br = new float[n, n];
                var cr = new float[n, n];
                for (int i = 0; i < n; i++)
                for (int j = 0; i < n; i++)
                {
                    ar[i, j] = (float)random.NextDouble();
                    br[i, j] = (float)random.NextDouble();
                }

                var aj = new float[n][];
                var bj = new float[n][];
                var cj = new float[n][];
                for (int i = 0; i < n; i++)
                {
                    aj[i] = new float[n];
                    bj[i] = new float[n];
                    cj[i] = new float[n];
                    for (int j = 0; j < n; j++)
                    {
                        aj[i][j] = (float)random.NextDouble();
                        bj[i][j] = (float)random.NextDouble();
                    }
                }


                Console.WriteLine($"n={n} not transposed");
                var s1n = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1d(a, b, c, n);
                Console.WriteLine($"normal 1d: {s1n.Elapsed.TotalSeconds}s");

                var s2n = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply2d(ar, br, cr, n);
                Console.WriteLine($"normal 2d: {s2n.Elapsed.TotalSeconds}s");

                var s3n = Stopwatch.StartNew();
                NormalMatrixMultiplication.MultiplyJagged(aj, bj, cj, n);
                Console.WriteLine($"normal jagged: {s3n.Elapsed.TotalSeconds}s");

                var s4n = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1dParallel(a, b, c, n);
                Console.WriteLine($"normal 1d parallel: {s4n.Elapsed.TotalSeconds}s");

                Console.WriteLine();

                Console.WriteLine($"n={n} transposed");
                var s1 = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1dWithTranspose(a, b, c, n);
                Console.WriteLine($"normal 1d: {s1.Elapsed.TotalSeconds}s");

                var s2 = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply2dWithTranspose(ar, br, cr, n);
                Console.WriteLine($"normal 2d: {s2.Elapsed.TotalSeconds}s");

                var s3 = Stopwatch.StartNew();
                NormalMatrixMultiplication.MultiplyJaggedWithTranspose(aj, bj, cj, n);
                Console.WriteLine($"normal jagged: {s3.Elapsed.TotalSeconds}s");

                var s4 = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1dWithTransposeAndParallel(a, b, c, n);
                Console.WriteLine($"normal 1d parallel: {s4.Elapsed.TotalSeconds}s");


                Console.WriteLine();

                Console.WriteLine($"n={n} transposed + unrolled");
                var s1u = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1dWithTransposeAndUnrolled(a, b, c, n);
                Console.WriteLine($"normal 1d: {s1u.Elapsed.TotalSeconds}s");

                var s2u = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply2dWithTransposeAndUnrolled(ar, br, cr, n);
                Console.WriteLine($"normal 2d: {s2u.Elapsed.TotalSeconds}s");

                var s3u = Stopwatch.StartNew();
                NormalMatrixMultiplication.MultiplyJaggedWithTransposeAndUnrolled(aj, bj, cj, n);
                Console.WriteLine($"normal jagged: {s3u.Elapsed.TotalSeconds}s");
                Console.WriteLine();

                Console.WriteLine($"n={n} transposed + unrolled + parallel");

                var s5 = Stopwatch.StartNew();
                NormalMatrixMultiplication.Multiply1dWithTransposeAndUnrolledAndParallel(a, b, c, n);
                Console.WriteLine($"normal 1d: {s5.Elapsed.TotalSeconds}s");

                Console.WriteLine();
                Console.WriteLine();
            }


            //            var summary = BenchmarkRunner.Run<MatrixMultiplicationBenchmark>();

            Console.ReadKey();
        }
    }
}