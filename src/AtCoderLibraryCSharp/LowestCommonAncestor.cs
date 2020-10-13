using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class LowestCommonAncestor
    {
        private readonly int _length;
        private readonly int _log;
        private readonly int[] _depth;
        private readonly int[][] _parents;

        public LowestCommonAncestor(IEnumerable<IEnumerable<int>> tree, int root = 0)
        {
            var T = tree.Select(x => x.ToArray()).ToArray();
            _length = T.Length;
            if (root < 0 || _length <= root) throw new IndexOutOfRangeException(nameof(root));
            while (_length >> _log > 0) _log++;
            _depth = new int[_length];
            _parents = new int[_length][].Select(x => new int[_log]).ToArray();
            var queue = new Queue<(int current, int previous, int distance)>();
            queue.Enqueue((root, -1, 0));
            var used = new bool[_length];
            used[root] = true;
            while (queue.Any())
            {
                var (current, previous, distance) = queue.Dequeue();
                _parents[current][0] = previous;
                _depth[current] = distance;
                foreach (var next in T[current].Where(next => !used[next]))
                {
                    used[next] = true;
                    queue.Enqueue((next, current, distance + 1));
                }
            }

            for (var i = 0; i + 1 < _log; i++)
            {
                for (var v = 0; v < _length; v++)
                {
                    var parent = _parents[v][i];
                    _parents[v][i + 1] = parent == -1 ? -1 : _parents[parent][i];
                }
            }
        }

        public int Find(int u, int v)
        {
            if (u < 0 || _length <= u) throw new IndexOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new IndexOutOfRangeException(nameof(v));
            if (_depth[u] > _depth[v]) (u, v) = (v, u);
            v = GetAncestor(v, _depth[v] - _depth[u]);
            if (u == v) return u;
            for (var i = _log - 1; i >= 0; i--)
                if (_parents[u][i] != _parents[v][i])
                    (u, v) = (_parents[u][i], _parents[v][i]);

            return _parents[u][0];
        }

        public int GetAncestor(int v, int height)
        {
            if (v < 0 || _length <= v) throw new IndexOutOfRangeException(nameof(v));
            var parent = v;
            for (var i = 0; i < _log && parent != -1; i++)
                if ((height >> i & 1) == 1)
                    parent = _parents[parent][i];

            return parent;
        }

        public int GetDistance(int u, int v)
        {
            if (u < 0 || _length <= u) throw new IndexOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new IndexOutOfRangeException(nameof(v));
            var p = Find(u, v);
            return _depth[u] + _depth[v] - _depth[p] * 2;
        }
    }
}