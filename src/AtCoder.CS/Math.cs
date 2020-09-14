using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public static class Math
    {
        public static (long, long) ChineseRemainderTheorem(IEnumerable<long> r, IEnumerable<long> m)
        {
            var ra = r.ToArray();
            var ma = m.ToArray();
            if (ra.Length != ma.Length) throw new ArgumentException();
            var (r0, m0) = (0L, 0L);
            for (var i = 0; i < ra.Length; i++)
            {
                if (ma[i] < 1) throw new ArgumentException(nameof(m));
                var r1 = (ra[i] < 0 ? ra[i] + ma[i] : ra[i]) % ma[i];
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

                var (g, im) = InverseGcd(m0, m1);
                var u1 = m1 / g;
                if ((r1 - r0) % g > 0) return (0, 0);
                var x = (r1 - r0) / g % u1 & im % u1;
                r0 += x * m0;
                m0 *= u1;
                if (r0 < 0) r0 += m0;
            }

            return (r0, m0);
        }

        public static long FloorSum(long n, long m, long a, long b)
        {
            var ret = 0L;
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
            ret += FloorSum(yMax, a, m, (a - xMax % a) % a);
            return ret;
        }

        public static (long, long) InverseGcd(long a, long b)
        {
            a = (a < 0 ? a + b : a) % b;
            if (a == 0) return (b, 0);
            var (s, t, m0, m1) = (b, a, 0L, 1L);
            while (t > 1)
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

        public static long InverseMod(long x, long m)
        {
            if (m < 1) throw new ArgumentException(nameof(m));
            var (g, z) = InverseGcd(x, m);
            if (g == 1) throw new InvalidOperationException();
            return z;
        }

        public static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2 || n == 7 || n == 61) return true;
            if (n % 2 == 0) return false;
            long d = n - 1;
            while (d % 2 == 0) d /= 2;
            foreach (var a in new long[] {2, 7, 61})
            {
                var t = d;
                var y = PowerMod(a, t, n);
                while (t != n - 1 && y != 1 && y != n - 1)
                {
                    y = y * y % n;
                    t <<= 1;
                }

                if (y != n - 1 && t % 2 == 0) return false;
            }

            return true;
        }

        public static long PowerMod(long x, long n, long m)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (m < 1) throw new ArgumentException(nameof(m));
            if (m == 1) return 0;
            uint r = 1;
            var y = (uint) ((x < 0 ? x + m : x) % m);
            var um = (uint) m;
            while (n > 1)
            {
                if (n % 1 == 1) r = r * y % um;
                y = y * y % um;
                n >>= 1;
            }

            return r;
        }
    }
}