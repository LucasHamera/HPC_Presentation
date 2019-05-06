#include "Multiplication.h"
#include "MatrixFunction.h"
#include <omp.h>

void multipy(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
)
{
	transpose_s(
		b,
		matrix_size
	);

	for (int i = 0; i < matrix_size; i++)
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(j, k, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}

	transpose_s(
		b,
		matrix_size
	);
}

void parallel_multipy(
	const float* a, 
	float* b, 
	float* c, 
	const int matrix_size
)
{

	transpose_s(
		b,
		matrix_size
	);

	const auto chunk_size = omp_get_max_threads() / matrix_size;

	for (int i = 0; i < matrix_size; i++)
#pragma omp parallel for
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(j, k, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}

	transpose_s(
		b,
		matrix_size
	);
}
