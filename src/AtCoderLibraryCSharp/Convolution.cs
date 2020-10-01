using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public static class Convolution
    {
        private static ModuloInteger[] _sumE;
        private static ModuloInteger[] _sumIe;
        private static int _primitiveRoot;
        private static long _modulo;

        public static IEnumerable<ModuloInteger> Execute(IEnumerable<ModuloInteger> a, IEnumerable<ModuloInteger> b)
        {
            var (a1, b1) = (a.ToArray(), b.ToArray());
            var (n, m) = (a1.Length, b1.Length);
            if (n < 1 || m < 1) return new ModuloInteger[0];
            var ret = new ModuloInteger[n + m - 1];
            if (System.Math.Min(n, m) <= 60)
            {
                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    ret[i + j] += a1[i] * b1[j];
                return ret;
            }

            var z = 1 << CeilPower2(n + m - 1);
            Array.Resize(ref a1, z);
            Array.Resize(ref b1, z);
            a1 = Butterfly(a1).ToArray();
            b1 = Butterfly(b1).ToArray();
            for (var i = 0; i < a1.Length; i++) a1[i] *= b1[i];
            ret = ButterflyInverse(a1).ToArray();
            Array.Resize(ref ret, n + m - 1);
            var iz = ModuloInteger.Inverse(z);
            return ret.Select(x => x * iz);
        }

        private static void Initialize()
        {
            if (ModuloInteger.Modulo != _modulo)
            {
                _modulo = ModuloInteger.Modulo;
                _sumE = null;
                _sumIe = null;
            }

            if (_sumE != null && _sumIe != null) return;
            var m = ModuloInteger.Modulo;
            _primitiveRoot = Math.PrimitiveRoot(m);
            _sumE = new ModuloInteger[30];
            _sumIe = new ModuloInteger[30];
            var es = new ModuloInteger[30];
            var ies = new ModuloInteger[30];
            var count2 = BitScanForward(m - 1);
            var e = new ModuloInteger(_primitiveRoot).Power((m - 1) >> count2);
            var ie = e.Inverse();
            for (var i = count2; i >= 2; i--)
            {
                es[i - 2] = e;
                ies[i - 2] = ie;
                e *= e;
                ie *= ie;
            }

            ModuloInteger now = 1;
            ModuloInteger inow = 1;
            for (var i = 0; i <= count2 - 2; i++)
            {
                _sumE[i] = es[i] * now;
                _sumIe[i] = ies[i] * inow;
                now *= ies[i];
                inow *= es[i];
            }
        }

        private static IEnumerable<ModuloInteger> Butterfly(IEnumerable<ModuloInteger> items)
        {
            var ret = items.ToArray();
            var h = CeilPower2(ret.Length);
            Initialize();

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
                        var l = ret[i + offset];
                        var r = ret[i + offset + p] * now;
                        ret[i + offset] = l + r;
                        ret[i + offset + p] = l - r;
                    }

                    now *= _sumE[BitScanForward(~s)];
                }
            }

            return ret;
        }

        private static IEnumerable<ModuloInteger> ButterflyInverse(IEnumerable<ModuloInteger> items)
        {
            var ret = items.ToArray();
            var h = CeilPower2(ret.Length);
            Initialize();

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
                        var l = ret[i + offset];
                        var r = ret[i + offset + p];
                        ret[i + offset] = l + r;
                        ret[i + offset + p] = (l - r) * inow;
                    }

                    inow *= _sumIe[BitScanForward(~s)];
                }
            }

            return ret;
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
            while ((1 << x) < n) x++;
            return x;
        }
    }
}