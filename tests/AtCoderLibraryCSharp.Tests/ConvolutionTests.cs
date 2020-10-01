using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class ConvolutionTests
    {
        [Test]
        public void EmptyTest()
        {
            ModuloInteger.SetMod998244353();
            Assert.That(Convolution.Execute(new ModuloInteger[0], new ModuloInteger[0]),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new ModuloInteger[0], new ModuloInteger[] {1, 2}),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new ModuloInteger[] {1, 2}, new ModuloInteger[0]),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new ModuloInteger[] {1}, new ModuloInteger[0]),
                Is.EquivalentTo(new ModuloInteger[0]));

            Assert.That(Convolution.Execute(new long[0], new long[0]),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new long[0], new long[] {1, 2}),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new long[] {1, 2}, new long[0]),
                Is.EquivalentTo(new ModuloInteger[0]));
            Assert.That(Convolution.Execute(new long[] {1}, new long[0]),
                Is.EquivalentTo(new long[0]));
        }

        [Test]
        public void MiddleTest()
        {
            ModuloInteger.SetMod998244353();
            var random = new Random(19937);
            var a = new ModuloInteger[1234].Select(_ => (ModuloInteger) random.Next()).ToArray();
            var b = new ModuloInteger[2345].Select(_ => (ModuloInteger) random.Next()).ToArray();

            Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
        }

        [Test]
        public void SimpleModuloIntegerTest()
        {
            var mods = new[] {998244353, 924844033};
            var random = new Random(19937);
            foreach (var mod in mods)
            {
                ModuloInteger.SetMod(mod);
                for (var n = 1; n < 20; n++)
                for (var m = 1; m < 20; m++)
                {
                    var a = new ModuloInteger[n].Select(_ => (ModuloInteger) random.Next()).ToArray();
                    var b = new ModuloInteger[m].Select(_ => (ModuloInteger) random.Next()).ToArray();
                    Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
                }
            }
        }

        [Test]
        public void SimpleLongTest()
        {
            var random = new Random(19937);
            for (var n = 1; n < 20; n++)
            for (var m = 1; m < 20; m++)
            {
                var a = new long[n].Select(_ => random.Next() % (long) 1e6 - (long) 5e5).ToArray();
                var b = new long[m].Select(_ => random.Next() % (long) 1e6 - (long) 5e5).ToArray();
                Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
            }
        }

        [Test]
        public void LongBoundTest()
        {
            const long mod1 = 754974721;
            const long mod2 = 167772161;
            const long mod3 = 469762049;
            const long m23 = mod2 * mod3;
            const long m13 = mod1 * mod3;
            const long m12 = mod1 * mod2;

            for (var i = -1000; i <= 1000; i++)
            {
                var a = new[] {-m12 - m13 - m23 + i};
                var b = new[] {1L};
                Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
            }

            for (var i = 0; i <= 1000; i++)
            {
                var a = new[] {long.MinValue + i};
                var b = new[] {1L};
                Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
            }

            for (var i = 0; i <= 1000; i++)
            {
                var a = new[] {long.MaxValue + i};
                var b = new[] {1L};
                Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
            }
        }

        [Test]
        public void Mod641Test()
        {
            const int mod = 641;
            ModuloInteger.SetMod(mod);
            var a = new ModuloInteger[64].Select(_ => (ModuloInteger) Utilities.RandomInteger(0, mod - 1)).ToArray();
            var b = new ModuloInteger[65].Select(_ => (ModuloInteger) Utilities.RandomInteger(0, mod - 1)).ToArray();
            Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
        }

        [Test]
        public void Mod18433Test()
        {
            const int mod = 18433;
            ModuloInteger.SetMod(mod);
            var a = new ModuloInteger[1024].Select(_ => (ModuloInteger) Utilities.RandomInteger(0, mod - 1)).ToArray();
            var b = new ModuloInteger[1025].Select(_ => (ModuloInteger) Utilities.RandomInteger(0, mod - 1)).ToArray();
            Assert.That(Convolution.Execute(a, b), Is.EquivalentTo(ConvolutionNaive(a, b)));
        }

        private static ModuloInteger[] ConvolutionNaive(ModuloInteger[] a, ModuloInteger[] b)
        {
            var n = a.Length;
            var m = b.Length;
            if (n < 1 || m < 1) return new ModuloInteger[0];
            var ret = new ModuloInteger[n + m - 1];
            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
                ret[i + j] += a[i] * b[j];
            return ret;
        }

        private static long[] ConvolutionNaive(long[] a, long[] b)
        {
            var n = a.Length;
            var m = b.Length;
            if (n < 1 || m < 1) return new long[0];
            var ret = new long[n + m - 1];
            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
                ret[i + j] += a[i] * b[j];
            return ret;
        }
    }
}