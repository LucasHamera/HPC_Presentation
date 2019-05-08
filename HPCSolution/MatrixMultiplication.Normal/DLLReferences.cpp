#include "DLLReferences.h"
#include "Multiplication.h"

int multipy_1d_r(
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
		multipy_1d(
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

int multipy_1d_with_transpose_r(
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
		multipy_1d_with_transpose(
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

int multipy_1d_with_transpose_and_unrolled_r(
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
		multipy_1d_with_transpose_and_unrolled(
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

int parallel_f_for_multipy_1d_r(
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
		parallel_f_for_multipy_1d(
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

int parallel_s_for_multipy_1d_r(
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
		parallel_s_for_multipy_1d(
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

int parallel_t_for_multipy_1d_r(
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
		parallel_t_for_multipy_1d(
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

int parallel_multipy_1d_with_transpose_r(
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
		parallel_multipy_1d_with_transpose(
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

int parallel_multipy_1d_with_transpose_and_unrolled_r(
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
		parallel_multipy_1d_with_transpose_and_unrolled(
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
