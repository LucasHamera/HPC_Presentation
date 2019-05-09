using System.Linq;
using Alea;
using Alea.CSharp;
using Alea.Parallel;

namespace MatrixMultiplication.AleaGPU
{
    public class AleaMatrixMultiplication
    {

        private static void MatrixMultiplyKernel(float[] a,float[] b, float[] c, int m, int n, int k)
        { 
            int row = blockIdx.y * blockDim.y + threadIdx.y; 
            int col = blockIdx.x * blockDim.x + threadIdx.x;
            float sum = 0.0f;
            if( col < k && row < m) 
            {
                for(int i = 0; i < n; i++) 
                {
                    sum += a[row * n + i] * b[i * k + col];
                }
                c[row * k + col] = sum;
            }
        }

        private const int BlockSize = 16;

        private static void MatrixMultiplyKernelSec(float[] d_a, float[] d_b, float[] d_result, int n)
        {

            var tile_a = __shared__.Array2D<float>(BlockSize, BlockSize);
            var tile_b = __shared__.Array2D<float>(BlockSize, BlockSize);

            int row = blockIdx.y * BlockSize + threadIdx.y;
            int col = blockIdx.x * BlockSize + threadIdx.x;
            float tmp = 0;
            int idx;

            for (int sub = 0; sub < gridDim.x; ++sub) 
            {
                idx = row * n + sub * BlockSize + threadIdx.x;
                if(idx >= n*n)
                {
                    tile_a[threadIdx.y,threadIdx.x] = 0;
                }
                else
                {
                    tile_a[threadIdx.y,threadIdx.x] = d_a[idx];
                }

                idx = (sub * BlockSize + threadIdx.y) * n + col;
                if(idx >= n*n)
                {
                    tile_b[threadIdx.y,threadIdx.x] = 0;
                }  
                else
                {
                    tile_b[threadIdx.y,threadIdx.x] = d_b[idx];
                }
                Intrinsic.__syncthreads();

                for (int k = 0; k < BlockSize; ++k) 
                {
                    tmp += tile_a[threadIdx.y,k] * tile_b[k,threadIdx.x];
                }
                Intrinsic.__syncthreads();
            }
            if(row < n && col < n)
            {
                d_result[row * n + col] = tmp;
            }
        }

        [GpuManaged]
        public static void MultiplyWithKernel(
            float[] a,
            float[] b,
            float[] c,
            int n
        )
        {
            var gpu = Gpu.Default;

            int gridRows = (n + BlockSize - 1) / BlockSize;
            int gridCols = (n + BlockSize - 1) / BlockSize;
            dim3 dimGrid = new dim3(gridCols, gridRows);
            dim3 dimBlock = new dim3(BlockSize, BlockSize);

            var lp = new LaunchParam(dimGrid, dimBlock);

            gpu.Launch(
                MatrixMultiplyKernel,
                lp,
                a,
                b,
                c,
                n,
                n,
                n
            );
        }

        [GpuManaged]
        public static void MultiplyWithKernelSec(
            float[] a,
            float[] b,
            float[] c,
            int n
        )
        {
            var gpu = Gpu.Default;

            var grid_rows = (n + BlockSize - 1) / BlockSize;
            var grid_cols = (n + BlockSize - 1) / BlockSize;
            var dimGrid = new dim3(grid_cols, grid_rows);
            var dimBlock = new dim3(BlockSize, BlockSize);

            var lp = new LaunchParam(dimGrid, dimBlock);

            gpu.Launch(
                MatrixMultiplyKernelSec,
                lp,
                a,
                b,
                c,
                n
            );
        }


        public static void Multiply(
                float[] a,
                float[] b,
                float[] c, 
                int n
            )
        {
            Gpu gpu = Gpu.Default;

            
            gpu.For(
                0, 
                n,
                i =>
                {
                    for (var j = 0; j < n; ++j)
                    {
                        var tmp = 0.0f;
                        for (var k = 0; k < n; ++k)
                            tmp += a[i * n + k] * b[k * n + j];
                        c[i * n + j] = tmp;
                    }
                }
                );
        }


    }
}
