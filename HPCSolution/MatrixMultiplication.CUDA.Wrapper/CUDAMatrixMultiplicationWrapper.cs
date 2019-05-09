using System;

namespace MatrixMultiplication.CUDA.Wrapper
{
    public class CUDAMatrixMultiplicationWrapper
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

        public static unsafe void Multiply1d(
            float[] firstMatrix,
            float[] secondMatrix,
            float[] outMatrix,
            int m,
            int n, 
            int k
        )
        {
            fixed(float* firstMatrixPtr = firstMatrix)
            fixed(float* secondMatrixPtr = secondMatrix)
            fixed (float* outMatrixPtr = outMatrix)
            {
                var result = NativeFunctions
                    .multipy_1d_diff_dim_r(
                        firstMatrixPtr,
                        secondMatrixPtr,
                        outMatrixPtr,
                        m,
                        n,
                        k
                    );

                if(result != 0)
                    throw new Exception("Cannot multiply");
            }
        }

        public static IntPtr AllockMatrix(
            int matrixSize
        )
        {
            var result = NativeFunctions.alocate_1d_r(
                out var matrixPtr,
                matrixSize
            );

            if(result != 0)
                throw new Exception("Cannot allocate");

            result = NativeFunctions
                .set_identity_1d_r(
                    matrixPtr,
                    matrixSize
                );

            if(result != 0)
                throw new Exception("Cannot allocate");

            return matrixPtr;
        }

        public static void Multiply1dWithoutCopy(
            IntPtr firstMatrix,
            IntPtr secondMatrix,
            IntPtr outMatrix,
            int matrixSize
        )
        {
            var result = NativeFunctions
                .multipy_1d_only_gpu_r(
                    firstMatrix,
                    secondMatrix,
                    outMatrix,
                    matrixSize
                );

            if(result != 0)
                throw new Exception("Cannot multiply");
        }

        public static void Multiply1dWithoutCopy(
            IntPtr firstMatrix,
            IntPtr secondMatrix,
            IntPtr outMatrix,
            int m,
            int n, 
            int k
        )
        {
            var result = NativeFunctions
                .multipy_1d_diff_dim_only_gpu_r(
                    firstMatrix,
                    secondMatrix,
                    outMatrix,
                    m,
                    n,
                    k
                );

            if(result != 0)
                throw new Exception("Cannot multiply");
        }
    }
}
