using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixMultiplication.NormalSharp
{
    public class NormalMatrixMultiplication
    {
        public static void Multiply1d(float[] a, float[] b, float[] c, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i * n + k] * b[k * n + j];
                c[i * n + j] = tmp;
            }
        }

        public static void Multiply1dWithTranspose(float[] a, float[] b, float[] c, int n)
        {
            Transpose1d(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i * n + k] * b[j * n + k];
                c[i * n + j] = tmp;
            }

            Transpose1d(b, n);
        }

        public static void Multiply1dWithTransposeAndUnrolled(float[] a, float[] b, float[] c, int n)
        {
            Transpose1d(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;

                int k = 0;
                while (k + 3 < n)
                {
                    var s = 0.0f;

                    s += a[i * n + k + 0] * b[j * n + k + 0];
                    s += a[i * n + k + 1] * b[j * n + k + 1];
                    s += a[i * n + k + 2] * b[j * n + k + 2];
                    s += a[i * n + k + 3] * b[j * n + k + 3];
//                    s += a[i * n + k + 4] * b[j * n + k + 4];
//                    s += a[i * n + k + 5] * b[j * n + k + 5];
//                    s += a[i * n + k + 6] * b[j * n + k + 6];
//                    s += a[i * n + k + 7] * b[j * n + k + 7];

                    tmp += s;
                    k += 4;
                }

                for (; k < n; ++k)
                    tmp += a[i * n + k] * b[j * n + k];

                c[i * n + j] = tmp;
            }

            Transpose1d(b, n);
        }

        public static void Multiply2d(float[,] a, float[,] b, float[,] c, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i, k] * b[k, j];
                c[i, j] = tmp;
            }
        }

        public static void Multiply2dWithTranspose(float[,] a, float[,] b, float[,] c, int n)
        {
            Transpose2d(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i, k] * b[j, k];
                c[i, j] = tmp;
            }

            Transpose2d(b, n);
        }

        public static void Multiply2dWithTransposeAndUnrolled(float[,] a, float[,] b, float[,] c, int n)
        {
            Transpose2d(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;

                int k = 0;
                while (k + 3 < n)
                {
                    var s = 0.0f;
                    s += a[i, k + 0] * b[j, k + 0];
                    s += a[i, k + 1] * b[j, k + 1];
                    s += a[i, k + 2] * b[j, k + 2];
                    s += a[i, k + 3] * b[j, k + 3];
                    tmp += s;
                    k += 4;
                }

                for (; k < n; ++k)
                    tmp += a[i, k] * b[j, k];
                c[i, j] = tmp;
            }

            Transpose2d(b, n);
        }

        public static void MultiplyJagged(float[][] a, float[][] b, float[][] c, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i][k] * b[k][j];
                c[i][j] = tmp;
            }
        }

        public static void MultiplyJaggedWithTranspose(float[][] a, float[][] b, float[][] c, int n)
        {
            TransposeJagged(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;
                for (int k = 0; k < n; ++k)
                    tmp += a[i][k] * b[j][k];
                c[i][j] = tmp;
            }

            TransposeJagged(b, n);
        }

        public static void MultiplyJaggedWithTransposeAndUnrolled(float[][] a, float[][] b, float[][] c, int n)
        {
            TransposeJagged(b, n);

            for (int i = 0; i < n; ++i)
            for (int j = 0; j < n; ++j)
            {
                var tmp = 0.0f;

                int k = 0;
                while (k + 3 < n)
                {
                    var s = 0.0f;
                    s += a[i][k + 0] * b[j][k + 0];
                    s += a[i][k + 1] * b[j][k + 1];
                    s += a[i][k + 2] * b[j][k + 2];
                    s += a[i][k + 3] * b[j][k + 3];
                    tmp += s;
                    k += 4;
                }

                for (; k < n; ++k)
                    tmp += a[i][k] * b[j][k];
                c[i][j] = tmp;
            }

            TransposeJagged(b, n);
        }

        public static void Transpose1d(float[] a, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; i < n; ++i)
            {
                var tmp = a[i * n + j];
                a[i * n + j] = a[j * n + i];
                a[j * n + i] = tmp;
            }
        }

        public static void Transpose2d(float[,] a, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; i < n; ++i)
            {
                var tmp = a[i, j];
                a[i, j] = a[j, i];
                a[j, i] = tmp;
            }
        }

        public static void TransposeJagged(float[][] a, int n)
        {
            for (int i = 0; i < n; ++i)
            for (int j = 0; i < n; ++i)
            {
                var tmp = a[i][j];
                a[i][j] = a[j][i];
                a[j][i] = tmp;
            }
        }
    }
}