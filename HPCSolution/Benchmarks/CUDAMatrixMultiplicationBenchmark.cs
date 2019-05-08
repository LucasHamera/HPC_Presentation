using BenchmarkDotNet.Attributes;
using MatrixMultiplication.CUDA.Wrapper;
using MatrixMultiplication.SIMDWrapper;
using System;

namespace Benchmarks
{

    #region Result4

    //|                        Method | MatrixSize |          Mean |         Error |        StdDev |        Median |
    //|------------------------------ |----------- |--------------:|--------------:|--------------:|--------------:|
    //|                OpenMPParallel |         10 |      3.680 us |     0.1112 us |     0.3082 us |      3.655 us |
    //|             CUDAFirstMultiply |         10 |    329.753 us |    12.5671 us |    36.8570 us |    322.609 us |
    //|            CUDASecondMultiply |         10 |    307.212 us |     9.2096 us |    25.9758 us |    295.513 us |
    //|  CUDAFirstMultiplyWithoutCopy |         10 |     72.128 us |     3.2209 us |     9.4968 us |     70.660 us |
    //| CUDASecondMultiplyWithoutCopy |         10 |     70.879 us |     3.9748 us |    11.7199 us |     63.806 us |
    //|                OpenMPParallel |        100 |     90.710 us |     1.7972 us |     3.5475 us |     90.368 us |
    //|             CUDAFirstMultiply |        100 |    418.775 us |    16.2783 us |    47.7414 us |    405.020 us |
    //|            CUDASecondMultiply |        100 |    380.426 us |     7.5718 us |    19.9472 us |    372.659 us |
    //|  CUDAFirstMultiplyWithoutCopy |        100 |    101.229 us |     4.7851 us |    14.1090 us |     93.281 us |
    //| CUDASecondMultiplyWithoutCopy |        100 |     91.069 us |     4.0229 us |    11.8617 us |     84.786 us |
    //|                OpenMPParallel |        250 |    977.838 us |    36.1275 us |   105.9556 us |    940.499 us |
    //|             CUDAFirstMultiply |        250 |  1,587.013 us |    32.2718 us |    37.1642 us |  1,575.508 us |
    //|            CUDASecondMultiply |        250 |  1,494.406 us |    17.6265 us |    16.4878 us |  1,494.408 us |
    //|  CUDAFirstMultiplyWithoutCopy |        250 |    466.271 us |     9.3062 us |    19.2189 us |    464.370 us |
    //| CUDASecondMultiplyWithoutCopy |        250 |    278.101 us |     5.5423 us |     8.1238 us |    276.070 us |
    //|                OpenMPParallel |        500 |  6,909.159 us |   138.3472 us |   405.7483 us |  6,831.614 us |
    //|             CUDAFirstMultiply |        500 |  6,329.022 us |    41.3730 us |    34.5483 us |  6,325.756 us |
    //|            CUDASecondMultiply |        500 |  4,885.780 us |    97.2554 us |   213.4780 us |  4,794.176 us |
    //|  CUDAFirstMultiplyWithoutCopy |        500 |  2,982.110 us |    13.5538 us |    12.6783 us |  2,977.831 us |
    //| CUDASecondMultiplyWithoutCopy |        500 |  1,617.573 us |     5.3356 us |     4.4555 us |  1,616.600 us |
    //|                OpenMPParallel |       1000 | 50,522.673 us | 1,702.6377 us | 5,020.2657 us | 50,222.830 us |
    //|             CUDAFirstMultiply |       1000 | 32,335.120 us |   159.2015 us |   141.1280 us | 32,367.822 us |
    //|            CUDASecondMultiply |       1000 | 18,479.251 us |   173.4195 us |   162.2167 us | 18,427.102 us |
    //|  CUDAFirstMultiplyWithoutCopy |       1000 | 25,541.902 us |    30.1455 us |    28.1981 us | 25,530.244 us |
    //| CUDASecondMultiplyWithoutCopy |       1000 | 11,579.214 us |    19.2134 us |    17.9722 us | 11,570.898 us |
    

#endregion
    public class CUDAMatrixMultiplicationBenchmark
    {
        [Params(
            10_000
        )]
        public int MatrixSize { get; set; }

        public float[] A { get; private set; }
        public float[] B { get; private set; }
        public float[] C { get; private set; }

        public IntPtr A_CUDA { get; private set; }
        public IntPtr B_CUDA { get; private set; }
        public IntPtr C_CUDA { get; private set; }

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

            A_CUDA = CUDAMatrixMultiplicationWrapper
                .AllockMatrix(
                    MatrixSize
                );

            B_CUDA = CUDAMatrixMultiplicationWrapper
                .AllockMatrix(
                    MatrixSize
                );

            C_CUDA = CUDAMatrixMultiplicationWrapper
                .AllockMatrix(
                    MatrixSize
                );
        }



        [Benchmark]
        public void OpenMPParallel()
        {
            SIMDMatrixMultiplicationWrapper
                .Multiply1dWithOpenMPParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void CUDAFirstMultiply()
        {
            CUDAMatrixMultiplicationWrapper
                .Multiply1d(
                    A,
                    B,
                    C,
                    MatrixSize,
                    MatrixSize,
                    MatrixSize
                );
        }

        [Benchmark]
        public void CUDASecondMultiply()
        {
            CUDAMatrixMultiplicationWrapper
                .Multiply1d(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        
        [Benchmark]
        public void CUDAFirstMultiplyWithoutCopy()
        {
            CUDAMatrixMultiplicationWrapper
                .Multiply1dWithoutCopy(
                    A_CUDA,
                    B_CUDA,
                    C_CUDA,
                    MatrixSize,
                    MatrixSize,
                    MatrixSize
                );
        }

        [Benchmark]
        public void CUDASecondMultiplyWithoutCopy()
        {
            CUDAMatrixMultiplicationWrapper
                .Multiply1dWithoutCopy(
                    A_CUDA,
                    B_CUDA,
                    C_CUDA,
                    MatrixSize
                );
        }
    }
}
