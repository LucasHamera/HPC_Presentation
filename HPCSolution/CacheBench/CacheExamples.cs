using System;

namespace CacheBench
{
    public struct Vector3D
    {
        public float x;
        public float y;
        public float z;

        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct Particle
    {
        public Vector3D position;
        public Vector3D velocity;
        public Vector3D acceleration;
    }

    public class CacheExamples
    {
        public static Vector3D SumSeparate(int n, float[] xs, float[] ys, float[] zs)
        {
            var sumX = 0.0f;
            var sumY = 0.0f;
            var sumZ = 0.0f;
            for (var i = 0; i < n; ++i)
            {
                sumX += xs[i];
                sumY += ys[i];
                sumZ += zs[i];
            }

            return new Vector3D(sumX / n, sumY / n, sumZ / n);
        }

        public static Vector3D SumSmallStruct(int n, Vector3D[] b)
        {
            var sumX = 0.0f;
            var sumY = 0.0f;
            var sumZ = 0.0f;
            for (var i = 0; i < n; ++i)
            {
                sumX += b[i].x;
                sumY += b[i].y;
                sumZ += b[i].z;
            }

            return new Vector3D(sumX / n, sumY / n, sumZ / n);
        }

        public static Vector3D SumBigStruct(int n, Particle[] a)
        {
            var sumX = 0.0f;
            var sumY = 0.0f;
            var sumZ = 0.0f;
            for (var i = 0; i < n; ++i)
            {
                sumX += a[i].position.x;
                sumY += a[i].position.y;
                sumZ += a[i].position.z;
            }

            return new Vector3D(sumX / n, sumY / n, sumZ / n);
        }
    }
}