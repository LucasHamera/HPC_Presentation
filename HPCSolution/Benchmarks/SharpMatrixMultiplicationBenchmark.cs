using BenchmarkDotNet.Attributes;
using MatrixMultiplication.NormalSharp;
using System;

namespace Benchmarks
{
    #region Resul

    //|                             Method | MatrixSize |              Mean |           Error |          StdDev |
    //|----------------------------------- |----------- |------------------:|----------------:|----------------:|
    //|                MultiplyJaggedSharp |         10 |          1.531 us |       0.0306 us |       0.0618 us |
    //|                         Multiply2d |         10 |          2.477 us |       0.0494 us |       0.0607 us |
    //|                         Multiply1d |         10 |          1.331 us |       0.0265 us |       0.0498 us |
    //|            Multiply1dWithTranspose |         10 |          1.186 us |       0.0225 us |       0.0221 us |
    //| Multiply1dWithTransposeAndUnrolled |         10 |          1.286 us |       0.0273 us |       0.0373 us |
    //|                MultiplyJaggedSharp |        100 |      1,357.602 us |      23.2452 us |      21.7436 us |
    //|                         Multiply2d |        100 |      2,338.702 us |      24.3722 us |      21.6053 us |
    //|                         Multiply1d |        100 |      1,448.661 us |      28.6429 us |      53.0914 us |
    //|            Multiply1dWithTranspose |        100 |      1,383.894 us |      16.8266 us |      14.0510 us |
    //| Multiply1dWithTransposeAndUnrolled |        100 |      1,076.452 us |      12.0103 us |      11.2345 us |
    //|                MultiplyJaggedSharp |        250 |     23,476.325 us |     511.5211 us |     717.0811 us |
    //|                         Multiply2d |        250 |     37,923.643 us |     556.8028 us |     520.8337 us |
    //|                         Multiply1d |        250 |     22,007.590 us |     388.9259 us |     344.7727 us |
    //|            Multiply1dWithTranspose |        250 |     20,767.400 us |     411.7288 us |     364.9868 us |
    //| Multiply1dWithTransposeAndUnrolled |        250 |     16,644.401 us |     283.8997 us |     251.6696 us |
    //|                MultiplyJaggedSharp |        500 |    214,642.380 us |   4,247.8189 us |   4,891.7945 us |
    //|                         Multiply2d |        500 |    382,662.140 us |   7,518.8480 us |   8,658.7165 us |
    //|                         Multiply1d |        500 |    186,925.080 us |   3,728.2134 us |   8,261.4633 us |
    //|            Multiply1dWithTranspose |        500 |    168,821.071 us |   3,231.4848 us |   3,173.7502 us |
    //| Multiply1dWithTransposeAndUnrolled |        500 |    133,871.478 us |   1,386.7767 us |   1,297.1918 us |
    //|                MultiplyJaggedSharp |       1000 | 10,104,715.060 us | 175,000.9159 us | 163,695.9654 us |
    //|                         Multiply2d |       1000 |  3,966,262.178 us |  79,468.6357 us | 234,315.0653 us |
    //|                         Multiply1d |       1000 |  1,771,000.480 us |  23,732.8995 us |  22,199.7689 us |
    //|            Multiply1dWithTranspose |       1000 |  1,368,079.876 us |  43,202.2301 us |  44,365.5096 us |
    //| Multiply1dWithTransposeAndUnrolled |       1000 |  1,072,467.979 us |  18,017.4126 us |  15,971.9669 us |

    #endregion

    public class SharpMatrixMultiplicationBenchmark
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

        public float[,] A_TD { get; private set; }
        public float[,] B_TD { get; private set; }
        public float[,] C_TD { get; private set; }

        public float[][] A_JAGGED { get; private set; }
        public float[][] B_JAGGED  { get; private set; }
        public float[][] C_JAGGED { get; private set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var getMatrixIndex = new Func<int, int, long>((row, col) => row * MatrixSize + col);

            var matrixSizePow = MatrixSize * MatrixSize;
            A = new float[matrixSizePow];
            B = new float[matrixSizePow];
            C = new float[matrixSizePow];
            A_TD = new float[MatrixSize,MatrixSize];
            B_TD = new float[MatrixSize,MatrixSize];
            C_TD = new float[MatrixSize,MatrixSize];
            A_JAGGED = new float[MatrixSize][];
            B_JAGGED = new float[MatrixSize][];
            C_JAGGED = new float[MatrixSize][];

            for (int i = 0; i < MatrixSize; i++)
            {
                var oneDIndex = getMatrixIndex(i, i);
                A_JAGGED[i] = new float[MatrixSize];
                B_JAGGED[i] = new float[MatrixSize];
                C_JAGGED[i] = new float[MatrixSize];

                A[oneDIndex] = B[oneDIndex] = C[oneDIndex] = 1.0f;
                A_TD[i, i] = B_TD[i, i] = C_TD[i, i] = 1.0f;
                A_JAGGED[i][i] = B_JAGGED[i][i] = C_JAGGED[i][i] = 1.0f;
            }
        }

        [Benchmark]
        public void MultiplyJaggedSharp()
        {
            NormalMatrixMultiplication
                .MultiplyJagged(
                        A_JAGGED,
                        B_JAGGED,
                        C_JAGGED,
                        MatrixSize
                    );
        }

        [Benchmark]
        public void Multiply2d()
        {
            NormalMatrixMultiplication
                .Multiply2d(
                    A_TD,
                    B_TD,
                    C_TD,
                    MatrixSize
                );
        }

        [Benchmark]
        public void Multiply1d()
        {
            NormalMatrixMultiplication
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
            NormalMatrixMultiplication
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
            NormalMatrixMultiplication
                .Multiply1dWithTransposeAndUnrolled(
                    A,
                    B,
                    C,
                    MatrixSize
                );
        }
    }
}
