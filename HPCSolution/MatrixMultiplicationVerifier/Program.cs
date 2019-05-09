using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MatrixMultiplication.NormalSharp;
using MatrixMultiplication.NormalWrapper;
using MatrixMultiplication.SIMDWrapper;
using MatrixMultiplication.Vector;

namespace MatrixMultiplicationVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 15;

            var a1d = new float[n * n];
            var b1d = new float[n * n];

            var a2d = new float[n, n];
            var b2d = new float[n, n];

            var aJagged = new float[n][];
            var bJagged = new float[n][];

            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                aJagged[i] = new float[n];
                bJagged[i] = new float[n];

                for (int j = 0; j < n; j++)
                {
                    var a = (float) random.NextDouble();
                    var b = (float) random.NextDouble();

                    a1d[i * n + j] = a;
                    b1d[i * n + j] = b;

                    a2d[i, j] = a;
                    b2d[i, j] = b;

                    aJagged[i][j] = a;
                    bJagged[i][j] = b;
                }
            }

            var expectedResult1d = new float[n * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                for (int p = 0; p < n; p++)
                {
                    var c = a1d[i * n + p] * b1d[p * n + j];
                    expectedResult1d[i * n + j] += c;
                }
            }

            var methods = FindMultiplicationMethods<NormalMatrixMultiplication>()
                .Concat(FindMultiplicationMethods<SIMDMatrixMultiplicationWrapper>())
                .Concat(FindMultiplicationMethods<MatrixMultiplicationWrapper>())
                .Concat(FindMultiplicationMethods<VectorMatrixMultiplication>());

            foreach (var (name, matrixType, function) in methods)
            {
                Func<int, int, float> itemGetter = null;

                if (matrixType == typeof(float[]))
                {
                    var c = new float[n * n];
                    function(a1d, b1d, c, n);
                    itemGetter = (i, j) => c[i * n + j];
                }
                else if (matrixType == typeof(float[,]))
                {
                    var c = new float[n, n];
                    function(a2d, b2d, c, n);
                    itemGetter = (i, j) => c[i, j];
                }
                else if (matrixType == typeof(float[][]))
                {
                    var c = Enumerable.Range(0, n).Select(x => new float[n]).ToArray();
                    function(aJagged, bJagged, c, n);
                    itemGetter = (i, j) => c[i][j];
                }

                if (itemGetter == null)
                {
                    Console.WriteLine($"Error: cannot verify {name}");
                    continue;
                }

                var isOk = true;
                for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    var actual = itemGetter(i, j);
                    var expected = expectedResult1d[i * n + j];
                    var isEqual = Math.Abs(actual - expected) <= 0.0001;
                    if (!isEqual)
                    {
                        if (isOk)
                            Console.WriteLine($"{name}: errors");

                        Console.WriteLine($" Wrong value at {i},{j}: {actual} != {expected}");
                        isOk = false;
                    }
                }

                if (isOk)
                    Console.WriteLine($"{name}: ok");
            }
        }

        private static IEnumerable<(string, Type, Action<object, object, object, int>)> FindMultiplicationMethods<T>()
        {
            var methods = typeof(T).GetMembers(BindingFlags.Static | BindingFlags.Public).Cast<MethodInfo>();
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 4)
                    continue;
                if (parameters[3].ParameterType != typeof(int))
                    continue;
                if (parameters[0].ParameterType != parameters[1].ParameterType ||
                    parameters[1].ParameterType != parameters[2].ParameterType)
                    continue;
                var matrixType = parameters[0].ParameterType;

                var methodName = $"{typeof(T).Name}.{method.Name}";
                yield return (
                    methodName,
                    matrixType,
                    (a, b, c, size) => method.Invoke(null, new[] {a, b, c, size})
                );
            }
        }
    }
}