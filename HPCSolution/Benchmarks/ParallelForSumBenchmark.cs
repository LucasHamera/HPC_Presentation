using BenchmarkDotNet.Attributes;
using ParallelBenchmark;

namespace Benchmarks
{
    #region Result

    //|                 Method | ArraySize |              Mean |            Error |           StdDev |
    //|----------------------- |---------- |------------------:|-----------------:|-----------------:|
    //|              NormalFor |       100 |          88.26 ns |         1.765 ns |         2.587 ns |
    //|            ParallelFor |       100 |       4,345.23 ns |        86.058 ns |       183.396 ns |
    //| InterlockedParallelFor |       100 |       7,775.56 ns |       143.239 ns |       126.978 ns |
    //|     PartSumParallelFor |       100 |       6,922.29 ns |       137.561 ns |       340.017 ns |
    //|           MyarallelFor |       100 |       6,161.03 ns |        24.366 ns |        21.600 ns |
    //|              NormalFor |      1000 |         760.46 ns |        11.856 ns |        10.510 ns |
    //|            ParallelFor |      1000 |      18,910.59 ns |       308.736 ns |       288.791 ns |
    //| InterlockedParallelFor |      1000 |      11,931.58 ns |       219.589 ns |       234.957 ns |
    //|     PartSumParallelFor |      1000 |       9,109.71 ns |       181.031 ns |       312.270 ns |
    //|           MyarallelFor |      1000 |       7,544.10 ns |        65.445 ns |        61.217 ns |
    //|              NormalFor |     10000 |       7,744.85 ns |       128.455 ns |       107.265 ns |
    //|            ParallelFor |     10000 |     162,288.61 ns |     1,979.426 ns |     1,851.556 ns |
    //| InterlockedParallelFor |     10000 |      34,347.41 ns |       640.156 ns |       567.482 ns |
    //|     PartSumParallelFor |     10000 |      14,105.94 ns |       277.723 ns |       361.118 ns |
    //|           MyarallelFor |     10000 |      10,292.72 ns |       205.200 ns |       228.079 ns |
    //|              NormalFor |    100000 |      76,269.08 ns |     1,465.361 ns |     1,370.699 ns |
    //|            ParallelFor |    100000 |   1,567,747.81 ns |    11,854.581 ns |     9,899.112 ns |
    //| InterlockedParallelFor |    100000 |     215,301.67 ns |     4,146.868 ns |     5,535.950 ns |
    //|     PartSumParallelFor |    100000 |      51,281.62 ns |     1,019.703 ns |     2,925.719 ns |
    //|           MyarallelFor |    100000 |      34,515.35 ns |       688.253 ns |     1,739.303 ns |
    //|              NormalFor |   1000000 |     764,057.35 ns |    15,004.853 ns |    21,993.920 ns |
    //|            ParallelFor |   1000000 |  15,512,268.27 ns |   168,669.500 ns |   140,846.672 ns |
    //| InterlockedParallelFor |   1000000 |   1,860,091.52 ns |    35,692.358 ns |    42,489.214 ns |
    //|     PartSumParallelFor |   1000000 |     347,052.34 ns |     9,629.496 ns |    27,936.913 ns |
    //|           MyarallelFor |   1000000 |     228,235.68 ns |     4,498.809 ns |     9,780.040 ns |
    //|              NormalFor |  10000000 |   8,190,111.16 ns |   163,263.171 ns |   194,353.192 ns |
    //|            ParallelFor |  10000000 | 140,741,516.67 ns | 2,813,288.561 ns | 2,493,907.032 ns |
    //| InterlockedParallelFor |  10000000 |  17,620,358.33 ns |   212,509.951 ns |   198,781.940 ns |
    //|     PartSumParallelFor |  10000000 |   2,882,537.91 ns |    57,524.398 ns |   121,338.479 ns |
    //|           MyarallelFor |  10000000 |   1,935,747.97 ns |    38,639.848 ns |   105,122.556 ns |

#endregion

    public class ParallelForSumBenchmark
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

        public int[] Array { get; private set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Array = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
                Array[i] = 1;
        }

        [Benchmark]
        public float NormalFor()
        {
            var sum = 0;
            for (int i = 0; i <  ArraySize; i++)
                sum += Array[i];
            return sum;
        }

        [Benchmark]
        public float ParallelFor()
        {
            return ParallelSum
                .NormalParallelFor(
                    Array
                );
        }

        [Benchmark]
        public float InterlockedParallelFor()
        {
            return ParallelSum
                .InterlockedParallelFor(
                    Array
                );
        }

        [Benchmark]
        public float PartSumParallelFor()
        {
            return ParallelSum
                .PartSumParallelFor(
                    Array
                );
        }

        //[Benchmark]
        //public float MyarallelFor()
        //{
        //    return ParallelSum
        //        .MyarallelFor(
        //            Array
        //        );
        //}
    }
}
