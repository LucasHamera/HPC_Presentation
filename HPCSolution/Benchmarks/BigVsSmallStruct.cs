using System;
using BenchmarkDotNet.Attributes;
using CacheBench;

namespace Benchmarks
{
//    |         Method | ArraySize |         Mean |      Error |     StdDev |
//    |--------------- |---------- |-------------:|-----------:|-----------:|
//    |   SumBigStruct |      1000 |     1.255 us |  0.0091 us |  0.0080 us |
//    | SumSmallStruct |      1000 |     1.493 us |  0.0184 us |  0.0163 us |
//    |    SumSeparate |      1000 |     1.194 us |  0.0258 us |  0.0623 us |
//    |   SumBigStruct |     10000 |    17.379 us |  0.1505 us |  0.1257 us |
//    | SumSmallStruct |     10000 |    14.864 us |  0.0756 us |  0.0590 us |
//    |    SumSeparate |     10000 |    11.349 us |  0.1312 us |  0.1163 us |
//    |   SumBigStruct |    100000 |   264.819 us |  2.4725 us |  2.1918 us |
//    | SumSmallStruct |    100000 |   152.678 us |  3.0505 us |  3.5129 us |
//    |    SumSeparate |    100000 |   114.196 us |  0.8504 us |  0.7101 us |
//    |   SumBigStruct |   1000000 | 2,869.095 us | 31.9370 us | 29.8739 us |
//    | SumSmallStruct |   1000000 | 1,864.254 us | 20.2691 us | 18.9597 us |
//    |    SumSeparate |   1000000 | 1,473.229 us | 29.2465 us | 31.2934 us |

    public class BigVsSmallStruct
    {
        [Params(
            1_000,
            10_000,
            100_000,
            1_000_000
        )]
        public int ArraySize { get; set; }

        Particle[] particles;
        Vector3D[] positions;
        float[] xs;
        float[] ys;
        float[] zs;

        [GlobalSetup]
        public void GlobalSetup()
        {
            particles = new Particle[ArraySize];
            positions = new Vector3D[ArraySize];
            xs = new float[ArraySize];
            ys = new float[ArraySize];
            zs = new float[ArraySize];

            var r = new Random();

            for (int i = 0; i < ArraySize; ++i)
            {
                var x = (float)r.NextDouble();
                var y = (float)r.NextDouble();
                var z = (float)r.NextDouble();
                var position = new Vector3D(x, y, z);

                particles[i] = new Particle
                {
                    position = position,
                    velocity = new Vector3D(y, z, x),
                };

                positions[i] = position;

                xs[i] = x;
                ys[i] = y;
                zs[i] = z;
            }
        }

        [Benchmark]
        public void SumBigStruct()
        {
            CacheExamples.SumBigStruct(ArraySize, particles);
        }

        [Benchmark]
        public void SumSmallStruct()
        {
            CacheExamples.SumSmallStruct(ArraySize, positions);
        }

        [Benchmark]
        public void SumSeparate()
        {
            CacheExamples.SumSeparate(ArraySize, xs, ys, zs);
        }
    }
}
