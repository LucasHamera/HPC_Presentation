#include "DLLReferences.cuh"

int multipy_1d_r(
		float  *a,
		float  *b,
		float  *c,
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
		const auto result = multipy_1d(
			a,
			b,
			c,
			matrix_size
		);

		if(result != 0)
			return result;
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int multipy_1d_diff_dim_r(
		float  *a,
		float  *b,
		float  *c,
		const int m,
		const int n, 
		const int k
	)
{
	if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (m <= 0)
		return 4;
		if (n <= 0)
		return 5;
		if (k <= 0)
		return 6;

	try
	{
		const auto result = multipy_1d_diff_dim(
			a,
			b,
			c,
			m,
			n,
			k
		);

		if(result != 0)
			return result;
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int alocate_1d_r(
	float** a, 
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (matrix_size <= 0)
		return 2;

	const auto result = alocate_1d(
		a,
		matrix_size
	);

	if(result != 0)
		return result;


	return 0;
}

int free_1d_r(
	float* a
)
{
	if (a == nullptr)
		return 1;

	const auto result = free_1d(
		a
	);

	if(result != 0)
		return result;


	return 0;
}

int set_identity_1d_r(
	float* a,
	const int matrix_size
)
{
	if (a == nullptr)
		return 1;
	if (matrix_size <= 0)
		return 2;

	try
	{
		set_identity_1d(
			a,
			matrix_size
		);
	}
	catch (...)
	{
		return 3;
	}

	return 0;
}

int multipy_1d_only_gpu_r(
	float * a, 
	float * b, 
	float * c, 
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
		const auto result = multipy_1d_only_gpu(
			a,
			b,
			c,
			matrix_size
		);

		if(result != 0)
			return result;
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}

int multipy_1d_diff_dim_only_gpu_r(
	float * a,
	float * b,
	float * c, 
	const int m, 
	const int n, 
	const int k
)
{
		if (a == nullptr)
		return 1;
	if (b == nullptr)
		return 2;
	if (c == nullptr)
		return 3;
	if (m <= 0)
		return 4;
		if (n <= 0)
		return 5;
		if (k <= 0)
		return 6;

	try
	{
		const auto result = multipy_1d_diff_dim_only_gpu(
			a,
			b,
			c,
			m,
			n,
			k
		);

		if(result != 0)
			return result;
	}
	catch (...)
	{
		return 4;
	}
	return 0;
}
