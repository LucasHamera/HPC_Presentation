using System;
using System.Runtime.InteropServices;

namespace MatrixMultiplication.CUDA.Wrapper
{
    internal static class  NativeFunctions
    {
        private const string DLLName = "MatrixMultiplication.CUDA.dll";

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_1d_r(
            float* a,
            float* b,
            float* c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_1d_diff_dim_r(
            float* a,
            float* b,
            float* c,
            int m,
            int n, 
            int k
        );
        
        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern int alocate_1d_r(
            out IntPtr a,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern int free_1d_r(
            IntPtr a
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern int set_identity_1d_r(
            IntPtr a,
            int matrix_size
        );
        
        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern int multipy_1d_only_gpu_r(
            IntPtr a,
            IntPtr b,
            IntPtr c,
            int matrix_size
        );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern int multipy_1d_diff_dim_only_gpu_r(
            IntPtr a,
            IntPtr b,
            IntPtr c,
            int m,
            int n, 
            int k
        );
    }
}
