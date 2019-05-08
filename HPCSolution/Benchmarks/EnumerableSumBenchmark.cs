using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Benchmarks
{
    #region Results

    //|         Method | ArraySize |             Mean |          Error |         StdDev |
    //|--------------- |---------- |-----------------:|---------------:|---------------:|
    //| IEnumerableSum |       100 |        581.25 ns |       3.802 ns |       3.175 ns |
    //|       ArraySum |       100 |         99.07 ns |       1.763 ns |       1.562 ns |
    //| IEnumerableSum |      1000 |      4,841.97 ns |      50.678 ns |      44.925 ns |
    //|       ArraySum |      1000 |      1,355.81 ns |      23.015 ns |      21.529 ns |
    //| IEnumerableSum |     10000 |     48,615.20 ns |     643.534 ns |     570.476 ns |
    //|       ArraySum |     10000 |     13,633.71 ns |     120.713 ns |     112.915 ns |
    //| IEnumerableSum |    100000 |    491,725.56 ns |  10,050.985 ns |  17,067.336 ns |
    //|       ArraySum |    100000 |    136,365.28 ns |   1,530.016 ns |   1,431.178 ns |
    //| IEnumerableSum |   1000000 |  4,884,352.68 ns |  71,110.635 ns |  63,037.726 ns |
    //|       ArraySum |   1000000 |  1,377,049.32 ns |  18,570.713 ns |  17,371.056 ns |
    //| IEnumerableSum |  10000000 | 48,499,426.00 ns | 808,425.975 ns | 756,202.159 ns |
    //|       ArraySum |  10000000 | 13,938,410.16 ns |  75,008.864 ns |  66,493.404 ns |

#endregion

    public class EnumerableSumBenchmark
    {
        [Params(
            (int)1E+2,
            (int)1E+3,
            (int)1E+4,
            (int)1E+5,
            (int)1E+6,
            (int)1E+7
        )] 
        public int ArraySize { get; set; }

        public float[] Array { get; private set; }

        [GlobalSetup]
        public void SetUp()
        {
            Array = new float[ArraySize];
        }

        [Benchmark]
        public void IEnumerableSum()
        {
            SumEnumerable(Array);
        }

        private float SumEnumerable(
            IEnumerable<float> enumerable
        )
        {
            var sum = 0.0f;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            return sum;
        }

        [Benchmark]
        public void ArraySum()
        {
            SumArray(Array);
        }

        private float SumArray(
            float[] array
        )
        {
            var sum = 1.0f;
            foreach (var item in array)
            {
                sum += item;
            }
            return sum;
        }


    }
}
