#pragma once
#include "Multiplication.cuh"

#define DLLIMPORT __declspec(dllimport)   

extern "C" {
	int DLLIMPORT multipy_1d_r(
		float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT multipy_1d_diff_dim_r(
		float  *a,
		float  *b,
		float  *c,
		const int m,
		const int n, 
		const int k
	);

	int DLLIMPORT alocate_1d_r(
		float** a, 
		const int matrix_size
	);

	int DLLIMPORT free_1d_r(
		float* a
	);

	int DLLIMPORT set_identity_1d_r(
		float* a, 
		const int matrix_size
	);

	int DLLIMPORT multipy_1d_only_gpu_r(
		float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT multipy_1d_diff_dim_only_gpu_r(
		float  *a,
		float  *b,
		float  *c,
		const int m,
		const int n, 
		const int k
	);
}