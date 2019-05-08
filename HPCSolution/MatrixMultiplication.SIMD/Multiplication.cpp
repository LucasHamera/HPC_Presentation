#include "Multiplication.h"

void vectorized_avx2(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
	transpose(b, matrix_size);

	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			int k = 0;
			__m256 sum = _mm256_set1_ps(0.0f);
			while (k + 7 < matrix_size)
			{
				const auto x = _mm256_loadu_ps(&a[i * matrix_size + k]);
				const auto y = _mm256_loadu_ps(&b[j * matrix_size + k]);
				sum = _mm256_add_ps(sum, _mm256_mul_ps(x, y));

				k += 8;
			}

			float v[8];
			_mm256_storeu_ps(&v[0], sum);
			float tmp = v[0] + v[1] + v[2] + v[3] + v[4] + v[5] + v[6] + v[7];
			for (; k < matrix_size; ++k)
				tmp += a[i * matrix_size + k] * b[j * matrix_size + k];
			c[i * matrix_size + j] = tmp;
		}

	transpose(b, matrix_size);
}

void vectorized_sse(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
	transpose(b, matrix_size);

	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			float tmp = 0.0f;

			int k = 0;
			__m128 sum = _mm_set1_ps(0.0f);
			while (k + 3 < matrix_size)
			{
				const auto x = _mm_loadu_ps(&a[i * matrix_size + k]);
				const auto y = _mm_loadu_ps(&b[j * matrix_size + k]);
				sum = _mm_add_ps(sum, _mm_mul_ps(x, y));

				k += 4;
			}

			float v[4];
			_mm_storeu_ps(&v[0], sum);
			tmp = v[0] + v[1] + v[2] + v[3];

			for (; k < matrix_size; ++k)
				tmp += a[i * matrix_size + k] * b[j * matrix_size + k];
			c[i * matrix_size + j] = tmp;
		}

	transpose(b, matrix_size);
}

void parallel_vectorized_avx2(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
	transpose(b, matrix_size);

#pragma omp parallel for
	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			int k = 0;
			__m256 sum = _mm256_set1_ps(0.0f);
			while (k + 7 < matrix_size)
			{
				const auto x = _mm256_loadu_ps(&a[i * matrix_size + k]);
				const auto y = _mm256_loadu_ps(&b[j * matrix_size + k]);
				sum = _mm256_add_ps(sum, _mm256_mul_ps(x, y));

				k += 8;
			}

			float v[8];
			_mm256_storeu_ps(&v[0], sum);
			float tmp = v[0] + v[1] + v[2] + v[3] + v[4] + v[5] + v[6] + v[7];
			for (; k < matrix_size; ++k)
				tmp += a[i * matrix_size + k] * b[j * matrix_size + k];
			c[i * matrix_size + j] = tmp;
		}

	transpose(b, matrix_size);
}

void parallel_vectorized_sse(
	const float* a, 
	float* b,
	float* c, 
	const int matrix_size
)
{
	transpose(b, matrix_size);

#pragma omp parallel for
	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			float tmp = 0.0f;

			int k = 0;
			__m128 sum = _mm_set1_ps(0.0f);
			while (k + 3 < matrix_size)
			{
				const auto x = _mm_loadu_ps(&a[i * matrix_size + k]);
				const auto y = _mm_loadu_ps(&b[j * matrix_size + k]);
				sum = _mm_add_ps(sum, _mm_mul_ps(x, y));

				k += 4;
			}

			float v[4];
			_mm_storeu_ps(&v[0], sum);
			tmp = v[0] + v[1] + v[2] + v[3];

			for (; k < matrix_size; ++k)
				tmp += a[i * matrix_size + k] * b[j * matrix_size + k];
			c[i * matrix_size + j] = tmp;
		}

	transpose(b, matrix_size);
}

void parallel_vectorized_omp(
	const float* a, 
	float* b,
	float* c,
	const int matrix_size
)
{
	transpose(b, matrix_size);

#pragma omp parallel for
	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < matrix_size; ++j)
		{
			float tmp = 0.0f;

#pragma omp simd
			for (int k = 0; k < matrix_size; ++k)
				tmp += a[i * matrix_size + k] * b[j * matrix_size + k];
			c[i * matrix_size + j] = tmp;
		}

	transpose(b, matrix_size);
}
