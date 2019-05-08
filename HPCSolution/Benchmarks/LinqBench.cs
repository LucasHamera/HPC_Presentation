using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
//|     Method | ArraySize |          Mean |         Error |        StdDev |        Median |
//|----------- |---------- |--------------:|--------------:|--------------:|--------------:|
//|    SumLinq |      5000 |      58.32 us |     0.1861 us |     0.1650 us |      58.31 us |
//|     SumFor |      5000 |      20.32 us |     0.1706 us |     0.1596 us |      20.28 us |
//|    SumLinq |     10000 |     121.99 us |     2.4236 us |     3.6275 us |     120.86 us |
//|     SumFor |     10000 |      51.72 us |     0.3156 us |     0.2798 us |      51.69 us |
//|    SumLinq |    100000 |   1,212.13 us |     6.2175 us |     5.5116 us |   1,211.98 us |
//|     SumFor |    100000 |     582.71 us |     1.4556 us |     1.2904 us |     582.90 us |
//|    SumLinq |   1000000 |  12,054.62 us |    57.4964 us |    50.9690 us |  12,053.00 us |
//|     SumFor |   1000000 |   5,908.37 us |    24.1685 us |    21.4248 us |   5,907.72 us |
//|    SumLinq |  10000000 | 122,052.05 us |   446.7993 us |   373.0976 us | 122,101.08 us |
//|     SumFor |  10000000 |  59,074.95 us |   232.1084 us |   193.8211 us |  59,036.53 us |

    public class LinqBench
    {
        [Params(
            5_000,
            10_000,
            100_000,
            1_000_000,
            10_000_000
        )]
        public int ArraySize;

        public float[] Array;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();

            Array = new float[ArraySize];

            for (int i = 0; i < ArraySize; i++)
                Array[i] = 2 * (float) random.NextDouble() - 1;
        }

//        [Benchmark]
//        public void MinMaxLinq()
//        {
//            var minValue = Array.Where(x => x > 0).Min();
//            var maxValue = Array.Where(x => x > 0).Max();
//        }
//
//
//        [Benchmark]
//        public void MinMaxFor()
//        {
//            var minValue = +1e10;
//            var maxValue = -1e10;
//
//            for (int i = 0; i < ArraySize; i++)
//            {
//                if (Array[i] > 0)
//                {
//                    if (Array[i] < minValue)
//                        minValue = Array[i];
//
//                    if (Array[i] > maxValue)
//                        maxValue = Array[i];
//                }
//            }
//        }


        [Benchmark]
        public void SumLinq()
        {
            var sum = Array.Where(x => x > 0).Sum();
        }

        [Benchmark]
        public void SumFor()
        {
            var sum = 0.0f;
            for (int i = 0; i < ArraySize; i++)
            {
                if (Array[i] > 0)
                {
                    sum += Array[i];
                }
            }
        }
    }
}