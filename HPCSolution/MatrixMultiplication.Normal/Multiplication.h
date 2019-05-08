#pragma once

void multipy_1d(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void multipy_1d_with_transpose(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void multipy_1d_with_transpose_and_unrolled(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_f_for_multipy_1d(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_s_for_multipy_1d(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_t_for_multipy_1d(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_multipy_1d_with_transpose(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);

void parallel_multipy_1d_with_transpose_and_unrolled(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
);
