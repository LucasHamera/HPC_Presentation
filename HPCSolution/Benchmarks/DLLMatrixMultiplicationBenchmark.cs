using BenchmarkDotNet.Attributes;
using System;
using MatrixMultiplication.NormalWrapper;

namespace Benchmarks
{
    #region Result  

    //|                             Method | MatrixSize |               Mean |             Error |            StdDev |
    //|----------------------------------- |----------- |-------------------:|------------------:|------------------:|
    //|                         Multiply1d |         10 |           927.8 ns |         18.502 ns |         39.428 ns |
    //|            Multiply1dWithTranspose |         10 |           784.0 ns |         39.191 ns |         45.132 ns |
    //| Multiply1dWithTransposeAndUnrolled |         10 |           920.8 ns |          8.588 ns |          7.613 ns |
    //|                         Multiply1d |        100 |     1,106,293.4 ns |     25,756.857 ns |     24,092.980 ns |
    //|            Multiply1dWithTranspose |        100 |     1,067,908.6 ns |     23,810.122 ns |     23,384.723 ns |
    //| Multiply1dWithTransposeAndUnrolled |        100 |       505,217.9 ns |      9,605.974 ns |      9,434.351 ns |
    //|                         Multiply1d |        250 |    21,124,597.9 ns |    362,630.218 ns |    339,204.531 ns |
    //|            Multiply1dWithTranspose |        250 |    21,164,627.5 ns |    412,088.527 ns |    490,562.077 ns |
    //| Multiply1dWithTransposeAndUnrolled |        250 |     8,153,082.9 ns |    162,787.636 ns |    353,886.924 ns |
    //|                         Multiply1d |        500 |   187,482,817.5 ns |  3,647,841.770 ns |  4,342,496.138 ns |
    //|            Multiply1dWithTranspose |        500 |   177,669,922.2 ns |  2,748,149.020 ns |  2,570,620.298 ns |
    //| Multiply1dWithTransposeAndUnrolled |        500 |    62,159,197.0 ns |  1,174,053.246 ns |  1,098,210.135 ns |
    //|                         Multiply1d |       1000 | 1,699,312,982.4 ns | 43,911,336.268 ns | 45,093,709.391 ns |
    //|            Multiply1dWithTranspose |       1000 | 1,472,661,540.0 ns | 14,936,858.625 ns | 13,971,946.825 ns |
    //| Multiply1dWithTransposeAndUnrolled |       1000 |   536,861,173.8 ns | 10,678,023.137 ns | 19,525,369.097 ns |

    #endregion
    public class DLLMatrixMultiplicationBenchmark
    {
        [Params(
            10,
            100, 
            250,
            500,
            1000
        )]
        public int MatrixSize { get; set; }

        public float[] A { get; private set; }
        public float[] B { get; private set; }
        public float[] C { get; private set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var getMatrixIndex = new Func<int, int, long>((row, col) => row * MatrixSize + col);

            var matrixSizePow = MatrixSize * MatrixSize;
            A = new float[matrixSizePow];
            B = new float[matrixSizePow];
            C = new float[matrixSizePow];

            for (int i = 0; i < MatrixSize; i++)
            {
                var oneDIndex = getMatrixIndex(i, i);
                A[oneDIndex] = B[oneDIndex] = C[oneDIndex] = 1.0f;
            }
        }

        [Benchmark]
        public void Multiply1d()
        {
            MatrixMultiplicationWrapper
                .Multiply1d(
                        A,
                        B,
                        C,
                        MatrixSize
                    );
        }

        [Benchmark]
        public void Multiply1dWithTranspose()
        {
            MatrixMultiplicationWrapper
                .Multiply1dWithTranspose(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dWithTransposeAndUnrolled()
        {
            MatrixMultiplicationWrapper
                .Multiply1dWithTransposeAndUnrolled(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }
    }
}
