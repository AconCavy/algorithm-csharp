using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmSharp
{
    public static class Mathematics
    {
        public static (long rem, long mod) ChineseRemainderTheorem(IEnumerable<long> r, IEnumerable<long> m)
        {
            var ra = r.ToArray();
            var ma = m.ToArray();
            if (ra.Length != ma.Length) throw new ArgumentException();
            var (r0, m0) = (0L, 1L);
            for (var i = 0; i < ra.Length; i++)
            {
                if (ma[i] < 1) throw new ArgumentException(nameof(m));
                var r1 = SafeModulo(ra[i], ma[i]);
                var m1 = ma[i];
                if (m0 < m1)
                {
                    (r0, r1) = (r1, r0);
                    (m0, m1) = (m1, m0);
                }

                if (m0 % m1 == 0)
                {
                    if (r0 % m1 != r1) return (0, 0);
                    continue;
                }

                var (g, im) = InverseGreatestCommonDivisor(m0, m1);
                var u1 = m1 / g;
                if ((r1 - r0) % g != 0) return (0, 0);
                var x = (r1 - r0) / g % u1 * im % u1;
                r0 += x * m0;
                m0 *= u1;
                if (r0 < 0) r0 += m0;
            }

            return (r0, m0);
        }

        public static long FloorSum(long n, long m, long a, long b)
        {
            var ret = 0L;
            while (true)
            {
                if (a >= m)
                {
                    ret += (n - 1) * n * (a / m) / 2;
                    a %= m;
                }

                if (b >= m)
                {
                    ret += n * (b / m);
                    b %= m;
                }

                var yMax = (a * n + b) / m;
                var xMax = yMax * m - b;
                if (yMax == 0) return ret;
                ret += (n - (xMax + a - 1) / a) * yMax;
                (n, m, a, b) = (yMax, a, m, (a - xMax % a) % a);
            }
        }

        public static (long g, long im) InverseGreatestCommonDivisor(long a, long b)
        {
            a = SafeModulo(a, b);
            if (a == 0) return (b, 0);
            var (s, t, m0, m1) = (b, a, 0L, 1L);
            while (t > 0)
            {
                var u = s / t;
                s -= t * u;
                m0 -= m1 * u;
                (s, t) = (t, s);
                (m0, m1) = (m1, m0);
            }

            if (m0 < 0) m0 += b / s;
            return (s, m0);
        }

        public static long InverseModulo(long value, long modulo)
        {
            if (modulo < 1) throw new ArgumentException(nameof(modulo));
            var (rem, mod) = InverseGreatestCommonDivisor(value, modulo);
            if (rem != 1) throw new InvalidOperationException();
            return mod;
        }

        public static bool IsPrime(int value)
        {
            if (value <= 1) return false;
            if (value == 2 || value == 7 || value == 61) return true;
            if (value % 2 == 0) return false;
            long d = value - 1;
            while (d % 2 == 0) d /= 2;
            foreach (var a in new long[] {2, 7, 61})
            {
                var t = d;
                var y = PowerModulo(a, t, value);
                while (t != value - 1 && y != 1 && y != value - 1)
                {
                    y = y * y % value;
                    t <<= 1;
                }

                if (y != value - 1 && t % 2 == 0) return false;
            }

            return true;
        }

        public static long PowerModulo(long value, long n, long modulo)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (modulo < 1) throw new ArgumentException(nameof(modulo));
            if (modulo == 1) return 0;
            var ret = 1L;
            var y = SafeModulo(value, modulo);
            while (n > 0)
            {
                if ((n & 1) == 1) ret = ret * y % modulo;
                y = y * y % modulo;
                n >>= 1;
            }

            return ret;
        }

        public static int PrimitiveRoot(long m)
        {
            switch (m)
            {
                case 2:
                    return 1;
                case 167772161:
                case 469762049:
                case 998244353:
                    return 3;
                case 754974721:
                    return 11;
            }

            var divs = new long[20];
            divs[0] = 2;
            var count = 1;
            var x = (m - 1) / 2;
            while (x % 2 == 0) x /= 2;
            for (var i = 3L; i * i <= x; i += 2)
            {
                if (x % i != 0) continue;
                divs[count++] = i;
                while (x % i == 0) x /= i;
            }

            if (x > 1) divs[count++] = x;
            for (var g = 2;; g++)
            {
                var ok = true;
                for (var i = 0; i < count && ok; i++)
                    if (PowerModulo(g, (m - 1) / divs[i], m) == 1)
                        ok = false;

                if (ok) return g;
            }
        }

        public static long SafeModulo(long value, long modulo) =>
            value % modulo < 0 ? value % modulo + modulo : value % modulo;
    }
}