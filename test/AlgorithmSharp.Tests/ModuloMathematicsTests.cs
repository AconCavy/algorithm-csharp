using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class ModuloMathematicsTests
    {
        [Test]
        public void FactorialTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            for (var n = 10000; n >= 1; n--)
                Assert.That(ModuloMathematics.Factorial(n).Value, Is.EqualTo(Enumeration.Factorial(n, modulo)));
        }

        [Test]
        public void CombinationTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            const int n = 10000;
            for (var r = 1; r <= n; r++)
                Assert.That(ModuloMathematics.Combination(n, r).Value,
                    Is.EqualTo(Enumeration.Combination(n, r, modulo)));
        }

        [Test]
        public void PermutationTest([Values(11, 998244353, 1000000007)] int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            const int n = 10000;
            for (var r = 1; r <= n; r++)
                Assert.That(ModuloMathematics.Permutation(n, r).Value,
                    Is.EqualTo(Enumeration.Permutation(n, r, modulo)));
        }

        [Test]
        public void NLessThan0Test()
        {
            const int n = -1;
            Assert.Throws<ArgumentException>(() => _ = ModuloMathematics.Factorial(n));
            Assert.Throws<ArgumentException>(() => _ = ModuloMathematics.Combination(n, 5));
            Assert.Throws<ArgumentException>(() => _ = ModuloMathematics.Permutation(n, 5));
        }

        [Test]
        public void LessThanAndGreaterThanNTest([Values(-1, 3)] int r)
        {
            const int n = 2;
            Assert.Throws<ArgumentException>(() => _ = ModuloMathematics.Combination(n, r));
            Assert.Throws<ArgumentException>(() => _ = ModuloMathematics.Permutation(n, r));
        }

        private static class Enumeration
        {
            private static readonly Dictionary<long, long> Memo = new Dictionary<long, long> { { 0, 1 }, { 1, 1 } };
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
                return top * Mathematics.PowerModulo(bottom, modulo - 2, modulo) % modulo;
            }

            public static long Combination(long n, long r, long modulo)
            {
                r = Math.Min(r, n - r);
                var top = Factorial(n, modulo);
                var bottom = Factorial(r, modulo) * Factorial(n - r, modulo) % modulo;
                return top * Mathematics.PowerModulo(bottom, modulo - 2, modulo) % modulo;
            }
        }
    }
}