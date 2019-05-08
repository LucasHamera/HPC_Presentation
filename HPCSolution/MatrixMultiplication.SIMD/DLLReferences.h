#pragma once

#define DLLIMPORT __declspec(dllimport)   

extern "C" {

int DLLIMPORT vectorized_sse_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

int DLLIMPORT vectorized_avx2_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

int DLLIMPORT parallel_vectorized_sse_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

int DLLIMPORT parallel_vectorized_avx2_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

int DLLIMPORT parallel_vectorized_omp_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

}
