using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class DisjointSetUnion
    {
        private readonly int _length;
        private readonly int[] _parentOrSize;

        public DisjointSetUnion(int length = 0)
        {
            _length = length;
            _parentOrSize = Enumerable.Repeat(-1, length).ToArray();
        }

        public int Merge(int a, int b)
        {
            if (a < 0 || _length <= a) throw new IndexOutOfRangeException(nameof(a));
            if (b < 0 || _length <= b) throw new IndexOutOfRangeException(nameof(b));
            var (x, y) = (LeaderOf(a), LeaderOf(b));
            if (x == y) return x;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            return x;
        }

        public bool IsSame(int a, int b)
        {
            if (a < 0 || _length <= a) throw new IndexOutOfRangeException(nameof(a));
            if (b < 0 || _length <= b) throw new IndexOutOfRangeException(nameof(b));
            return LeaderOf(a) == LeaderOf(b);
        }

        public int LeaderOf(int a)
        {
            if (a < 0 || _length <= a) throw new IndexOutOfRangeException(nameof(a));
            if (_parentOrSize[a] < 0) return a;
            return _parentOrSize[a] = LeaderOf(_parentOrSize[a]);
        }

        public int SizeOf(int a)
        {
            if (a < 0 || _length <= a) throw new IndexOutOfRangeException(nameof(a));
            return -_parentOrSize[LeaderOf(a)];
        }

        public IEnumerable<IEnumerable<int>> GetGroups()
        {
            var leaders = new int[_length];
            var groupSize = new int[_length];
            for (var i = 0; i < _length; i++)
            {
                leaders[i] = LeaderOf(i);
                groupSize[leaders[i]]++;
            }

            var ret = new List<int>[_length].Select(x => new List<int>()).ToArray();
            for (var i = 0; i < _length; i++) ret[leaders[i]].Add(i);
            return ret.Where(x => x.Any());
        }
    }
}