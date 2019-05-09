using System;
using BenchmarkDotNet.Attributes;

namespace MatrixMultiplication.AleaGPU
{
    public class AleaMatrixMultiplicationBenchmark
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
                A[oneDIndex] = B[oneDIndex] =  1.0f;
            }
        }

        [Benchmark]
        public void NormalMultiply()
        {
            AleaMatrixMultiplication
                .Multiply(A,B,C,MatrixSize);
        }

        [Benchmark]
        public void KernelMultiply()
        {
            AleaMatrixMultiplication
                .MultiplyWithKernel(A,B,C,MatrixSize);
        }

        [Benchmark]
        public void KernelMultiplySec()
        {
            AleaMatrixMultiplication
                .MultiplyWithKernelSec(A,B,C,MatrixSize);
        }
    }
}
