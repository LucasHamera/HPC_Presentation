using System;
using BenchmarkDotNet.Attributes;
using MatrixMultiplication.SIMDWrapper;
using MatrixMultiplication.Vector;

namespace Benchmarks
{
    #region Result

    //|              Method | MatrixSize |             Mean |            Error |           StdDev |           Median |
    //|-------------------- |----------- |-----------------:|-----------------:|-----------------:|-----------------:|
    //|         VectorSharp |         10 |         764.0 ns |        13.935 ns |        12.353 ns |         762.3 ns |
    //|              SSEDLL |         10 |         679.4 ns |         5.906 ns |         5.235 ns |         679.0 ns |
    //|             AVX2DLL |         10 |       1,004.2 ns |        20.026 ns |        18.732 ns |       1,002.7 ns |
    //| VectorSharpParallel |         10 |       5,454.7 ns |        60.860 ns |        56.929 ns |       5,436.4 ns |
    //|      SSEDLLParallel |         10 |       3,682.2 ns |        77.172 ns |       216.399 ns |       3,652.8 ns |
    //|     AVX2DLLParallel |         10 |       3,638.7 ns |        95.875 ns |       267.261 ns |       3,541.7 ns |
    //|      OpenMPParallel |         10 |       3,654.3 ns |        90.168 ns |       258.708 ns |       3,558.4 ns |
    //|         VectorSharp |        100 |     291,443.6 ns |     3,416.630 ns |     3,028.753 ns |     291,458.4 ns |
    //|              SSEDLL |        100 |     283,065.9 ns |     5,559.858 ns |     7,031.440 ns |     279,198.3 ns |
    //|             AVX2DLL |        100 |     308,838.0 ns |     6,140.071 ns |     9,559.351 ns |     307,669.5 ns |
    //| VectorSharpParallel |        100 |     169,712.9 ns |     3,317.233 ns |     5,632.913 ns |     168,574.4 ns |
    //|      SSEDLLParallel |        100 |     151,245.5 ns |     2,689.533 ns |     2,515.791 ns |     150,458.3 ns |
    //|     AVX2DLLParallel |        100 |     127,514.5 ns |     1,979.355 ns |     1,851.490 ns |     127,421.7 ns |
    //|      OpenMPParallel |        100 |     123,506.2 ns |     1,394.218 ns |     1,304.152 ns |     123,796.3 ns |
    //|         VectorSharp |        250 |   3,851,789.4 ns |    76,619.503 ns |    91,210.068 ns |   3,844,293.0 ns |
    //|              SSEDLL |        250 |   4,887,964.0 ns |    96,258.633 ns |   166,041.412 ns |   4,848,080.1 ns |
    //|             AVX2DLL |        250 |   3,709,390.1 ns |    74,124.676 ns |    76,120.585 ns |   3,702,773.4 ns |
    //| VectorSharpParallel |        250 |   1,440,626.7 ns |    28,320.755 ns |    41,512.195 ns |   1,431,901.2 ns |
    //|      SSEDLLParallel |        250 |   1,886,636.8 ns |    35,732.465 ns |    35,094.058 ns |   1,888,960.6 ns |
    //|     AVX2DLLParallel |        250 |   1,277,137.0 ns |    23,474.968 ns |    21,958.500 ns |   1,275,560.4 ns |
    //|      OpenMPParallel |        250 |   1,319,303.3 ns |    14,792.703 ns |    13,837.104 ns |   1,323,224.0 ns |
    //|         VectorSharp |        500 |  29,289,426.6 ns |   560,237.359 ns |   688,021.789 ns |  29,237,268.8 ns |
    //|              SSEDLL |        500 |  42,266,491.7 ns |   804,890.291 ns |   790,509.890 ns |  42,298,754.2 ns |
    //|             AVX2DLL |        500 |  28,751,974.3 ns |   557,920.990 ns |   572,943.780 ns |  28,723,850.0 ns |
    //| VectorSharpParallel |        500 |   9,869,295.8 ns |   196,257.981 ns |   248,203.507 ns |   9,851,246.9 ns |
    //|      SSEDLLParallel |        500 |  10,665,178.7 ns |   351,561.003 ns | 1,031,067.565 ns |  10,706,750.8 ns |
    //|     AVX2DLLParallel |        500 |   7,626,203.5 ns |   193,681.383 ns |   568,033.968 ns |   7,579,857.8 ns |
    //|      OpenMPParallel |        500 |   7,767,856.6 ns |   207,585.201 ns |   608,811.460 ns |   7,787,603.1 ns |
    //|         VectorSharp |       1000 | 264,526,276.2 ns | 5,273,331.322 ns | 6,277,525.821 ns | 261,993,500.0 ns |
    //|              SSEDLL |       1000 | 389,636,520.0 ns | 7,713,942.335 ns | 8,883,387.464 ns | 387,519,900.0 ns |
    //|             AVX2DLL |       1000 | 253,923,751.6 ns | 4,909,293.764 ns | 7,497,010.086 ns | 253,550,850.0 ns |
    //| VectorSharpParallel |       1000 |  67,268,778.1 ns | 1,297,879.272 ns | 1,214,037.076 ns |  67,314,828.6 ns |
    //|      SSEDLLParallel |       1000 |  65,991,051.7 ns | 1,309,212.260 ns | 1,607,830.230 ns |  65,553,300.0 ns |
    //|     AVX2DLLParallel |       1000 |  47,126,689.3 ns | 2,643,418.823 ns | 7,710,976.781 ns |  43,572,241.7 ns |
    //|      OpenMPParallel |       1000 |  42,849,954.5 ns |   599,573.745 ns |   468,107.742 ns |  42,699,295.5 ns |

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
