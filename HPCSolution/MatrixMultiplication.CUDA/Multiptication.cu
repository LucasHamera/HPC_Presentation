
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>
#include<cmath>

#include "Multiplication.cuh"

const int BLOCK_SIZE = 16;

__global__ void gpu_square_matrix_mult(float *d_a, float *d_b, float *d_result, int n) 
{
    __shared__ float tile_a[BLOCK_SIZE][BLOCK_SIZE];
    __shared__ float tile_b[BLOCK_SIZE][BLOCK_SIZE];

    int row = blockIdx.y * BLOCK_SIZE + threadIdx.y;
    int col = blockIdx.x * BLOCK_SIZE + threadIdx.x;
    float tmp = 0;
    int idx;

    for (int sub = 0; sub < gridDim.x; ++sub) 
    {
        idx = row * n + sub * BLOCK_SIZE + threadIdx.x;
        if(idx >= n*n)
        {
            // n may not divisible by BLOCK_SIZE
            tile_a[threadIdx.y][threadIdx.x] = 0;
        }
        else
        {
            tile_a[threadIdx.y][threadIdx.x] = d_a[idx];
        }

        idx = (sub * BLOCK_SIZE + threadIdx.y) * n + col;
        if(idx >= n*n)
        {
            tile_b[threadIdx.y][threadIdx.x] = 0;
        }  
        else
        {
            tile_b[threadIdx.y][threadIdx.x] = d_b[idx];
        }
        __syncthreads();

        for (int k = 0; k < BLOCK_SIZE; ++k) 
        {
            tmp += tile_a[threadIdx.y][k] * tile_b[k][threadIdx.x];
        }
        __syncthreads();
    }
    if(row < n && col < n)
    {
        d_result[row * n + col] = tmp;
    }
}


__global__ void gpu_matrix_mult(float *a,float *b, float *c, int m, int n, int k)
{ 
    int row = blockIdx.y * blockDim.y + threadIdx.y; 
    int col = blockIdx.x * blockDim.x + threadIdx.x;
    float sum = 0;
    if( col < k && row < m) 
    {
        for(int i = 0; i < n; i++) 
        {
            sum += a[row * n + i] * b[i * k + col];
        }
        c[row * k + col] = sum;
    }
} 

