using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class SequentialVsRandomAccess
    {
//        |     Method | ArraySize |           Mean |         Error |        StdDev |
//        |----------- |---------- |---------------:|--------------:|--------------:|
//        | Sequential |      5000 |       6.535 us |     0.0469 us |     0.0439 us |
//        |     Random |      5000 |       6.488 us |     0.0341 us |     0.0302 us |
//        | Sequential |     10000 |      13.022 us |     0.1204 us |     0.1068 us |
//        |     Random |     10000 |      13.985 us |     0.1092 us |     0.0912 us |
//        | Sequential |    100000 |     132.065 us |     1.1902 us |     1.1133 us |
//        |     Random |    100000 |     220.559 us |     2.9496 us |     2.7591 us |
//        | Sequential |   1000000 |   1,523.046 us |    13.5841 us |    12.0420 us |
//        |     Random |   1000000 |   9,288.885 us |    51.0908 us |    42.6631 us |
//        | Sequential |  10000000 |  15,248.213 us |   185.0738 us |   154.5450 us |
//        |     Random |  10000000 | 131,570.288 us | 2,655.7530 us | 3,261.5032 us |

        [Params(
            5_000,
            10_000,
            100_000,
            1_000_000,
            10_000_000
        )]
        public int ArraySize { get; set; }

        public int[] Array { get; set; }
        public int[] IndicesSequential { get; set; }
        public int[] IndicesRandom { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Array = new int[ArraySize];
            IndicesSequential = new int[ArraySize];
            IndicesRandom = new int[ArraySize];

            for (int i = 0; i < ArraySize; i++)
            {
                Array[i] = 1;
                IndicesSequential[i] = i;
                IndicesRandom[i] = i;
            }

            Randomize(IndicesRandom);
        }

        [Benchmark]
        public void Sequential()
        {
            var sum = 0;
            for (int i = 0; i < ArraySize; i++)
            {
                sum += Array[IndicesSequential[i]];
            }
        }


        [Benchmark]
        public void Random()
        {
            var sum = 0;
            for (int i = 0; i < ArraySize; i++)
            {
                sum += Array[IndicesRandom[i]];
            }
        }

        public static void Randomize<T>(T[] items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
    }
}