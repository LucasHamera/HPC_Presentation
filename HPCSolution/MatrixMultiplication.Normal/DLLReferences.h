#pragma once

#define DLLIMPORT __declspec(dllimport)   

extern "C" {
	int DLLIMPORT multipy_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);

	int DLLIMPORT parallel_multipy_r(
		const float  *a,
		float  *b,
		float  *c,
		const int matrix_size
	);
}