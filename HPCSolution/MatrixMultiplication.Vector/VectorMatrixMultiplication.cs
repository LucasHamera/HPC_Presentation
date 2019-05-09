using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MatrixMultiplication.Vector
{
    public class VectorMatrixMultiplication
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetMatrixIndex(
                int row,
                int col,
                int matrix_cols
            )
        {
            return row * matrix_cols + col;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transpose1d(float[] a, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = i + 1; j < n; ++j)
            {
                var tmp = a[i * n + j];
                a[i * n + j] = a[j * n + i];
                a[j * n + i] = tmp;
            }
        }

        public static void Multiply(
                float[] firstMatrix,
                float[] secondMatrix,
                float[] outMatrix,
                int matrixSize
            )
        {
            Transpose1d(secondMatrix, matrixSize);


            for (int i = 0; i < matrixSize; ++i)
            for (int j = 0; j < matrixSize; ++j)
            {
                float tmp = 0.0f;

                int k = 0;
                var sum = Vector4.Zero;
                while (k + 3 < matrixSize)
                {
                    var xIndex = GetMatrixIndex(i, k, matrixSize);
                    var yIndex = GetMatrixIndex(j, k, matrixSize);
                    var x = new Vector4(
                        firstMatrix[xIndex],
                        firstMatrix[xIndex + 1],
                        firstMatrix[xIndex + 2],
                        firstMatrix[xIndex + 3]
                        );

                    var y = new Vector4(                       
                        secondMatrix[yIndex],
                        secondMatrix[yIndex + 1],
                        secondMatrix[yIndex + 2],
                        secondMatrix[yIndex + 3]
                        );
                    sum += x * y;

                    k += 4;
                }

                tmp = System.Numerics.Vector4.Dot(sum, Vector4.One);

                for (; k < matrixSize; ++k)
                    tmp += firstMatrix[GetMatrixIndex(i, k, matrixSize)] * secondMatrix[GetMatrixIndex(j, k, matrixSize)];
                outMatrix[GetMatrixIndex(i, j, matrixSize)] = tmp;
            }
            
            Transpose1d(secondMatrix, matrixSize);
        }

        public static void MultiplyParallel(
            float[] firstMatrix,
            float[] secondMatrix,
            float[] outMatrix,
            int matrixSize
        )
        {
            Transpose1d(secondMatrix, matrixSize);
            
            var vectorCount = Vector<float>.Count;

            Parallel.For(
                0,
                matrixSize,
                i =>
                {
                    for (int j = 0; j < matrixSize; ++j)
                    {
                        float tmp = 0.0f;

                        int k = 0;
                        var sum = Vector4.Zero;
                        while (k + 3 < matrixSize)
                        {
                            var xIndex = GetMatrixIndex(i, k, matrixSize);
                            var yIndex = GetMatrixIndex(j, k, matrixSize);
                            var x = new Vector4(
                                firstMatrix[xIndex],
                                firstMatrix[xIndex + 1],
                                firstMatrix[xIndex + 2],
                                firstMatrix[xIndex + 3]
                            );

                            var y = new Vector4(                       
                                secondMatrix[yIndex],
                                secondMatrix[yIndex + 1],
                                secondMatrix[yIndex + 2],
                                secondMatrix[yIndex + 3]
                            );
                            sum += x * y;

                            k += 4;
                        }

                        tmp = System.Numerics.Vector4.Dot(sum, Vector4.One);

                        for (; k < matrixSize; ++k)
                            tmp += firstMatrix[GetMatrixIndex(i, k, matrixSize)] * secondMatrix[GetMatrixIndex(j, k, matrixSize)];
                        outMatrix[GetMatrixIndex(i, j, matrixSize)] = tmp;
                    }
                }
            );
            
            Transpose1d(secondMatrix, matrixSize);
        }
    }
}
