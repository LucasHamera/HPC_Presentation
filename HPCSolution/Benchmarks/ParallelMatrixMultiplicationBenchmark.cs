using System;
using BenchmarkDotNet.Attributes;
using MatrixMultiplication.NormalSharp;
using MatrixMultiplication.NormalWrapper;

namespace Benchmarks
{
    #region Result

    //|                                                Method | MatrixSize |             Mean |          Error |         StdDev |           Median |
    //|------------------------------------------------------ |----------- |-----------------:|---------------:|---------------:|-----------------:|
    //|                                       Multiply1dSharp |         10 |         5.574 us |      0.0377 us |      0.0352 us |         5.554 us |
    //|    Multiply1dWithTransposeAndUnrolledAndParallelSharp |         10 |         5.820 us |      0.1144 us |      0.1272 us |         5.774 us |
    //|                                 Multiply1dDLLFirstFor |         10 |         3.060 us |      0.0598 us |      0.0948 us |         3.052 us |
    //|                                Multiply1dDLLSecondFor |         10 |        34.132 us |      0.6811 us |      1.4366 us |        33.929 us |
    //|                                 Multiply1dDLLThirdFor |         10 |       341.777 us |      6.8053 us |     10.1859 us |       339.846 us |
    //|      Multiply1dWithTransposeAndUnrolledAndParallelDLL |         10 |         3.337 us |      0.0667 us |      0.1838 us |
    //|                                       Multiply1dSharp |        100 |       466.950 us |      9.1219 us |      8.5327 us |       466.327 us |
    //|    Multiply1dWithTransposeAndUnrolledAndParallelSharp |        100 |       452.535 us |      8.9537 us |     19.2738 us |       447.901 us |
    //|                                 Multiply1dDLLFirstFor |        100 |       213.945 us |      4.9133 us |     13.6962 us |       209.672 us |
    //|                                Multiply1dDLLSecondFor |        100 |       496.614 us |     17.0064 us |     47.6879 us |       481.255 us |
    //|                                 Multiply1dDLLThirdFor |        100 |    34,115.243 us |    674.7220 us |  1,466.7902 us |    34,069.340 us |
    //|      Multiply1dWithTransposeAndUnrolledAndParallelDLL |        100 |       161.152 us |      3.1758 us |      5.3928 us |
    //|                                       Multiply1dSharp |        250 |     6,107.169 us |    121.2515 us |    199.2196 us |     6,088.179 us |
    //|    Multiply1dWithTransposeAndUnrolledAndParallelSharp |        250 |     5,679.701 us |    106.9903 us |    100.0788 us |     5,644.938 us |
    //|                                 Multiply1dDLLFirstFor |        250 |     3,608.784 us |    119.0656 us |    341.6215 us |     3,516.375 us |
    //|                                Multiply1dDLLSecondFor |        250 |     4,113.059 us |     81.2145 us |    160.3095 us |     4,085.471 us |
    //|                                 Multiply1dDLLThirdFor |        250 |   230,850.575 us |  7,159.0100 us | 20,308.9197 us |   227,076.133 us |
    //|      Multiply1dWithTransposeAndUnrolledAndParallelDLL |        250 |     2,579.178 us |    125.2060 us |    369.1726 us |
    //|                                       Multiply1dSharp |        500 |    52,213.442 us |    956.1917 us |    894.4223 us |    51,962.300 us |
    //|    Multiply1dWithTransposeAndUnrolledAndParallelSharp |        500 |    43,081.550 us |    416.9136 us |    389.9812 us |    43,112.642 us |
    //|                                 Multiply1dDLLFirstFor |        500 |    31,504.486 us |    673.7689 us |  1,878.1984 us |    31,006.378 us |
    //|                                Multiply1dDLLSecondFor |        500 |    31,892.802 us |    642.2580 us |  1,725.3845 us |    31,841.487 us |
    //|                                 Multiply1dDLLThirdFor |        500 |   923,522.940 us | 18,484.3687 us | 40,573.6374 us |   912,753.650 us |
    //|      Multiply1dWithTransposeAndUnrolledAndParallelDLL |        500 |    17,061.960 us |    371.5151 us |  1,089.5894 us |
    //|                                       Multiply1dSharp |       1000 | 2,301,483.296 us | 45,097.4565 us | 57,033.8429 us | 2,308,407.400 us |
    //|    Multiply1dWithTransposeAndUnrolledAndParallelSharp |       1000 |   327,605.847 us |  4,459.0213 us |  4,170.9713 us |   327,616.400 us |
    //|                                 Multiply1dDLLFirstFor |       1000 |   775,414.908 us | 21,458.8900 us | 63,272.0213 us |   770,391.650 us |
    //|                                Multiply1dDLLSecondFor |       1000 | 1,020,197.316 us | 26,687.6341 us | 77,425.6656 us | 1,006,458.800 us |
    //|                                 Multiply1dDLLThirdFor |       1000 | 4,218,862.421 us | 79,396.2535 us | 70,382.7107 us | 4,236,403.450 us |
    //|      Multiply1dWithTransposeAndUnrolledAndParallelDLL |       1000 |   115,897.949 us |  2,299.4082 us |  4,949.7187 us |

#endregion

    public class ParallelMatrixMultiplicationBenchmark
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
        public void Multiply1dSharp()
        {
            NormalMatrixMultiplication
                .Multiply1dParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dWithTransposeAndUnrolledAndParallelSharp()
        {
            NormalMatrixMultiplication
                .Multiply1dWithTransposeAndUnrolledAndParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dDLLFirstForDLL()
        {
            MatrixMultiplicationWrapper
                .Multiply1dParallelFirstFor(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dDLLSecondForDLL()
        {
            MatrixMultiplicationWrapper
                .Multiply1dParallelSecondFor(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dDLLThirdForDLL()
        {
            MatrixMultiplicationWrapper
                .Multiply1dParallelThirdFor(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1dWithTransposeAndUnrolledAndParallelDLL()
        {
            MatrixMultiplicationWrapper
                .Multiply1dWithTransposeAndUnrolledAndParallel(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }
    }
}
