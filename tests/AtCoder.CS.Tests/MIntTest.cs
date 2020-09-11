using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AtCoder.CS.Tests
{
    public class MIntTest
    {
        [Test]
        public void DynamicBorderTest()
        {
            const int modUpper = int.MaxValue;
            for (var mod = modUpper; mod >= modUpper - 20; mod--)
            {
                MInt.SetMod(mod);
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
                    Assert.That(new MInt(a).Power(3).Value, Is.EqualTo(a * a % mod * a % mod));
                    foreach (var b in values)
                    {
                        MInt l = a;
                        MInt r = b;
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
            MInt.SetMod(1);
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    Assert.That(((MInt) i * (MInt) j).Value, Is.Zero);
                }
            }

            MInt l = 1234;
            MInt r = 5678;
            Assert.That((l + r).Value, Is.Zero);
            Assert.That((l - r).Value, Is.Zero);
            Assert.That((l * r).Value, Is.Zero);
            Assert.That(l.Power(r.Value).Value, Is.Zero);
            Assert.That(l.GetInverse(), Is.Zero);
        }

        [Test]
        public void InverseTest()
        {
            var n = 11;
            MInt.SetMod(n);
            for (var i = 1; i < n - 1; i++)
            {
                var x = MInt.GetInverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = 12;
            MInt.SetMod(n);
            for (var i = 1; i < n - 1; i++)
            {
                if (GCD(i, n) != 1) continue;
                var x = MInt.GetInverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = (int) 1e9 + 7;
            MInt.SetMod1000000007();
            for (var i = 1; i < 100000; i++)
            {
                var x = MInt.GetInverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            n = 1000000008;
            MInt.SetMod(n);
            for (var i = 1; i < 100000; i++)
            {
                if (GCD(i, n) != 1) continue;
                var x = MInt.GetInverse(i);
                Assert.That(x * i % n, Is.EqualTo(1));
            }

            MInt.SetMod998244353();
            for (var i = 1; i < 100000; i++)
            {
                var x = MInt.GetInverse(i);
                Assert.That(x, Is.GreaterThanOrEqualTo(0));
                Assert.That(x, Is.LessThanOrEqualTo(998244353 - 1));
                Assert.That(x * i % 998244353, Is.EqualTo(1));
            }
        }

        [Test]
        public void IncrementTest()
        {
            MInt.SetMod(11);
            MInt a = 8;
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
            MInt.SetMod998244353();
            Assert.That(MInt.Mod, Is.EqualTo(998244353));
            Assert.That((new MInt(1) + new MInt(2)).Value, Is.EqualTo(3));
            Assert.That((1L + new MInt(2)).Value, Is.EqualTo(3));
            Assert.That((new MInt(1) + 2L).Value, Is.EqualTo(3));

            MInt.SetMod1000000007();
            Assert.That(MInt.Mod, Is.EqualTo(1000000007));

            MInt.SetMod(3);
            Assert.That(MInt.Mod, Is.EqualTo(3));
            Assert.That((new MInt(2) - new MInt(1)).Value, Is.EqualTo(1));
            Assert.That((2L - new MInt(1)).Value, Is.EqualTo(1));
            Assert.That((new MInt(2) - 1L).Value, Is.EqualTo(1));
            Assert.That((new MInt(1) + new MInt(2)).Value, Is.EqualTo(0));

            MInt.SetMod(11);
            Assert.That(MInt.Mod, Is.EqualTo(11));
            Assert.That(new MInt(4).Value, Is.EqualTo(4));
            Assert.That(new MInt(-4).Value, Is.EqualTo(7));
            Assert.That((new MInt(3) * new MInt(5)).Value, Is.EqualTo(4));
            Assert.That(new MInt(1) == new MInt(3), Is.False);
            Assert.That(new MInt(1) != new MInt(3), Is.True);
            Assert.That(new MInt(1) == new MInt(12), Is.True);
            Assert.That(new MInt(1) != new MInt(12), Is.False);

            Assert.That(new MInt(1).ToString(), Is.EqualTo("1"));
            Assert.Throws<ArgumentException>(() => new MInt(3).Power(-1));
        }

        private long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    }
}