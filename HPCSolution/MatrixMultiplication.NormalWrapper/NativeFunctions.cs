using System.Runtime.InteropServices;

namespace MatrixMultiplication.NormalWrapper
{
    internal static class  NativeFunctions
    {
        private const string DLLName = "MatrixMultiplication.Normal.dll";
        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int multipy_r(
                float* a,
                float* b, 
                float* c, 
                int matrix_size
            );

        [DllImport(DLLName, CallingConvention = CallingConvention.Winapi)]
        public static extern unsafe int parallel_multipy_r(
            float* a,
            float* b, 
            float* c, 
            int matrix_size
        );
    }
}
