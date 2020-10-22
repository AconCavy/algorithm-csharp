using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public static class Convolution
    {
        private static ModuloInteger[] _sumE;
        private static ModuloInteger[] _sumIe;
        private static long _modulo;

        public static IEnumerable<ModuloInteger> Execute(IEnumerable<ModuloInteger> a, IEnumerable<ModuloInteger> b)
        {
            var (a1, b1) = (a.ToArray(), b.ToArray());
            var (n, m) = (a1.Length, b1.Length);
            if (n < 1 || m < 1) return new ModuloInteger[0];
            ModuloInteger[] ret;
            if (System.Math.Min(n, m) <= 60)
            {
                ret = new ModuloInteger[n + m - 1];
                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    ret[i + j] += a1[i] * b1[j];
                return ret;
            }

            var z = 1 << CeilPower2(n + m - 1);
            Array.Resize(ref a1, z);
            Array.Resize(ref b1, z);
            Butterfly(a1);
            Butterfly(b1);
            ret = new ModuloInteger[z];
            for (var i = 0; i < z; i++) ret[i] = a1[i] * b1[i];
            ButterflyInverse(ret);
            var iz = ModuloInteger.Inverse(z);
            return ret.Take(n + m - 1).Select(x => x * iz);
        }

        public static IEnumerable<long> Execute(IEnumerable<long> a, IEnumerable<long> b)
        {
            var (a1, b1) = (a.ToArray(), b.ToArray());
            var (n, m) = (a1.Length, b1.Length);
            if (n < 1 || m < 1) return new long[0];
            var ret = new long[n + m - 1];
            unchecked
            {
                const long mod1 = 754974721;
                const long mod2 = 167772161;
                const long mod3 = 469762049;
                const long m23 = mod2 * mod3;
                const long m13 = mod1 * mod3;
                const long m12 = mod1 * mod2;
                const ulong m123 = (ulong) mod1 * (ulong) mod2 * (ulong) mod3;

                var i1 = (ulong) Math.InverseGreatestCommonDivisor(m23, mod1).im;
                var i2 = (ulong) Math.InverseGreatestCommonDivisor(m13, mod2).im;
                var i3 = (ulong) Math.InverseGreatestCommonDivisor(m12, mod3).im;

                ModuloInteger.SetModulo(mod1);
                var c1 = Execute(a1.Select(x => (ModuloInteger) x), b1.Select(x => (ModuloInteger) x)).ToArray();
                ModuloInteger.SetModulo(mod2);
                var c2 = Execute(a1.Select(x => (ModuloInteger) x), b1.Select(x => (ModuloInteger) x)).ToArray();
                ModuloInteger.SetModulo(mod3);
                var c3 = Execute(a1.Select(x => (ModuloInteger) x), b1.Select(x => (ModuloInteger) x)).ToArray();
                for (var i = 0; i < ret.Length; i++)
                {
                    var x = 0UL;
                    x += (ulong) c1[i].Value * i1 % mod1 * m23;
                    x += (ulong) c2[i].Value * i2 % mod2 * m13;
                    x += (ulong) c3[i].Value * i3 % mod3 * m12;
                    var tmp = x % mod1;
                    var diff = (long) c1[i] - Math.SafeModulo((long) tmp, mod1);
                    if (diff < 0) diff += mod1;
                    var offset = new[] {0UL, 0UL, m123, m123 * 2, m123 * 3};
                    x -= offset[diff % 5];
                    ret[i] = (long) x;
                }
            }

            return ret;
        }

        private static void Initialize()
        {
            _modulo = ModuloInteger.Modulo;
            _sumE = new ModuloInteger[30];
            _sumIe = new ModuloInteger[30];
            var es = new ModuloInteger[30];
            var ies = new ModuloInteger[30];
            var bit = BitScanForward(_modulo - 1);
            var e = ModuloInteger.Power(Math.PrimitiveRoot(_modulo), (_modulo - 1) >> bit);
            var ie = e.Inverse();
            for (var i = bit; i >= 2; i--)
            {
                es[i - 2] = e;
                ies[i - 2] = ie;
                e *= e;
                ie *= ie;
            }

            ModuloInteger now = 1;
            ModuloInteger inow = 1;
            for (var i = 0; i <= bit - 2; i++)
            {
                _sumE[i] = es[i] * now;
                _sumIe[i] = ies[i] * inow;
                now *= ies[i];
                inow *= es[i];
            }
        }

        private static void Butterfly(IList<ModuloInteger> items)
        {
            var h = CeilPower2(items.Count);
            if (ModuloInteger.Modulo != _modulo) Initialize();

            for (var ph = 1; ph <= h; ph++)
            {
                var w = 1 << (ph - 1);
                var p = 1 << (h - ph);
                ModuloInteger now = 1;
                for (var s = 0; s < w; s++)
                {
                    var offset = s << (h - ph + 1);
                    for (var i = 0; i < p; i++)
                    {
                        var l = items[i + offset];
                        var r = items[i + offset + p] * now;
                        items[i + offset] = l + r;
                        items[i + offset + p] = l - r;
                    }

                    now *= _sumE[BitScanForward(~s)];
                }
            }
        }

        private static void ButterflyInverse(IList<ModuloInteger> items)
        {
            var h = CeilPower2(items.Count);
            if (ModuloInteger.Modulo != _modulo) Initialize();

            for (var ph = h; ph >= 1; ph--)
            {
                var w = 1 << (ph - 1);
                var p = 1 << (h - ph);
                ModuloInteger inow = 1;
                for (var s = 0; s < w; s++)
                {
                    var offset = s << (h - ph + 1);
                    for (var i = 0; i < p; i++)
                    {
                        var l = items[i + offset];
                        var r = items[i + offset + p];
                        items[i + offset] = l + r;
                        items[i + offset + p] = (l - r) * inow;
                    }

                    inow *= _sumIe[BitScanForward(~s)];
                }
            }
        }

        private static int BitScanForward(long n)
        {
            if (n == 0) return 0;
            var x = 0;
            while ((n >> x & 1) == 0) x++;
            return x;
        }

        private static int CeilPower2(int n)
        {
            var x = 0;
            while (1 << x < n) x++;
            return x;
        }
    }
}