__global__ void set_identity_kernel(
	float *a, 
	int m,
	int n
)
{ 
    int row = blockIdx.y * blockDim.y + threadIdx.y; 
    int col = blockIdx.x * blockDim.x + threadIdx.x;
	
	    if( col < n && row < m) 
		{
			a[row * n + col] = (row == col) ? 1.0f: 0.0f;
		}
} 

	int multipy_1d(
		float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	)
	{    
		
		float *a_gpu;
		float *b_gpu;
		float *c_gpu;
		cudaError_t cudaStatus;

		const auto matrix_size_pow = matrix_size * matrix_size;
		const auto matrix_byte_size = matrix_size_pow * sizeof(float);

		cudaStatus = cudaMalloc((void**)&a_gpu, matrix_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		cudaStatus = cudaMalloc((void**)&b_gpu, matrix_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		cudaStatus = cudaMalloc((void**)&c_gpu, matrix_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		    cudaStatus = cudaMemcpy(a_gpu, a, matrix_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

	    cudaStatus = cudaMemcpy(b_gpu, b, matrix_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

	    cudaStatus = cudaMemcpy(c_gpu, c, matrix_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

	    unsigned int grid_rows = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		unsigned int grid_cols = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		dim3 dimGrid(grid_cols, grid_rows);
		dim3 dimBlock(BLOCK_SIZE, BLOCK_SIZE);

		gpu_square_matrix_mult<<<dimGrid, dimBlock>>>(a_gpu, b_gpu, c_gpu, matrix_size); 

		// Check for any errors launching the kernel
		cudaStatus = cudaGetLastError();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
			goto Error;
		}
    
		// cudaDeviceSynchronize waits for the kernel to finish, and returns
		// any errors encountered during the launch.
		cudaStatus = cudaDeviceSynchronize();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
			goto Error;
		}

		// Copy output vector from GPU buffer to host memory.
		cudaStatus = cudaMemcpy(c, c_gpu, matrix_byte_size, cudaMemcpyDeviceToHost);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMemcpy failed!");
			goto Error;
		}


	
		Error:
		cudaFree(a_gpu);
		cudaFree(b_gpu);
		cudaFree(c_gpu);
    
		return static_cast<int>(cudaStatus);
	}

	int multipy_1d_diff_dim(
		float * a, 
		float * b, 
		float * c, 
		const int m,
		const int n,
		const int k
	)
	{
		float *a_gpu;
		float *b_gpu;
		float *c_gpu;
		cudaError_t cudaStatus;

		const int a_gpu_byte_size = sizeof(float)*m*n;
		cudaStatus = cudaMalloc((void**)&a_gpu, a_gpu_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		const int b_gpu_byte_size = sizeof(int)*n*k;
		cudaStatus = cudaMalloc((void**)&b_gpu, b_gpu_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		const int c_gpu_byte_size = sizeof(int)*n*k;
		cudaStatus = cudaMalloc((void**)&c_gpu,  sizeof(int)*m*k);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		    cudaStatus = cudaMemcpy(a_gpu, a, a_gpu_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

	    cudaStatus = cudaMemcpy(b_gpu, b, b_gpu_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

	    cudaStatus = cudaMemcpy(c_gpu, c, c_gpu_byte_size, cudaMemcpyHostToDevice);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaMemcpy failed!");
        goto Error;
    }

		unsigned int grid_rows = (m + BLOCK_SIZE - 1) / BLOCK_SIZE;
		unsigned int grid_cols = (k + BLOCK_SIZE - 1) / BLOCK_SIZE;
		dim3 dimGrid(grid_cols, grid_rows);
		dim3 dimBlock(BLOCK_SIZE, BLOCK_SIZE);

		gpu_matrix_mult<<<dimGrid, dimBlock>>>(a_gpu, b_gpu, c_gpu, m, n, k); 

		// Check for any errors launching the kernel
		cudaStatus = cudaGetLastError();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
			goto Error;
		}
    
		// cudaDeviceSynchronize waits for the kernel to finish, and returns
		// any errors encountered during the launch.
		cudaStatus = cudaDeviceSynchronize();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
			goto Error;
		}

		// Copy output vector from GPU buffer to host memory.
		cudaStatus = cudaMemcpy(c, c_gpu, c_gpu_byte_size, cudaMemcpyDeviceToHost);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMemcpy failed!");
			goto Error;
		}
	
		Error:
		cudaFree(a_gpu);
		cudaFree(b_gpu);
		cudaFree(c_gpu);
    
		return static_cast<int>(cudaStatus);
	}

	int alocate_1d(
		float** a, 
		const int matrix_size
	)
	{
	
		cudaError_t cudaStatus;

		const auto matrix_size_pow = matrix_size * matrix_size;
		const auto matrix_byte_size = matrix_size_pow * sizeof(float);

		cudaStatus = cudaMalloc((void**)a, matrix_byte_size);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMalloc failed!");
			goto Error;
		}

		Error:
		cudaFree(a);

		return static_cast<int>(cudaStatus);
	}

	int free_1d(
		float * a
	)
	{
		cudaFree(a);
		return 0;
	}

	void set_identity_1d(
		float * a, 
		const int matrix_size
	)
	{
		cudaError_t cudaStatus;

	    unsigned int grid_rows = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		unsigned int grid_cols = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		dim3 dimGrid(grid_cols, grid_rows);
		dim3 dimBlock(BLOCK_SIZE, BLOCK_SIZE);

		set_identity_kernel<<<dimGrid, dimBlock>>>(a, matrix_size, matrix_size); 

		// Check for any errors launching the kernel
		cudaStatus = cudaGetLastError();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
			goto Error;
		}
    
		// cudaDeviceSynchronize waits for the kernel to finish, and returns
		// any errors encountered during the launch.
		cudaStatus = cudaDeviceSynchronize();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
			goto Error;
		}
	
		Error:
	}

	int multipy_1d_only_gpu(
		float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	)
	{
		cudaError_t cudaStatus;

		const auto matrix_size_pow = matrix_size * matrix_size;
		const auto matrix_byte_size = matrix_size_pow * sizeof(float);

	    unsigned int grid_rows = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		unsigned int grid_cols = (matrix_size + BLOCK_SIZE - 1) / BLOCK_SIZE;
		dim3 dimGrid(grid_cols, grid_rows);
		dim3 dimBlock(BLOCK_SIZE, BLOCK_SIZE);

		gpu_square_matrix_mult<<<dimGrid, dimBlock>>>(a, b, c, matrix_size); 

		// Check for any errors launching the kernel
		cudaStatus = cudaGetLastError();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
			goto Error;
		}
    
		// cudaDeviceSynchronize waits for the kernel to finish, and returns
		// any errors encountered during the launch.
		cudaStatus = cudaDeviceSynchronize();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
			goto Error;
		}

		Error:
		return static_cast<int>(cudaStatus);
	}

	int multipy_1d_diff_dim_only_gpu(
		float  *a,
		float  *b,
		float  *c,
		const int m,
		const int n, 
		const int k
	)
	{
		cudaError_t cudaStatus;

		unsigned int grid_rows = (m + BLOCK_SIZE - 1) / BLOCK_SIZE;
		unsigned int grid_cols = (k + BLOCK_SIZE - 1) / BLOCK_SIZE;
		dim3 dimGrid(grid_cols, grid_rows);
		dim3 dimBlock(BLOCK_SIZE, BLOCK_SIZE);

		gpu_matrix_mult<<<dimGrid, dimBlock>>>(a, b, c, m, n, k); 

		// Check for any errors launching the kernel
		cudaStatus = cudaGetLastError();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
			goto Error;
		}
    
		// cudaDeviceSynchronize waits for the kernel to finish, and returns
		// any errors encountered during the launch.
		cudaStatus = cudaDeviceSynchronize();
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
			goto Error;
		}
	
		Error:
		return static_cast<int>(cudaStatus);

	}
