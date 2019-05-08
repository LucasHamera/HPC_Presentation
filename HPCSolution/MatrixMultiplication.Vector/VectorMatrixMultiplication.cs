using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MatrixMultiplication.Vector
{
    public static class VectorMatrixMultiplication
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

            var vectorCount = System.Numerics.Vector<float>.Count;

            for (int i = 0; i < matrixSize; ++i)
            for (int j = 0; j < matrixSize; ++j)
            {
                float tmp = 0.0f;

                int k = 0;
                var sum = Vector<float>.One;
                while (k + (vectorCount - 1) < matrixSize)
                {
                    var x = new Vector<float>(firstMatrix,GetMatrixIndex(i, k, matrixSize));
                    var y = new Vector<float>(secondMatrix,GetMatrixIndex(j, k, matrixSize));
                    sum += x * y;

                    k += vectorCount;
                }

                tmp = System.Numerics.Vector.Dot(sum, Vector<float>.One);

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
            
            var vectorCount = System.Numerics.Vector<float>.Count;

            Parallel.For(
                0,
                matrixSize,
                i =>
                {
                    for (int j = 0; j < matrixSize; ++j)
                    {
                        float tmp = 0.0f;

                        int k = 0;
                        var sum = Vector<float>.One;
                        while (k + (vectorCount - 1) < matrixSize)
                        {
                            var x = new Vector<float>(firstMatrix, GetMatrixIndex(i, k, matrixSize));
                            var y = new Vector<float>(secondMatrix, GetMatrixIndex(j, k, matrixSize));
                            sum += x * y;

                            k += vectorCount;
                        }

                        tmp = System.Numerics.Vector.Dot(sum, Vector<float>.One);

                        for (; k < matrixSize; ++k)
                            tmp += firstMatrix[GetMatrixIndex(i, k, matrixSize)] *
                                   secondMatrix[GetMatrixIndex(j, k, matrixSize)];
                        outMatrix[GetMatrixIndex(i, j, matrixSize)] = tmp;
                    }
                }
            );
            
            Transpose1d(secondMatrix, matrixSize);
        }
    }
}
