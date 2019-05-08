using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
//    |   Method | ArraySize |          Mean |       Error |        StdDev |
//    |--------- |---------- |--------------:|------------:|--------------:|
//    |   Sorted |      5000 |      7.449 us |   0.0674 us |     0.0563 us |
//    | Unsorted |      5000 |     24.973 us |   0.1648 us |     0.1461 us |
//    |   Sorted |     10000 |     14.902 us |   0.0925 us |     0.0820 us |
//    | Unsorted |     10000 |     60.338 us |   0.4044 us |     0.3783 us |
//    |   Sorted |    100000 |    149.524 us |   1.7804 us |     1.5783 us |
//    | Unsorted |    100000 |    684.131 us |  14.9854 us |    25.8492 us |
//    |   Sorted |   1000000 |  1,587.703 us |   8.3020 us |     7.3595 us |
//    | Unsorted |   1000000 |  7,518.435 us | 197.0024 us |   577.7740 us |
//    |   Sorted |  10000000 | 17,526.182 us | 568.5364 us | 1,612.8432 us |
//    | Unsorted |  10000000 | 63,119.553 us | 389.4182 us |   345.2091 us |

    public class SortedVsUnsortedArray
    {
        [Params(5_000,
            10_000,
            100_000,
            1_000_000,
            10_000_000)]
        public int ArraySize;

        public float[] ArraySorted;
        public float[] ArrayUnsorted;

        [GlobalSetup]
        public void Setup()
        {
            var r = new Random();

            ArrayUnsorted = new float[ArraySize];
            ArraySorted = new float[ArraySize];
            for (int i = 0; i < ArraySize; i++)
                ArrayUnsorted[i] = 2* (float) r.NextDouble() - 1.0f;

            Array.Copy(ArrayUnsorted, ArraySorted, ArraySize);
            Array.Sort(ArraySorted);
        }

        [Benchmark]
        public void Sorted()
        {
            float sumPositive = 0.0f;
            for (int i = 0; i < ArraySize; i++)
            {
                if (ArraySorted[i] > 0.0)
                    sumPositive += ArraySorted[i];
            }
        }

        [Benchmark]
        public void Unsorted()
        {
            float sumPositive = 0.0f;
            for (int i = 0; i < ArraySize; i++)
            {
                if (ArrayUnsorted[i] > 0.0)
                    sumPositive += ArrayUnsorted[i];
            }
        }
    }
}
