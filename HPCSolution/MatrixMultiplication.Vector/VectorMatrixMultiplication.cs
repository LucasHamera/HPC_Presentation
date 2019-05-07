using System.Numerics;
using System.Runtime.CompilerServices;

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
        public static void Multiply(
                float[] firstMatrix,
                float[] secondMatrix,
                float[] outMatrix,
                int matrixSize
            )
        {
            var vecSize = Vector<float>.Count;

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    var lineResult = Vector<float>.One;
                    int k = 0;
                    for (; k < matrixSize; k+=vecSize)
                    { 
                        var firstVector = new Vector<float>(firstMatrix,GetMatrixIndex(i, k, matrixSize));
                        var secVector = new Vector<float>(secondMatrix,GetMatrixIndex(j, k, matrixSize));
                        lineResult += firstVector * secVector;
                    }

                    var lineResultOne = System.Numerics.Vector.Dot(lineResult, Vector<float>.One);
                    for (; k < matrixSize; k ++)
                        lineResultOne 
                            += firstMatrix[GetMatrixIndex(i, k, matrixSize)]
                               * secondMatrix[GetMatrixIndex(j, k, matrixSize)];

                    outMatrix[GetMatrixIndex(i, j, matrixSize)] = lineResultOne;
                }
            }
           
        }
    }
}
