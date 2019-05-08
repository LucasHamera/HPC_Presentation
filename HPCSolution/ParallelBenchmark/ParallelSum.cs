using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelBenchmark
{
    public static class ParallelSum
    {
        public static int NormalParallelFor(
            int[] Array
        )
        {
            var sum = 0;
            Parallel.For(
                0,
                Array.Length,
                i => sum += Array[i]
            );
            return sum;
        }

        public static int InterlockedParallelFor(
            int[] Array
        )
        {
            var processorCount = Environment.ProcessorCount;
            var iterationPerProcessor = Array.Length / processorCount;

            var sum = 0;
            Parallel.For(
                0,
                processorCount,
                i =>
                {
                    var start = i * iterationPerProcessor;
                    var end = start + iterationPerProcessor;
                    for (int j = start; j < end; j++)
                        Interlocked.Add(ref Array[j], sum);
                }
            );
            return sum;
        }

        public static int PartSumParallelFor(
            int[] Array
        )
        {
            var processorCount = Environment.ProcessorCount;
            var iterationPerProcessor = Array.Length / processorCount;

            var sum = 0;
            Parallel.For(
                0,
                processorCount,
                i =>
                {
                    var partSum = 0;
                    var start = i * iterationPerProcessor;
                    var end = start + iterationPerProcessor;
                    for (int j = start; j < end; j++)
                    {
                        partSum += Array[j];
                    }

                    Interlocked.Add(ref partSum, sum);
                }
            );
            return sum;
        }

        //public static int MyarallelFor(
        //    int[] Array
        //)
        //{
        //    return  MyParallel.Sum(Array);
        //}
    }
}
