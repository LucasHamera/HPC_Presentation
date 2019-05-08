#pragma once

#define DLLIMPORT __declspec(dllimport)   

extern "C" {
	int DLLIMPORT multipy_1d_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT multipy_1d_with_transpose_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT multipy_1d_with_transpose_and_unrolled_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_f_for_multipy_1d_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_s_for_multipy_1d_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_t_for_multipy_1d_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_multipy_1d_with_transpose_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_multipy_1d_with_transpose_and_unrolled_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);
}