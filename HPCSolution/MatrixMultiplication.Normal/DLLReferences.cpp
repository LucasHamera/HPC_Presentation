#include "DLLReferences.h"
#include "Multiplication.h"

int multipy_r(
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
		multipy(
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

int parallel_multipy_r(
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
		parallel_multipy(
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
