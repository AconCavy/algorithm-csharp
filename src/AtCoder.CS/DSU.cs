using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class DSU
    {
        private readonly int _n;
        private readonly int[] _parentOrSize;

        public DSU(int n = 0)
        {
            _n = n;
            _parentOrSize = Enumerable.Repeat(-1, n).ToArray();
        }

        public int Merge(int a, int b)
        {
            if (a < 0 || _n <= a) throw new IndexOutOfRangeException(nameof(a));
            if (b < 0 || _n <= b) throw new IndexOutOfRangeException(nameof(b));
            var (x, y) = (LeaderOf(a), LeaderOf(b));
            if (x == y) return x;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            return x;
        }

        public bool IsSame(int a, int b)
        {
            if (a < 0 || _n <= a) throw new IndexOutOfRangeException(nameof(a));
            if (b < 0 || _n <= b) throw new IndexOutOfRangeException(nameof(b));
            return LeaderOf(a) == LeaderOf(b);
        }

        public int LeaderOf(int a)
        {
            if (a < 0 || _n <= a) throw new IndexOutOfRangeException(nameof(a));
            if (_parentOrSize[a] < 0) return a;
            return _parentOrSize[a] = LeaderOf(_parentOrSize[a]);
        }

        public int SizeOf(int a)
        {
            if (a < 0 || _n <= a) throw new IndexOutOfRangeException(nameof(a));
            return -_parentOrSize[LeaderOf(a)];
        }

        public IEnumerable<IEnumerable<int>> Groups()
        {
            var leaders = new int[_n];
            var groupSize = new int[_n];
            for (var i = 0; i < _n; i++)
            {
                leaders[i] = LeaderOf(i);
                groupSize[leaders[i]]++;
            }

            var ret = new List<int>[_n].Select(x => new List<int>()).ToArray();
            for (var i = 0; i < _n; i++) ret[leaders[i]].Add(i);
            return ret.Where(x => x.Any());
        }
    }
}