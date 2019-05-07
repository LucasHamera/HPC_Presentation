using BenchmarkDotNet.Attributes;
using System;
using MatrixMultiplication.NormalWrapper;

namespace Benchmarks
{
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
