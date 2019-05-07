#pragma once
#include <algorithm>

const int get_matrix_index(
	const int row,
	const int col,
	const int matrix_cols
);

void transpose_s(
	float* matrix,
	const int matrix_size
);
