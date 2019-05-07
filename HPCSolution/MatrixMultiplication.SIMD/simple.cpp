#include <iostream>
#include <immintrin.h>
#include <vector>
#include <chrono>
#include <cassert>


void transpose(float* a, int n)
{
	for (int i = 0; i < n; ++i)
		for (int j = 0; i < n; ++i)
		{
			std::swap(
				a[i * n + j],
				a[j * n + i]
			);
		}
}

void scalar(float* a, float* b, float* c, int n)
{
	transpose(b, n);

	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			float tmp = 0.0f;
			for (int k = 0; k < n; ++k)
				tmp += a[i * n + k] * b[j * n + k];
			c[i * n + j] = tmp;
		}

	transpose(b, n);
}


void vectorized_avx2(float* a, float* b, float* c, int n)
{
	transpose(b, n);

	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			int k = 0;
			__m256 sum = _mm256_set1_ps(0.0f);
			while (k + 7 < n)
			{
				const auto x = _mm256_loadu_ps(&a[i * n + k]);
				const auto y = _mm256_loadu_ps(&b[j * n + k]);
				sum = _mm256_add_ps(sum, _mm256_mul_ps(x, y));

				k += 8;
			}

			float v[8];
			_mm256_storeu_ps(&v[0], sum);
			float tmp = v[0] + v[1] + v[2] + v[3] + v[4] + v[5] + v[6] + v[7];
			for (; k < n; ++k)
				tmp += a[i * n + k] * b[j * n + k];
			c[i * n + j] = tmp;
		}

	transpose(b, n);
}


void vectorized_sse(float* a, float* b, float* c, int n)
{
	transpose(b, n);

	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			float tmp = 0.0f;

			int k = 0;
			__m128 sum = _mm_set1_ps(0.0f);
			while (k + 3 < n)
			{
				const auto x = _mm_loadu_ps(&a[i * n + k]);
				const auto y = _mm_loadu_ps(&b[j * n + k]);
				sum = _mm_add_ps(sum, _mm_mul_ps(x, y));

				k += 4;
			}

			float v[4];
			_mm_storeu_ps(&v[0], sum);
			tmp = v[0] + v[1] + v[2] + v[3];

			for (; k < n; ++k)
				tmp += a[i * n + k] * b[j * n + k];
			c[i * n + j] = tmp;
		}

	transpose(b, n);
}


void scalar_not_transposed(float* a, float* b, float* c, int n)
{
	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			float tmp = 0.0f;
			for (int k = 0; k < n; ++k)
				tmp += a[i * n + k] * b[k * n + j];
			c[i * n + j] = tmp;
		}
}

void assert_identity(float* a, int n)
{
	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			if (i == j)
				assert(std::fabs(a[i * n + j] - 1.0f) < 0.0001f);
			else
				assert(std::fabs(a[i * n + j]) < 0.0001f);
		}
}

template <typename F>
auto measure(F f)
{
	const auto start = std::chrono::high_resolution_clock::now();
	f();
	const auto end = std::chrono::high_resolution_clock::now();
	return std::chrono::duration_cast<std::chrono::microseconds>(end - start).count() / 1000.0f;
}


int main(int argc, char* argv[])
{
	const auto N = 500;

	std::vector<float> a;
	a.resize(N * N, 0.0f);
	for (int i = 0; i < N; i++)
		for (int j = 0; j < N; j++)
			a[i * N + j] = i == j ? 1.0f : 0.0f;


	std::vector<float> b;
	b.resize(N * N, 0.0f);
	for (int i = 0; i < N; i++)
		for (int j = 0; j < N; j++)
			b[i * N + j] = i == j ? 1.0f : 0.0f;


	std::vector<float> c;
	c.resize(N * N, 1.0f);

	const auto t1 = measure([&]() { scalar(a.data(), b.data(), c.data(), N); });
	assert_identity(c.data(), N);

	const auto t2 = measure([&]() { vectorized_avx2(a.data(), b.data(), c.data(), N); });
	assert_identity(c.data(), N);

	const auto t3 = measure([&]() { scalar_not_transposed(a.data(), b.data(), c.data(), N); });
	assert_identity(c.data(), N);

	const auto t4 = measure([&]() { vectorized_sse(a.data(), b.data(), c.data(), N); });
	assert_identity(c.data(), N);

	std::cout << "normal c++: " << t1 << "ms\n";
	std::cout << "avx2 c++: " << t2 << "ms\n";
	std::cout << "not transposed c++: " << t3 << "ms\n";
	std::cout << "sse c++: " << t4 << "ms\n";
}
