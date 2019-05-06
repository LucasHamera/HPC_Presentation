using System;

namespace MatrixMultiplication.NormalWrapper
{
    public static class MatrixMultiplicationWrapper
    {
        public static unsafe void Multiply(
            float[] firstMatrix,
            float[] secondMatrix,
            float[] outMatrix,
            int matrixSize
        )
        {
            fixed(float* firstMatrixPtr = firstMatrix)
            fixed(float* secondMatrixPtr = secondMatrix)
            fixed (float* outMatrixPtr = outMatrix)
            {
                var result = NativeFunctions.multipy_r(
                    firstMatrixPtr,
                    secondMatrixPtr,
                    outMatrixPtr,
                    matrixSize
                );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void ParallelMultiply(
            float[] firstMatrix,
            float[] secondMatrix,
            float[] outMatrix,
            int matrixSize
        )
        {
            fixed(float* firstMatrixPtr = firstMatrix)
            fixed(float* secondMatrixPtr = secondMatrix)
            fixed (float* outMatrixPtr = outMatrix)
            {
                var result = NativeFunctions.parallel_multipy_r(
                    firstMatrixPtr,
                    secondMatrixPtr,
                    outMatrixPtr,
                    matrixSize
                );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }
    }
}
