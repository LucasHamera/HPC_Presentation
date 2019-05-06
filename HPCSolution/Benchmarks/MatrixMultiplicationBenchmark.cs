using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class MatrixMultiplicationBenchmark
    {
        [Params(
            10
            ,20
            ,50
            ,100
            ,200
            ,300
            ,500
        )]
        public uint MatrixSize { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {

        }

        [Benchmark]
        public void ExampleBenchmark()
        {

        }
    }
}
