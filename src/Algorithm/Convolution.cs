using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Algorithm
{
    public static class Convolution
    {
        private static ModuloInteger[] _sumE;
        private static ModuloInteger[] _sumIe;
        private static long _previousModulo;

        public static ModuloInteger[] Execute(IEnumerable<ModuloInteger> source1, IEnumerable<ModuloInteger> source2)
        {
            if (source1 is null) throw new ArgumentNullException(nameof(source1));
            if (source2 is null) throw new ArgumentNullException(nameof(source2));

            var (a, b) = (source1.ToArray(), source2.ToArray());
            if (a.Length == 0 || b.Length == 0) return Array.Empty<ModuloInteger>();
            if (Math.Min(a.Length, b.Length) <= 60) return Naive(a, b);

            var length = a.Length + b.Length - 1;
            var z = 1 << CeilLog2(length);
            if (ModuloInteger.Modulo != _previousModulo) Initialize();
            Array.Resize(ref a, z);
            Butterfly(a);
            Array.Resize(ref b, z);
            Butterfly(b);
            var result = new ModuloInteger[z];
            for (var i = 0; i < z; i++)
            {
                result[i] = a[i] * b[i];
            }

            ButterflyInverse(result);
            Array.Resize(ref result, length);
            var iz = ModuloInteger.Inverse(z);
            for (var i = 0; i < result.Length; i++)
            {
                result[i] *= iz;
            }

            return result;
        }

        public static long[] Execute(IEnumerable<long> source1, IEnumerable<long> source2)
        {
            if (source1 is null) throw new ArgumentNullException(nameof(source1));
            if (source2 is null) throw new ArgumentNullException(nameof(source2));

            var (a, b) = (source1.ToArray(), source2.ToArray());
            if (a.Length == 0 || b.Length == 0) return Array.Empty<long>();
            if (Math.Min(a.Length, b.Length) <= 60) return Naive(a, b);

            var result = new long[a.Length + b.Length - 1];
            unchecked
            {
                const long mod1 = 754974721;
                const long mod2 = 167772161;
                const long mod3 = 469762049;
                const long m23 = mod2 * mod3;
                const long m13 = mod1 * mod3;
                const long m12 = mod1 * mod2;
                const ulong m123 = (ulong)mod1 * (ulong)mod2 * (ulong)mod3;

                var i1 = (ulong)Mathematics.InverseGreatestCommonDivisor(m23, mod1).im;
                var i2 = (ulong)Mathematics.InverseGreatestCommonDivisor(m13, mod2).im;
                var i3 = (ulong)Mathematics.InverseGreatestCommonDivisor(m12, mod3).im;

                ModuloInteger.Modulo = mod1;
                var c1 = Execute(a.Select(x => (ModuloInteger)x), b.Select(x => (ModuloInteger)x));
                ModuloInteger.Modulo = mod2;
                var c2 = Execute(a.Select(x => (ModuloInteger)x), b.Select(x => (ModuloInteger)x));
                ModuloInteger.Modulo = mod3;
                var c3 = Execute(a.Select(x => (ModuloInteger)x), b.Select(x => (ModuloInteger)x));
                for (var i = 0; i < result.Length; i++)
                {
                    var x = 0UL;
                    x += (ulong)c1[i].Value * i1 % mod1 * m23;
                    x += (ulong)c2[i].Value * i2 % mod2 * m13;
                    x += (ulong)c3[i].Value * i3 % mod3 * m12;
                    var tmp = x % mod1;
                    var diff = (long)c1[i] - Mathematics.SafeModulo((long)tmp, mod1);
                    if (diff < 0) diff += mod1;
                    var offset = new[] { 0UL, 0UL, m123, m123 * 2, m123 * 3 };
                    x -= offset[diff % 5];
                    result[i] = (long)x;
                }
            }

            return result;
        }

        private static void Initialize()
        {
            _previousModulo = ModuloInteger.Modulo;
            _sumE = new ModuloInteger[30];
            _sumIe = new ModuloInteger[30];
            var es = new ModuloInteger[30];
            var ies = new ModuloInteger[30];
            var bit = BitScanForward(_previousModulo - 1);
            var e = ModuloInteger.Power(Mathematics.PrimitiveRoot(_previousModulo), (_previousModulo - 1) >> bit);
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

        private static void Butterfly(Span<ModuloInteger> items)
        {
            var h = CeilLog2(items.Length);
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

        private static void ButterflyInverse(Span<ModuloInteger> items)
        {
            var h = CeilLog2(items.Length);
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

        private static ModuloInteger[] Naive(ReadOnlySpan<ModuloInteger> source1, ReadOnlySpan<ModuloInteger> source2)
        {
            var length = source1.Length + source2.Length - 1;
            var result = new ModuloInteger[length];
            for (var i = 0; i < source1.Length; i++)
            {
                for (var j = 0; j < source2.Length; j++)
                {
                    result[i + j] += source1[i] * source2[j];
                }
            }

            return result;
        }

        private static long[] Naive(ReadOnlySpan<long> source1, ReadOnlySpan<long> source2)
        {
            var length = source1.Length + source2.Length - 1;
            var result = new long[length];
            for (var i = 0; i < source1.Length; i++)
            {
                for (var j = 0; j < source2.Length; j++)
                {
                    result[i + j] += source1[i] * source2[j];
                }
            }

            return result;
        }

        private static int BitScanForward(long n) => n == 0 ? 0 : BitOperations.TrailingZeroCount(n);
        private static int CeilLog2(int n) => (int)Math.Ceiling(Math.Log2(n));
    }
}