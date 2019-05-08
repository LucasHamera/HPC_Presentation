#pragma once
#include <algorithm>
#include <iostream>
#include <immintrin.h>
#include <cassert>
#include <cmath>

void transpose(
	float* a,
	int n
);

void vectorized_avx2(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void vectorized_sse(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_vectorized_avx2(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_vectorized_sse(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_vectorized_omp(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);
