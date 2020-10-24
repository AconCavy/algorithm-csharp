using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class EnumerationModuloTests
    {
        [Test]
        public void FactorialTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            for (var n = 10000; n >= 1; n--)
                Assert.That(EnumerationModulo.Factorial(n).Value, Is.EqualTo(Enumeration.Factorial(n, modulo)));
        }

        [Test]
        public void CombinationTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            const int n = 10000;
            for (var r = 1; r <= n; r++)
                Assert.That(EnumerationModulo.Combination(n, r).Value,
                    Is.EqualTo(Enumeration.Combination(n, r, modulo)));
        }

        [Test]
        public void PermutationTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            const int n = 10000;
            for (var r = 1; r <= n; r++)
                Assert.That(EnumerationModulo.Permutation(n, r).Value,
                    Is.EqualTo(Enumeration.Permutation(n, r, modulo)));
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 2)]
        public void InvalidArgumentsTest(int n, int r)
        {
            Assert.Throws<ArgumentException>(() => _ = EnumerationModulo.Combination(n, r));
            Assert.Throws<ArgumentException>(() => _ = EnumerationModulo.Permutation(n, r));
        }

        private static class Enumeration
        {
            private static readonly Dictionary<long, long> Memo = new Dictionary<long, long> {{0, 1}, {1, 1}};
            private static long _max = 1;

            public static long Factorial(long n, long modulo)
            {
                if (Memo.ContainsKey(n)) return Memo[n];
                var val = Memo[_max];
                for (var i = _max + 1; i <= n; i++)
                {
                    val *= i % modulo;
                    val %= modulo;
                    Memo[i] = val;
                }

                _max = n;
                return Memo[n];
            }

            public static long Permutation(long n, long r, long modulo)
            {
                var top = Factorial(n, modulo);
                var bottom = Factorial(n - r, modulo);
                return top * Math.PowerModulo(bottom, modulo - 2, modulo) % modulo;
            }

            public static long Combination(long n, long r, long modulo)
            {
                r = System.Math.Min(r, n - r);
                var top = Factorial(n, modulo);
                var bottom = Factorial(r, modulo) * Factorial(n - r, modulo) % modulo;
                return top * Math.PowerModulo(bottom, modulo - 2, modulo) % modulo;
            }
        }
    }
}