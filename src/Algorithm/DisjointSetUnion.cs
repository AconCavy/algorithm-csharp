using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class DisjointSetUnion
    {
        public int Length { get; }

        private readonly int[] _parentOrSize;

        public DisjointSetUnion(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            _parentOrSize = new int[Length];
            Array.Fill(_parentOrSize, -1);
        }

        public int Merge(int u, int v)
        {
            if (u < 0 || Length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || Length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var (x, y) = (LeaderOf(u), LeaderOf(v));
            if (x == y) return x;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            return x;
        }

        public bool IsSame(int u, int v)
        {
            if (u < 0 || Length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || Length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            return LeaderOf(u) == LeaderOf(v);
        }

        public int LeaderOf(int v)
        {
            if (v < 0 || Length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (_parentOrSize[v] < 0) return v;
            return _parentOrSize[v] = LeaderOf(_parentOrSize[v]);
        }

        public int SizeOf(int v)
        {
            if (v < 0 || Length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            return -_parentOrSize[LeaderOf(v)];
        }

        public IEnumerable<IReadOnlyCollection<int>> GetGroups()
        {
            var result = new List<int>[Length].Select(x => new List<int>()).ToArray();
            for (var i = 0; i < Length; i++) result[LeaderOf(i)].Add(i);
            return result.Where(x => x.Count > 0);
        }
    }
}