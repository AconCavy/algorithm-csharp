using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class MathematicsTests
    {
        [Test]
        public void ChineseRemainderTheoremInvalidArgumentsTest()
        {
            Assert.Throws<ArgumentException>(() =>
                Mathematics.ChineseRemainderTheorem(new long[] {1, 2}, new long[] {3}));
            Assert.Throws<ArgumentException>(() =>
                Mathematics.ChineseRemainderTheorem(new long[] {1, 2}, new long[] {-1, -2}));
        }

        [Test]
        public void ChineseRemainderTheoremHandmadeTest()
        {
            var (rem, mod) = Mathematics.ChineseRemainderTheorem(new long[] {1, 2, 1}, new long[] {2, 3, 2});
            Assert.That(rem, Is.EqualTo(5));
            Assert.That(mod, Is.EqualTo(6));
        }

        [Test]
        public void ChineseRemainderTheorem2ItemsTest()
        {
            for (var a = 1; a <= 20; a++)
            for (var b = 1; b <= 20; b++)
            for (var c = -10; c <= 10; c++)
            for (var d = -10; d <= 10; d++)
            {
                var (rem, mod) = Mathematics.ChineseRemainderTheorem(new long[] {c, d}, new long[] {a, b});
                var lcm = a * b / GreatestCommonDivisor(a, b);
                if (mod == 0)
                {
                    for (var x = 0; x < lcm; x++) Assert.That(x % a != c || x % b != d, Is.True);
                    continue;
                }

                Assert.That(mod, Is.EqualTo(lcm));
                Assert.That(rem % a, Is.EqualTo(Mathematics.SafeModulo(c, a)));
                Assert.That(rem % b, Is.EqualTo(Mathematics.SafeModulo(d, b)));
            }
        }

        [Test]
        public void ChineseRemainderTheorem3ItemsTest()
        {
            for (var a = 1; a <= 5; a++)
            for (var b = 1; b <= 5; b++)
            for (var c = 1; c <= 5; c++)
            for (var d = -5; d <= 5; d++)
            for (var e = -5; e <= 5; e++)
            for (var f = -5; f <= 5; f++)
            {
                var (rem, mod) = Mathematics.ChineseRemainderTheorem(new long[] {d, e, f}, new long[] {a, b, c});
                var lcm = a * b / GreatestCommonDivisor(a, b);
                lcm *= c / GreatestCommonDivisor(lcm, c);
                if (mod == 0)
                {
                    for (var x = 0; x < lcm; x++) Assert.That(x % a != d || x % b != e || x % c != f, Is.True);
                    continue;
                }

                Assert.That(mod, Is.EqualTo(lcm));
                Assert.That(rem % a, Is.EqualTo(Mathematics.SafeModulo(d, a)));
                Assert.That(rem % b, Is.EqualTo(Mathematics.SafeModulo(e, b)));
                Assert.That(rem % c, Is.EqualTo(Mathematics.SafeModulo(f, c)));
            }
        }

        [Test]
        public void FloorSumTest()
        {
            for (var n = 0; n < 20; n++)
            for (var m = 1; m < 20; m++)
            for (var a = 0; a < 20; a++)
            for (var b = 0; b < 20; b++)
                Assert.That(Mathematics.FloorSum(n, m, a, b), Is.EqualTo(FloorSumNaive(n, m, a, b)));
        }

        [Test]
        public void InverseGreatestCommonDivisorTest()
        {
            var list = new List<long>();
            for (var i = 0; i <= 10; i++)
            {
                list.Add(i);
                list.Add(-i);
                list.Add(long.MinValue + i);
                list.Add(long.MinValue - i);
                list.Add(long.MinValue / 2 + i);
                list.Add(long.MinValue / 2 - i);
                list.Add(long.MaxValue / 2 + i);
                list.Add(long.MaxValue / 2 - i);
                list.Add(long.MinValue / 3 + i);
                list.Add(long.MinValue / 3 - i);
                list.Add(long.MaxValue / 3 + i);
                list.Add(long.MaxValue / 3 - i);
            }

            list.Add(998244353);
            list.Add(-998244353);
            list.Add(1000000007);
            list.Add(-1000000007);
            list.Add(1000000009);
            list.Add(-1000000009);

            foreach (var a in list)
            foreach (var b in list)
            {
                if (b <= 0) continue;
                var a2 = Mathematics.SafeModulo(a, b);
                var igcd = Mathematics.InverseGreatestCommonDivisor(a, b);
                var g = GreatestCommonDivisor(a2, b);
                Assert.That(igcd.g, Is.EqualTo(g));
                Assert.That(igcd.im, Is.GreaterThanOrEqualTo(0));
                Assert.That(b / igcd.g, Is.GreaterThanOrEqualTo(igcd.im));
            }
        }

        [Test]
        public void InverseModuloTest([Range(1, 1000)] int modulo)
        {
            for (var a = -100; a <= 100; a++)
            {
                if (GreatestCommonDivisor(Mathematics.SafeModulo(a, modulo), modulo) != 1) continue;
                var c = Mathematics.InverseModulo(a, modulo);
                Assert.That(c, Is.GreaterThanOrEqualTo(0));
                Assert.That(modulo, Is.GreaterThanOrEqualTo(c));
                Assert.That((a * c % modulo + modulo) % modulo, Is.EqualTo(1 % modulo));
            }
        }

        [Test]
        public void InverseModuloZeroTest([Range(0, 10)] int x)
        {
            Assert.That(Mathematics.InverseModulo(x, 1), Is.Zero);
            Assert.That(Mathematics.InverseModulo(-x, 1), Is.Zero);
            Assert.That(Mathematics.InverseModulo(long.MinValue + x, 1), Is.Zero);
            Assert.That(Mathematics.InverseModulo(long.MaxValue - x, 1), Is.Zero);
        }

        [Test]
        public void InverseModuloBoundHandTest()
        {
            Assert.That(Mathematics.InverseModulo(long.MinValue, long.MaxValue),
                Is.EqualTo(Mathematics.InverseModulo(-1, long.MaxValue)));
            Assert.That(Mathematics.InverseModulo(long.MaxValue, long.MaxValue - 1), Is.EqualTo(1));
            Assert.That(Mathematics.InverseModulo(long.MaxValue - 1, long.MaxValue), Is.EqualTo(long.MaxValue - 1));
            Assert.That(Mathematics.InverseModulo(long.MaxValue / 2 + 1, long.MaxValue), Is.EqualTo(2));
        }

        [Test]
        public void InverseModuloInvalidArgumentTest()
        {
            Assert.Throws<ArgumentException>(() => Mathematics.InverseModulo(2, 0));
            Assert.Throws<ArgumentException>(() => Mathematics.InverseModulo(2, -1));
        }

        [Test]
        public void IsPrimeTest([Range(0, 1000)] int n)
        {
            Assert.That(Mathematics.IsPrime(n), Is.EqualTo(IsPrimeNaive(n)));
            var x = int.MaxValue - n;
            Assert.That(Mathematics.IsPrime(x), Is.EqualTo(IsPrimeNaive(x)));
        }

        [Test]
        public void IsPrimeOverThan1000Test()
        {
            for (var i = 1001; i < 10000; i++)
            {
                Assert.That(Mathematics.IsPrime(i), Is.EqualTo(IsPrimeNaive(i)));
                var x = int.MaxValue - i;
                Assert.That(Mathematics.IsPrime(x), Is.EqualTo(IsPrimeNaive(x)));
            }
        }

        [Test]
        public void PowerModuloTest([Range(1, 100)] int modulo)
        {
            for (var x = -100; x <= 100; x++)
            for (var n = 0; n <= 100; n++)
                Assert.That(Mathematics.PowerModulo(x, n, modulo), Is.EqualTo(PowerModuloNaive(x, n, modulo)));
        }

        [Test]
        public void PowerModuloInvalidArgumentTest()
        {
            Assert.DoesNotThrow(() => Mathematics.PowerModulo(2, 2, 11));
            Assert.Throws<ArgumentException>(() => Mathematics.PowerModulo(2, -1, 11));
            Assert.Throws<ArgumentException>(() => Mathematics.PowerModulo(2, 2, 0));
            Assert.Throws<ArgumentException>(() => Mathematics.PowerModulo(2, -1, 0));
        }

        [Test]
        public void PrimitiveRootTest([Range(2, 1000)] int m)
        {
            if (!Mathematics.IsPrime(m)) return;
            var n = Mathematics.PrimitiveRoot(m);
            Assert.That(n, Is.GreaterThanOrEqualTo(1));
            Assert.That(m, Is.GreaterThan(n));
            var x = 1L;
            for (var i = 1; i <= m - 2; i++)
            {
                x *= n;
                x %= m;
                Assert.That(x, Is.Not.EqualTo(1));
            }

            x *= n;
            x %= m;
            Assert.That(x, Is.EqualTo(1));
        }

        [Test]
        public void PrimitiveRootOverThan1000Test()
        {
            for (var m = 1001; m <= 10000; m++)
            {
                if (!Mathematics.IsPrime(m)) return;
                var n = Mathematics.PrimitiveRoot(m);
                Assert.That(n, Is.GreaterThanOrEqualTo(1));
                Assert.That(m, Is.GreaterThan(n));
                var x = 1L;
                for (var i = 1; i <= m - 2; i++)
                {
                    x *= n;
                    x %= m;
                    Assert.That(x, Is.Not.EqualTo(1));
                }

                x *= n;
                x %= m;
                Assert.That(x, Is.EqualTo(1));
            }
        }

        [Test]
        public void PrimitiveRootConstTest()
        {
            Assert.That(Mathematics.PrimitiveRoot(2), Is.EqualTo(1));
            Assert.That(Mathematics.PrimitiveRoot(167772161), Is.EqualTo(3));
            Assert.That(Mathematics.PrimitiveRoot(469762049), Is.EqualTo(3));
            Assert.That(Mathematics.PrimitiveRoot(998244353), Is.EqualTo(3));
            Assert.That(Mathematics.PrimitiveRoot(754974721), Is.EqualTo(11));
        }

        [Test]
        public void SafeModuloTest()
        {
            var list = new List<long>();
            for (var i = 0; i <= 10; i++)
            {
                list.Add(i);
                list.Add(-i);
                list.Add(long.MinValue + i);
                list.Add(long.MinValue - i);
            }

            foreach (var a in list)
            foreach (var b in list)
            {
                if (b <= 0) continue;
                var value = ((ulong)(a % b) + (ulong)b) % (ulong)b;
                Assert.That(value, Is.EqualTo(Mathematics.SafeModulo(a, b)));
            }
        }

        private static long GreatestCommonDivisor(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                (a, b) = (b, a % b);
            }
        }

        private static long FloorSumNaive(long n, long m, long a, long b)
        {
            var sum = 0L;
            for (var i = 0L; i < n; i++) sum += (a * i + b) / m;
            return sum;
        }

        private static bool IsPrimeNaive(long n)
        {
            if (n == 0 || n == 1) return false;
            for (var i = 2L; i * i <= n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }

        private static long PowerModuloNaive(long x, long n, long modulo)
        {
            var y = Mathematics.SafeModulo(x, modulo);
            var z = 1L % modulo;
            for (var i = 0L; i < n; i++) z = z * y % modulo;
            return z % modulo;
        }
    }
}