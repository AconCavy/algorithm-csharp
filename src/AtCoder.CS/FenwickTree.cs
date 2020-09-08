using System;

namespace AtCoder.CS
{
    /// <summary>
    /// Reference: https://en.wikipedia.org/wiki/Fenwick_tree
    /// </summary>
    public class FenwickTree
    {
        private readonly int _n;
        private readonly long[] _data;

        public FenwickTree(int n = 0)
        {
            _n = n;
            _data = new long[n];
        }

        public void Add(int p, long x)
        {
            if (p < 0 || _n <= p) throw new ArgumentException(nameof(p));
            p++;
            while (p <= _n)
            {
                _data[p - 1] += x;
                p += p & -p;
            }
        }

        public long Sum(int l, int r)
        {
            if (0 > l || l > r || r > _n) throw new ArgumentException();
            return Sum(r) - Sum(l);
        }

        private long Sum(int r)
        {
            var s = 0L;
            while (r > 0)
            {
                s += _data[r - 1];
                r -= r & -r;
            }

            return s;
        }
    }
}