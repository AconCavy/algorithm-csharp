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
            const int modUpper = int.MaxValue;
            for (var mod = modUpper; mod >= modUpper - 20; mod--)
            {
                ModuloInteger.SetMod(mod);
                var values = new List<long>();
                for (var i = 0; i < 10; i++)
                {
                    values.Add(i);
                    values.Add(mod - i);
                    values.Add(mod / 2 + i);
                    values.Add(mod / 2 - i);
                }

                foreach (var a in values)
                {
                    Assert.That(new ModuloInteger(a).Power(3).Value, Is.EqualTo(a * a % mod * a % mod));
                    foreach (var b in values)
                    {
                        ModuloInteger l = a;
                        ModuloInteger r = b;
                        Assert.That((l + r).Value, Is.EqualTo((a + b) % mod));
                        Assert.That((l - r).Value, Is.EqualTo((a - b + mod) % mod));
                        Assert.That((l * r).Value, Is.EqualTo(a * b % mod));
                    }
                }
            }
        }

        [Test]
        public void Mod1Test()
        {
            ModuloInteger.SetMod(1);
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    Assert.That((i * (ModuloInteger) j).Value, Is.Zero);
                }
            }

            ModuloInteger l = 1234;
            ModuloInteger r = 5678;
            Assert.That((l + r).Value, Is.Zero);
            Assert.That((l - r).Value, Is.Zero);
            Assert.That((l * r).Value, Is.Zero);
            Assert.That(l.Power(r.Value).Value, Is.Zero);
            Assert.That(l.Inverse().Value, Is.Zero);
        }

        [Test]
        public void InverseTest()
        {
            var n = 11;
            ModuloInteger.SetMod(n);
            for (var i = 1; i < n - 1; i++)
            {
                var x = ModuloInteger.Inverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = 12;
            ModuloInteger.SetMod(n);
            for (var i = 1; i < n - 1; i++)
            {
                if (GCD(i, n) != 1) continue;
                var x = ModuloInteger.Inverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = (int) 1e9 + 7;
            ModuloInteger.SetMod1000000007();
            for (var i = 1; i < 100000; i++)
            {
                var x = ModuloInteger.Inverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = 1000000008;
            ModuloInteger.SetMod(n);
            for (var i = 1; i < 100000; i++)
            {
                if (GCD(i, n) != 1) continue;
                var x = ModuloInteger.Inverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            ModuloInteger.SetMod998244353();
            for (var i = 1; i < 100000; i++)
            {
                var x = ModuloInteger.Inverse(i);
                Assert.That(x.Value, Is.GreaterThanOrEqualTo(0));
                Assert.That(x.Value, Is.LessThanOrEqualTo(998244353 - 1));
                Assert.That(x.Value * i % 998244353, Is.EqualTo(1));
            }
        }

        [Test]
        public void IncrementTest()
        {
            ModuloInteger.SetMod(11);
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
            ModuloInteger.SetMod998244353();
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(998244353));
            Assert.That((new ModuloInteger(1) + new ModuloInteger(2)).Value, Is.EqualTo(3));
            Assert.That((1L + new ModuloInteger(2)).Value, Is.EqualTo(3));
            Assert.That((new ModuloInteger(1) + 2L).Value, Is.EqualTo(3));

            ModuloInteger.SetMod1000000007();
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(1000000007));

            ModuloInteger.SetMod(3);
            Assert.That(ModuloInteger.Modulo, Is.EqualTo(3));
            Assert.That((new ModuloInteger(2) - new ModuloInteger(1)).Value, Is.EqualTo(1));
            Assert.That((2L - new ModuloInteger(1)).Value, Is.EqualTo(1));
            Assert.That((new ModuloInteger(2) - 1L).Value, Is.EqualTo(1));
            Assert.That((new ModuloInteger(1) + new ModuloInteger(2)).Value, Is.EqualTo(0));

            ModuloInteger.SetMod(11);
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
        public void CastIntTest()
        {
            ModuloInteger.SetMod(11);
            Assert.That((int) new ModuloInteger(1), Is.EqualTo(1));
            Assert.That((int) new ModuloInteger(12), Is.EqualTo(1));
            Assert.That((ModuloInteger) 1, Is.EqualTo(new ModuloInteger(1)));
            Assert.That((ModuloInteger) 12, Is.EqualTo(new ModuloInteger(1)));
        }

        [Test]
        public void IntOperatorsTest()
        {
            ModuloInteger.SetMod(11);
            Assert.That(new ModuloInteger(1) + 1, Is.EqualTo(new ModuloInteger(2)));
            Assert.That(new ModuloInteger(11) - 1, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(2) * 5, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(10) / 2, Is.EqualTo(new ModuloInteger(5)));
            Assert.That(10 / new ModuloInteger(5), Is.EqualTo(new ModuloInteger(2)));
        }

        [Test]
        public void CastLongTest()
        {
            ModuloInteger.SetMod(11);
            Assert.That((long) new ModuloInteger(1L), Is.EqualTo(1));
            Assert.That((long) new ModuloInteger(12L), Is.EqualTo(1));
            Assert.That((ModuloInteger) 1L, Is.EqualTo(new ModuloInteger(1)));
            Assert.That((ModuloInteger) 12L, Is.EqualTo(new ModuloInteger(1)));
        }

        [Test]
        public void LongOperatorsTest()
        {
            ModuloInteger.SetMod(11);
            Assert.That(new ModuloInteger(1) + 1L, Is.EqualTo(new ModuloInteger(2)));
            Assert.That(new ModuloInteger(11) - 1L, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(2) * 5L, Is.EqualTo(new ModuloInteger(10)));
            Assert.That(new ModuloInteger(10) / 2L, Is.EqualTo(new ModuloInteger(5)));
            Assert.That(10L / new ModuloInteger(5), Is.EqualTo(new ModuloInteger(2)));
        }

        private long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    }
}