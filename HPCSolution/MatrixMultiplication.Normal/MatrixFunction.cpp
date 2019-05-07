#include "MatrixFunction.h"


const int get_matrix_index(
	const int row,
	const int col,
	const int matrix_cols
)
{
	return row * matrix_cols + col;
}

void transpose_s(
	float* matrix,
	const int matrix_size
)
{
	for (int i = 0; i < matrix_size; i++)
		for (int j = i + 1; j < matrix_size; j++)
			std::swap(
				matrix[get_matrix_index(i, j, matrix_size)],
				matrix[get_matrix_index(j, i, matrix_size)]
			);
}
