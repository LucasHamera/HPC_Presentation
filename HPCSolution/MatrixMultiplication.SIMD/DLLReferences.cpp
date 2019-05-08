#include "DLLReferences.h"
#include "Multiplication.h"

int vectorized_sse_r(
	const float* a,
	float* b, 
	float* c, 
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (matrix_size <= 0)
		return 3;

	try
	{
		vectorized_sse(
			a,
			b,
			c,
			matrix_size
		);
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int vectorized_avx2_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (matrix_size <= 0)
		return 3;

	try
	{
		vectorized_avx2(
			a,
			b,
			c,
			matrix_size
		);
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int parallel_vectorized_sse_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (matrix_size <= 0)
		return 3;

	try
	{
		parallel_vectorized_sse(
			a,
			b,
			c,
			matrix_size
		);
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int parallel_vectorized_avx2_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (matrix_size <= 0)
		return 3;

	try
	{
		parallel_vectorized_avx2(
			a,
			b,
			c,
			matrix_size
		);
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int parallel_vectorized_omp_r(
	const float* a,
	float* b,
	float* c,
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (matrix_size <= 0)
		return 3;

	try
	{
		parallel_vectorized_omp(
			a,
			b,
			c,
			matrix_size
		);
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}
