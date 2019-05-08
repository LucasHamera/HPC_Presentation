using System.Runtime.InteropServices;

namespace MatrixMultiplication.SIMDWrapper
{
    internal static class  NativeFunctions
    {
        private const string DLLName = "MatrixMultiplication.SIMD.dll";

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int vectorized_sse_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int vectorized_avx2_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_vectorized_sse_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_vectorized_avx2_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_vectorized_omp_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );
    }
}
