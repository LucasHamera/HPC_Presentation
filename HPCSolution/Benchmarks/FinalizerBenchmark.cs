using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class FinalizerBenchmark
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
                var @object = new ClassWitFinalizer {A = 1};
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
