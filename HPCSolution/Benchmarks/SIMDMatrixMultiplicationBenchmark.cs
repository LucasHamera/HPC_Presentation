using System;
using BenchmarkDotNet.Attributes;
using MatrixMultiplication.SIMDWrapper;
using MatrixMultiplication.Vector;

namespace Benchmarks
{
    #region Result

    //|              Method | MatrixSize |             Mean |            Error |           StdDev |           Median |
    //|-------------------- |----------- |-----------------:|-----------------:|-----------------:|-----------------:|
    //|         VectorSharp |         10 |         859.6 ns |         17.82 ns |         48.19 ns |         841.5 ns |
    //|              SSEDLL |         10 |         760.6 ns |         16.36 ns |         47.99 ns |         739.4 ns |
    //|             AVX2DLL |         10 |       1,066.4 ns |         20.38 ns |         19.06 ns |       1,064.3 ns |
    //| VectorSharpParallel |         10 |       5,928.1 ns |        118.30 ns |        264.59 ns |       5,927.2 ns |
    //|      SSEDLLParallel |         10 |       4,688.5 ns |        245.78 ns |        716.95 ns |       4,470.3 ns |
    //|     AVX2DLLParallel |         10 |       4,598.6 ns |        153.94 ns |        439.21 ns |       4,483.4 ns |
    //|      OpenMPParallel |         10 |       4,753.0 ns |        230.86 ns |        673.43 ns |       4,564.4 ns |
    //|         VectorSharp |        100 |     306,459.5 ns |      4,581.71 ns |      4,285.73 ns |     307,082.8 ns |
    //|              SSEDLL |        100 |     298,696.2 ns |      7,449.95 ns |      6,604.19 ns |     297,540.3 ns |
    //|             AVX2DLL |        100 |     321,984.0 ns |      6,409.43 ns |     17,218.51 ns |     319,892.0 ns |
    //| VectorSharpParallel |        100 |     167,643.7 ns |      3,271.49 ns |      4,995.91 ns |     166,668.1 ns |
    //|      SSEDLLParallel |        100 |     126,772.0 ns |      2,509.86 ns |      7,037.92 ns |     124,273.2 ns |
    //|     AVX2DLLParallel |        100 |     106,172.8 ns |      2,171.39 ns |      5,075.54 ns |     104,630.5 ns |
    //|      OpenMPParallel |        100 |     109,900.0 ns |      3,086.67 ns |      9,101.11 ns |     107,838.6 ns |
    //|         VectorSharp |        250 |   4,023,050.1 ns |     79,925.04 ns |    186,822.19 ns |   4,019,589.5 ns |
    //|              SSEDLL |        250 |   5,111,324.8 ns |    100,197.98 ns |    126,718.36 ns |   5,087,850.8 ns |
    //|             AVX2DLL |        250 |   3,906,101.1 ns |     84,536.97 ns |    109,921.94 ns |   3,881,548.8 ns |
    //| VectorSharpParallel |        250 |   1,451,027.8 ns |     28,882.37 ns |     33,260.98 ns |   1,446,179.5 ns |
    //|      SSEDLLParallel |        250 |   1,667,390.5 ns |     40,668.73 ns |    119,912.68 ns |   1,695,794.0 ns |
    //|     AVX2DLLParallel |        250 |   1,123,495.4 ns |     30,583.10 ns |     90,174.95 ns |   1,122,519.6 ns |
    //|      OpenMPParallel |        250 |   1,159,277.4 ns |     23,560.63 ns |     69,469.05 ns |   1,155,276.4 ns |
    //|         VectorSharp |        500 |  35,725,655.7 ns |  1,153,303.57 ns |  3,252,913.18 ns |  34,896,415.4 ns |
    //|              SSEDLL |        500 |  47,456,814.7 ns |  1,361,329.47 ns |  3,971,061.95 ns |  46,350,327.3 ns |
    //|             AVX2DLL |        500 |  32,822,269.0 ns |    837,154.37 ns |  2,374,867.62 ns |  32,281,453.1 ns |
    //| VectorSharpParallel |        500 |  10,103,308.4 ns |    170,658.22 ns |    159,633.81 ns |  10,064,903.1 ns |
    //|      SSEDLLParallel |        500 |  10,096,233.3 ns |    271,021.00 ns |    794,857.67 ns |   9,969,381.2 ns |
    //|     AVX2DLLParallel |        500 |   6,897,739.6 ns |    136,968.70 ns |    381,814.00 ns |   6,955,718.8 ns |
    //|      OpenMPParallel |        500 |   7,095,176.5 ns |    139,841.84 ns |    335,052.13 ns |   7,083,608.2 ns |
    //|         VectorSharp |       1000 | 362,379,080.6 ns | 20,471,893.41 ns | 59,717,473.99 ns | 341,778,400.0 ns |
    //|              SSEDLL |       1000 | 433,512,084.6 ns | 11,430,990.30 ns |  9,545,394.61 ns | 434,127,500.0 ns |
    //|             AVX2DLL |       1000 | 304,208,639.9 ns |  6,017,696.31 ns | 16,675,008.13 ns | 298,987,200.0 ns |
    //| VectorSharpParallel |       1000 |  68,496,049.0 ns |  1,358,075.78 ns |  1,765,882.12 ns |  68,741,756.2 ns |
    //|      SSEDLLParallel |       1000 |  70,147,399.7 ns |  1,751,134.45 ns |  5,024,333.00 ns |  69,161,187.5 ns |
    //|     AVX2DLLParallel |       1000 |  46,857,365.1 ns |  1,694,517.53 ns |  4,861,888.46 ns |  45,709,766.7 ns |
    //|      OpenMPParallel |       1000 |  52,670,167.0 ns |  2,528,108.42 ns |  7,454,184.73 ns |  51,503,822.2 ns |

#endregion

    public class SIMDMatrixMultiplicationBenchmark
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
        public void VectorSharp()
        {
            VectorMatrixMultiplication
                .Multiply(
                        A,
                        B,
                        C,
                        MatrixSize
                    );
        }

        [Benchmark]
        public void SSEDLL()
        {
            SIMDMatrixMultiplicationWrapper
                .Multiply1dWithVectorizedSSE(
                        A,
                        B,
                        C,
                        MatrixSize
                    );
        }

        [Benchmark]
        public void AVX2DLL()
        {
            SIMDMatrixMultiplicationWrapper
                .Multiply1dWithVectorizedAVX2(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void VectorSharpParallel()
        {
            VectorMatrixMultiplication
                .MultiplyParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void SSEDLLParallel()
        {
            SIMDMatrixMultiplicationWrapper
                .Multiply1dWithVectorizedSSEParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void AVX2DLLParallel()
        {
            SIMDMatrixMultiplicationWrapper
                .Multiply1dWithVectorizedAVX2Parallel(
                    A,
                    B,
                    C,
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
    }
}
