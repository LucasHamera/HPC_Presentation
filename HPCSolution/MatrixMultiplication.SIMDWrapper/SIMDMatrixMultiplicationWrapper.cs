using System;

namespace MatrixMultiplication.SIMDWrapper
{
    public static class SIMDMatrixMultiplicationWrapper
    {
        public static unsafe void Multiply1dWithVectorizedSSE(
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
                var result = NativeFunctions
                    .vectorized_sse_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithVectorizedAVX2(
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
                var result = NativeFunctions
                    .vectorized_avx2_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithVectorizedSSEParallel(
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
                var result = NativeFunctions
                    .parallel_vectorized_sse_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithVectorizedAVX2Parallel(
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
                var result = NativeFunctions
                    .parallel_vectorized_avx2_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithOpenMPParallel(
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
                var result = NativeFunctions
                    .parallel_vectorized_omp_r(
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
