using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class LowestCommonAncestor
    {
        private readonly long[] _costs;
        private readonly int[] _distances;
        private readonly int _length;
        private readonly int _log;
        private readonly int[][] _parents;
        private readonly int _root;
        private readonly List<(int, long)>[] _tree;

        private bool _isUpdated;

        public LowestCommonAncestor(int length, int root = 0)
        {
            if (root < 0 || length <= root) throw new ArgumentOutOfRangeException(nameof(root));
            _length = length;
            _root = root;
            while (_length >> _log > 0) _log++;
            _distances = new int[length];
            _costs = new long[length];
            _parents = new int[length][].Select(x => new int[_log]).ToArray();
            _tree = new List<(int, long)>[length].Select(_ => new List<(int, long)>()).ToArray();
        }

        public void AddEdge(int u, int v, long cost = 1)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            _tree[u].Add((v, cost));
            _tree[v].Add((u, cost));
            _isUpdated = false;
        }

        public int Find(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (!_isUpdated) Build();

            if (_distances[u] > _distances[v]) (u, v) = (v, u);
            v = GetAncestor(v, _distances[v] - _distances[u]);
            if (u == v) return u;
            for (var i = _log - 1; i >= 0; i--)
            {
                if (_parents[u][i] != _parents[v][i]) (u, v) = (_parents[u][i], _parents[v][i]);
            }

            return _parents[u][0];
        }

        public int GetAncestor(int v, int height)
        {
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (!_isUpdated) Build();

            var parent = v;
            for (var i = 0; i < _log && parent != -1; i++)
            {
                if ((height >> i & 1) == 1) parent = _parents[parent][i];
            }

            return parent;
        }

        public int GetDistance(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var p = Find(u, v);
            return _distances[u] + _distances[v] - _distances[p] * 2;
        }

        public long GetCost(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var p = Find(u, v);
            return _costs[u] + _costs[v] - _costs[p] * 2;
        }

        public long GetCost(int u, int v, int mod)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var p = Find(u, v);
            var cost = (_costs[u] + _costs[v]) % mod;
            cost = (cost - _costs[p] * 2 % mod) % mod;
            return (cost + mod) % mod;
        }

        private void Build()
        {
            _isUpdated = true;
            var queue = new Queue<(int current, int from, int distance, long cost)>();
            queue.Enqueue((_root, -1, 0, 0));
            var used = new bool[_length];
            used[_root] = true;
            while (queue.Count > 0)
            {
                var (u, p, depth, cost) = queue.Dequeue();
                _parents[u][0] = p;
                _distances[u] = depth;
                _costs[u] = cost;
                foreach (var (v, c) in _tree[u])
                {
                    if (used[v]) continue;
                    used[v] = true;
                    queue.Enqueue((v, u, depth + 1, cost + c));
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
    }
}