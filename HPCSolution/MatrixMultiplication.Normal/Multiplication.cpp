#include "Multiplication.h"
#include "MatrixFunction.h"
#include <omp.h>

void multipy_1d(
	const float* a,
	float* b,
	float* c, 
	const int matrix_size
)
{
	for (int i = 0; i < matrix_size; i++)
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(k, j, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}
}

void multipy_1d_with_transpose(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
	transpose(
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

	transpose(
		b,
		matrix_size
	);
}

void multipy_1d_with_transpose_and_unrolled(
	const float* a, 
	float* b, 
	float* c,
	const int matrix_size
)
{
	transpose(
		b,
		matrix_size
	);

	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			auto tmp = 0.0f;

			int k = 0;
			while (k + 3 < matrix_size)
			{
				auto s = 0.0f;

				const auto aMatrixIndex = get_matrix_index(i, k, matrix_size);
				const auto bMatrixIndex = get_matrix_index(j, k, matrix_size);

				s += a[aMatrixIndex] * b[bMatrixIndex];
				s += a[aMatrixIndex + 1] * b[bMatrixIndex + 1];
				s += a[aMatrixIndex + 2] * b[bMatrixIndex + 2];
				s += a[aMatrixIndex + 3] * b[bMatrixIndex + 3];
				//                    s += a[aMatrixIndex + 4] * b[bMatrixIndex + 4];
				//                    s += a[aMatrixIndex + 5] * b[bMatrixIndex + 5];
				//                    s += a[aMatrixIndex + 6] * b[bMatrixIndex + 6];
				//                    s += a[aMatrixIndex + 7] * b[bMatrixIndex + 7];

				tmp += s;
				k += 4;
			}

			for (; k < matrix_size; ++k)
				tmp += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(j, k, matrix_size)];

			c[get_matrix_index(i, j, matrix_size)] = tmp;
		}

	transpose(
		b,
		matrix_size
	);
}

void parallel_f_for_multipy_1d(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
#pragma omp parallel for
	for (int i = 0; i < matrix_size; i++)
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(k, j, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}
}

void parallel_s_for_multipy_1d(
	const float* a,
	float* b, 
	float* c,
	const int matrix_size
)
{
	for (int i = 0; i < matrix_size; i++)
#pragma omp parallel for
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(k, j, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}
}

void parallel_t_for_multipy_1d(
	const float* a,
	float* b, 
	float* c,
	const int matrix_size
)
{
	for (int i = 0; i < matrix_size; i++)
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
#pragma omp parallel for
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(k, j, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}
}

void parallel_multipy_1d_with_transpose(
	const float* a, 
	float* b, 
	float* c, 
	const int matrix_size
)
{
	transpose(
		b,
		matrix_size
	);

#pragma omp parallel for
	for (int i = 0; i < matrix_size; i++)
		for (int j = 0; j < matrix_size; j++)
		{
			float line_result = 0.0f;
			for (int k = 0; k < matrix_size; k++)
				line_result += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(j, k, matrix_size)];
			c[get_matrix_index(i, j, matrix_size)] = line_result;
		}

	transpose(
		b,
		matrix_size
	);
}

void parallel_multipy_1d_with_transpose_and_unrolled(
	const float* a, 
	float* b, 
	float* c,
	const int matrix_size
)
{
	transpose(
		b,
		matrix_size
	);

#pragma omp parallel for
	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			auto tmp = 0.0f;

			int k = 0;
			while (k + 3 < matrix_size)
			{
				auto s = 0.0f;

				const auto aMatrixIndex = get_matrix_index(i, k, matrix_size);
				const auto bMatrixIndex = get_matrix_index(j, k, matrix_size);

				s += a[aMatrixIndex] * b[bMatrixIndex];
				s += a[aMatrixIndex + 1] * b[bMatrixIndex + 1];
				s += a[aMatrixIndex + 2] * b[bMatrixIndex + 2];
				s += a[aMatrixIndex + 3] * b[bMatrixIndex + 3];
				//                    s += a[aMatrixIndex + 4] * b[bMatrixIndex + 4];
				//                    s += a[aMatrixIndex + 5] * b[bMatrixIndex + 5];
				//                    s += a[aMatrixIndex + 6] * b[bMatrixIndex + 6];
				//                    s += a[aMatrixIndex + 7] * b[bMatrixIndex + 7];

				tmp += s;
				k += 4;
			}

			for (; k < matrix_size; ++k)
				tmp += a[get_matrix_index(i, k, matrix_size)] * b[get_matrix_index(j, k, matrix_size)];

			c[get_matrix_index(i, j, matrix_size)] = tmp;
		}

	transpose(
		b,
		matrix_size
	);
}
