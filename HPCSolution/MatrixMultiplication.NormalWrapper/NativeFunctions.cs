using System.Runtime.InteropServices;

namespace MatrixMultiplication.NormalWrapper
{
    internal static class  NativeFunctions
    {
        private const string DLLName = "MatrixMultiplication.Normal.dll";

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_1d_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_1d_with_transpose_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_1d_with_transpose_and_unrolled_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_f_for_multipy_1d_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_s_for_multipy_1d_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_t_for_multipy_1d_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_multipy_1d_with_transpose_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_multipy_1d_with_transpose_and_unrolled_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );
    }
}
