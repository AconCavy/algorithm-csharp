using System;

namespace AtCoderLibraryCSharp
{
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
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            p++;
            while (p <= _n)
            {
                _data[p - 1] += x;
                p += p & -p;
            }
        }

        public long Sum(int l, int r)
        {
            if (l < 0 || r < l || _n < r) throw new IndexOutOfRangeException();
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