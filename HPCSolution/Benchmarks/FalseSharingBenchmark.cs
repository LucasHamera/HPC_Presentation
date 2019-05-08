using BenchmarkDotNet.Attributes;
using System;
using System.Threading.Tasks;

namespace Benchmarks
{
    #region Result

    //|               Method | ForCount |           Mean |         Error |        StdDev |
    //|--------------------- |--------- |---------------:|--------------:|--------------:|
    //|       NormalParallel |      100 |       7.323 us |     0.1372 us |     0.1409 us |
    //|        SpaceParallel |      100 |       5.221 us |     0.1202 us |     0.1286 us |
    //| SpacePaddingParallel |      100 |       5.348 us |     0.1063 us |     0.1344 us |
    //|       NormalParallel |     1000 |      58.808 us |     0.4466 us |     0.4177 us |
    //|        SpaceParallel |     1000 |      17.547 us |     0.3523 us |     0.6171 us |
    //| SpacePaddingParallel |     1000 |      10.622 us |     0.2233 us |     0.6370 us |
    //|       NormalParallel |    10000 |     365.673 us |     1.5953 us |     1.4922 us |
    //|        SpaceParallel |    10000 |     120.443 us |     1.1587 us |     1.0839 us |
    //| SpacePaddingParallel |    10000 |      46.371 us |     1.0624 us |     2.6849 us |
    //|       NormalParallel |   100000 |   3,606.887 us |    20.7183 us |    19.3799 us |
    //|        SpaceParallel |   100000 |   1,164.434 us |    13.0198 us |    12.1788 us |
    //| SpacePaddingParallel |   100000 |     327.712 us |     6.8728 us |    15.7913 us |
    //|       NormalParallel |  1000000 |  39,931.332 us |   546.1578 us |   510.8764 us |
    //|        SpaceParallel |  1000000 |  11,263.425 us |   157.9595 us |   147.7554 us |
    //| SpacePaddingParallel |  1000000 |   3,176.600 us |    61.1759 us |    85.7601 us |
    //|       NormalParallel | 10000000 | 401,481.435 us | 8,024.7259 us | 8,240.8027 us |
    //|        SpaceParallel | 10000000 |  91,808.504 us | 1,810.4085 us | 2,289.5871 us |
    //| SpacePaddingParallel | 10000000 |  32,117.420 us |   637.5550 us | 1,099.7510 us |

#endregion

    public class FalseSharingBenchmark
    {
        [Params(
            (int)1E+2,
            (int)1E+3,
            (int)1E+4,
            (int)1E+5,
            (int)1E+6,
            (int)1E+7
        )] 
        public int ForCount { get; set; }

        public int[] Array { get; private set; }

        public int[] ArrayWithSpace { get; private set; }

        public int[] ArrayWithPaddingAndSpace { get; private set; }

        public int ProcessorCount { get; private set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            ProcessorCount = Environment.ProcessorCount;
            Array = new int[ProcessorCount];
            ArrayWithSpace = new int[16 * ProcessorCount];
            ArrayWithPaddingAndSpace = new int[16 + 16 * ProcessorCount];
        }

        [Benchmark]
        public void NormalParallel()
        {
            var tasks = new Task[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++)
            {
                var index = i;
                tasks[i] = Task.Run(
                    () =>
                    {
                        for (int j = 0; j < ForCount; j++)
                        {
                            Array[index] += 1;
                        }
                    }
                );
            }
            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void SpaceParallel()
        {
            var tasks = new Task[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++)
            {
                var index = i;
                tasks[i] = Task.Run(
                    () =>
                    {
                        for (int j = 0; j < ForCount; j++)
                        {
                            ArrayWithSpace[index * 16] += 1;
                        }
                    }
                );
            }
            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void SpacePaddingParallel()
        {
            var tasks = new Task[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++)
            {
                var index = i;
                tasks[i] = Task.Run(
                    () =>
                    {
                        for (int j = 0; j < ForCount; j++)
                        {
                            ArrayWithPaddingAndSpace[16 + index * 16] += 1;
                        }
                    }
                );
            }
            Task.WaitAll(tasks);
        }
    }
}
