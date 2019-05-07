using System;

namespace MatrixMultiplication.NormalWrapper
{
    public static class MatrixMultiplicationWrapper
    {
        public static unsafe void Multiply1d(
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
                    .multipy_1d_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithTranspose(
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
                    .multipy_1d_with_transpose_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithTransposeAndUnrolled(
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
                    .multipy_1d_with_transpose_and_unrolled_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dParallelFirstFor(
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
                    .parallel_f_for_multipy_1d_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dParallelSecondFor(
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
                    .parallel_s_for_multipy_1d_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dParallelThirdFor(
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
                    .parallel_t_for_multipy_1d_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithTransposeAndParallel(
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
                    .parallel_multipy_1d_with_transpose_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        matrixSize
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static unsafe void Multiply1dWithTransposeAndUnrolledAndParallel(
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
                    .parallel_multipy_1d_with_transpose_and_unrolled_r(
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
