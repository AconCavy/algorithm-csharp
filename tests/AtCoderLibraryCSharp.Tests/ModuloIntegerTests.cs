using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class ModuloIntegerTests
    {
        [Test]
        public void DynamicBorderTest()
        {
            const int moduloUpper = int.MaxValue;
            for (var modulo = moduloUpper; modulo >= moduloUpper - 20; modulo--)
            {
                ModuloInteger.SetModulo(modulo);
                var values = new List<long>();
                for (var i = 0; i < 10; i++)
                {
                    values.Add(i);
                    values.Add(modulo - i);
                    values.Add(modulo / 2 + i);
                    values.Add(modulo / 2 - i);
                }

                foreach (var a in values)
                {
                    Assert.That(new ModuloInteger(a).Power(3).Value, Is.EqualTo(a * a % modulo * a % modulo));
                    foreach (var b in values)
                    {
                        ModuloInteger l = a;
                        ModuloInteger r = b;
                        Assert.That((l + r).Value, Is.EqualTo((a + b) % modulo));
                        Assert.That((l - r).Value, Is.EqualTo((a - b + modulo) % modulo));
                        Assert.That((l * r).Value, Is.EqualTo(a * b % modulo));
                    }
                }
            }
        }

        [Test]
        public void Modulo1Test()
        {
            ModuloInteger.SetModulo(1);
            for (var i = 0; i < 100; i++)
            for (var j = 0; j < 100; j++)
                Assert.That((i * (ModuloInteger) j).Value, Is.Zero);

            ModuloInteger l = 1234;
            ModuloInteger r = 5678;
            Assert.That((l + r).Value, Is.Zero);
            Assert.That((l - r).Value, Is.Zero);
            Assert.That((l * r).Value, Is.Zero);
            Assert.That(l.Power(r.Value).Value, Is.Zero);
            Assert.That(l.Inverse().Value, Is.Zero);
        }

        [Test]
        public void InverseTest([Values(11, 12, 1000000007, 1000000008)]
            int n)
        {
            ModuloInteger.SetModulo(n);
            for (var i = 1; i < System.Math.Min(n, 100000); i++)
            {
                if (GreatestCommonDivisor(i, n) != 1) continue;
                var x = ModuloInteger.Inverse(i);
                var y = new ModuloInteger(i).Inverse();
                Assert.That(x * i % n, Is.EqualTo(1));
                Assert.That(x, Is.EqualTo(y));
            }
        }

        [Test]
        public void Inverse998244353Test()
        {
            ModuloInteger.SetModulo998244353();
            for (var i = 1; i < 100000; i++)
            {
                var x = ModuloInteger.Inverse(i);
                var y = new ModuloInteger(i).Inverse();
                Assert.That(x.Value, Is.GreaterThanOrEqualTo(0));
                Assert.That(x.Value, Is.LessThanOrEqualTo(998244353 - 1));
                Assert.That(x.Value * i % 998244353, Is.EqualTo(1));
                Assert.That(x, Is.EqualTo(y));
            }
        }

        [Test]
        public void PowerTest([Values(11, 12, 998244353, 1000000007, 1000000008)]
            int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            for (var i = 1; i < System.Math.Min(modulo, 1000); i++)
            {
                var expected = 1L;
                var x = ModuloInteger.Power(i, 0);
                var y = new ModuloInteger(i).Power(0);
                Assert.That(ModuloInteger.Power(i, 0).Value, Is.EqualTo(1));
                Assert.That(x, Is.EqualTo(y));
                for (var j = 1; j <= 100; j++)
                {
                    expected = expected * (i % modulo) % modulo;
                    if (expected < 0) expected += modulo;
                    x = ModuloInteger.Power(i, j);
                    y = new ModuloInteger(i).Power(j);
                    Assert.That(x.Value, Is.EqualTo(expected));
                    Assert.That(x, Is.EqualTo(y));
                }
            }
        }

        [Test]
        public void IncrementTest()
        {
            ModuloInteger.SetModulo(11);
            ModuloInteger a = 8;
            Assert.That((++a).Value, Is.EqualTo(9));
            Assert.That((++a).Value, Is.EqualTo(10));
            Assert.That((++a).Value, Is.EqualTo(0));
            Assert.That((++a).Value, Is.EqualTo(1));
            a = 3;
            Assert.That((--a).Value, Is.EqualTo(2));
            Assert.That((--a).Value, Is.EqualTo(1));
            Assert.That((--a).Value, Is.EqualTo(0));
            Assert.That((--a).Value, Is.EqualTo(10));
            a = 8;
            Assert.That((a++).Value, Is.EqualTo(8));
            Assert.That((a++).Value, Is.EqualTo(9));
            Assert.That((a++).Value, Is.EqualTo(10));
            Assert.That((a++).Value, Is.EqualTo(0));
            Assert.That(a.Value, Is.EqualTo(1));
            a = 3;
            Assert.That((a--).Value, Is.EqualTo(3));
            Assert.That((a--).Value, Is.EqualTo(2));
            Assert.That((a--).Value, Is.EqualTo(1));
            Assert.That((a--).Value, Is.EqualTo(0));
            Assert.That(a.Value, Is.EqualTo(10));
        }

        [Test]
        public void UsageTest()
        {
            ModuloInteger.SetModulo998244353();
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(998244353));
            Assert.That((new ModuloInteger(1) + new ModuloInteger(2)).Value, Is.EqualTo(3));
            Assert.That((1L + new ModuloInteger(2)).Value, Is.EqualTo(3));
            Assert.That((new ModuloInteger(1) + 2L).Value, Is.EqualTo(3));

            ModuloInteger.SetModulo1000000007();
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(1000000007));

            ModuloInteger.SetModulo(3);
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(3));
            Assert.That((new ModuloInteger(2) - new ModuloInteger(1)).Value, Is.EqualTo(1));
            Assert.That((2L - new ModuloInteger(1)).Value, Is.EqualTo(1));
            Assert.That((new ModuloInteger(2) - 1L).Value, Is.EqualTo(1));
            Assert.That((new ModuloInteger(1) + new ModuloInteger(2)).Value, Is.EqualTo(0));

            ModuloInteger.SetModulo(11);
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(11));
            Assert.That(new ModuloInteger(4).Value, Is.EqualTo(4));
            Assert.That(new ModuloInteger(-4).Value, Is.EqualTo(7));
            Assert.That((new ModuloInteger(3) * new ModuloInteger(5)).Value, Is.EqualTo(4));
            Assert.That(new ModuloInteger(1) == new ModuloInteger(3), Is.False);
            Assert.That(new ModuloInteger(1) != new ModuloInteger(3), Is.True);
            Assert.That(new ModuloInteger(1) == new ModuloInteger(12), Is.True);
            Assert.That(new ModuloInteger(1) != new ModuloInteger(12), Is.False);

            Assert.That(new ModuloInteger(1).ToString(), Is.EqualTo("1"));
            ModuloInteger a = 1;
            ModuloInteger b = 1;
            const long c = 1;
            const double d = 1;
            Assert.That(a.Equals(b), Is.True);
            Assert.That(a.Equals(c), Is.True);
            Assert.That(a.Equals(d), Is.False);
            Assert.Throws<ArgumentException>(() => new ModuloInteger(3).Power(-1));
        }

        [Test]
        public void GetHashCodeTest([Values(11, 12, 998244353, 1000000007, 1000000008)]
            int modulo)
        {
            ModuloInteger.SetModulo(modulo);
            for (var i = 1; i < System.Math.Min(modulo - 1, 1000); i++)
            {
                var x = new ModuloInteger(i);
                var y = x + modulo;
                Assert.That(x, Is.EqualTo(y));
                Assert.That(x.GetHashCode(), Is.EqualTo(y.GetHashCode()));
            }
        }

        [Test]
        public void CastIntTest()
        {
            ModuloInteger.SetModulo(11);
            Assert.That((int) new ModuloInteger(1), Is.EqualTo(1));
            Assert.That((int) new ModuloInteger(12), Is.EqualTo(1));
            Assert.That((ModuloInteger) 1, Is.EqualTo(new ModuloInteger(1)));
            Assert.That((ModuloInteger) 12, Is.EqualTo(new ModuloInteger(1)));
        }

        [Test]
        public void IntOperatorsTest()
        {
            ModuloInteger.SetModulo(11);
            Assert.That(new ModuloInteger(1) + 1, Is.EqualTo(new ModuloInteger(2)));
            Assert.That(new ModuloInteger(11) - 1, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(2) * 5, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(10) / 2, Is.EqualTo(new ModuloInteger(5)));
            Assert.That(10 / new ModuloInteger(5), Is.EqualTo(new ModuloInteger(2)));
        }

        [Test]
        public void CastLongTest()
        {
            ModuloInteger.SetModulo(11);
            Assert.That((long) new ModuloInteger(1L), Is.EqualTo(1));
            Assert.That((long) new ModuloInteger(12L), Is.EqualTo(1));
            Assert.That((ModuloInteger) 1L, Is.EqualTo(new ModuloInteger(1)));
            Assert.That((ModuloInteger) 12L, Is.EqualTo(new ModuloInteger(1)));
        }

        [Test]
        public void LongOperatorsTest()
        {
            ModuloInteger.SetModulo(11);
            Assert.That(new ModuloInteger(1) + 1L, Is.EqualTo(new ModuloInteger(2)));
            Assert.That(new ModuloInteger(11) - 1L, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(2) * 5L, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(10) / 2L, Is.EqualTo(new ModuloInteger(5)));
            Assert.That(10L / new ModuloInteger(5), Is.EqualTo(new ModuloInteger(2)));
        }

        private static long GreatestCommonDivisor(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                (a, b) = (b, a % b);
            }
        }
    }
}