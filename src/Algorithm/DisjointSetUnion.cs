using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class DisjointSetUnion
    {
        private readonly int _length;
        private readonly int[] _parentOrSize;

        public DisjointSetUnion(int length = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;
            _parentOrSize = new int[_length];
            Array.Fill(_parentOrSize, -1);
        }

        public int Merge(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var (x, y) = (LeaderOf(u), LeaderOf(v));
            if (x == y) return x;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            return x;
        }

        public bool IsSame(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            return LeaderOf(u) == LeaderOf(v);
        }

        public int LeaderOf(int v)
        {
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (_parentOrSize[v] < 0) return v;
            return _parentOrSize[v] = LeaderOf(_parentOrSize[v]);
        }

        public int SizeOf(int v)
        {
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            return -_parentOrSize[LeaderOf(v)];
        }

        public IEnumerable<IEnumerable<int>> GetGroups()
        {
            var ret = new List<int>[_length].Select(x => new List<int>()).ToArray();
            for (var i = 0; i < _length; i++) ret[LeaderOf(i)].Add(i);
            return ret.Where(x => x.Any());
        }
    }
}