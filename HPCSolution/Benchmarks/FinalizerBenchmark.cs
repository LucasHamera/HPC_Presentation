using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    #region Result

    //|           Method | ForCount |            Mean |         Error |        StdDev |          Median |
    //|----------------- |--------- |----------------:|--------------:|--------------:|----------------:|
    //|    WithFinalizer |      100 |    121,930.4 ns |   2,368.13 ns |   2,215.15 ns |    121,561.9 ns |
    //| WithoutFinalizer |      100 |        674.9 ns |      33.11 ns |      93.91 ns |        642.2 ns |
    //|    WithFinalizer |     1000 |  1,056,808.4 ns |  25,464.81 ns |  22,573.89 ns |  1,051,555.8 ns |
    //| WithoutFinalizer |     1000 |      4,993.7 ns |     110.33 ns |     122.63 ns |      4,935.6 ns |
    //|    WithFinalizer |    10000 | 10,682,628.5 ns | 207,008.17 ns | 212,582.15 ns | 10,648,876.6 ns |
    //| WithoutFinalizer |    10000 |     49,879.8 ns |     995.91 ns |   1,490.64 ns |     49,587.6 ns |

    #endregion

    public class FinalizerBenchmark
    {
        [Params(
            (int)1E+2,
            (int)1E+3,
            (int)1E+4
        )] 
        public int ForCount { get; set; }

        [Benchmark]
        public void WithFinalizer()
        {
            for (int i = 0; i < ForCount; i++)
            {
                var @object = new ClassWitFinalizer {A = 1};
            }
        }

        [Benchmark]
        public void WithoutFinalizer()
        {
            for (int i = 0; i < ForCount; i++)
            {
                var @object = new ClassWithoutFinalizer {A = 1};
            }
        }


        public class ClassWithoutFinalizer
        {
            public float A { get; set; }
            public float B { get; set; }
            public float C { get; set; }
            public float D { get; set; }
        }

        public class ClassWitFinalizer
        {
            public float A { get; set; }
            public float B { get; set; }
            public float C { get; set; }
            public float D { get; set; }

            ~ClassWitFinalizer()
            {

            }
        }
    }
}
