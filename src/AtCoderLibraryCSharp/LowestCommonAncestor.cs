using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class LowestCommonAncestor
    {
        private readonly int _length;
        private readonly int _log;
        private readonly int _root;
        private readonly int[] _depths;
        private readonly long[] _costs;
        private readonly int[][] _parents;
        private readonly List<int>[] _tree;
        private readonly long[][] _costTree;

        private bool _isUpdated;

        public LowestCommonAncestor(IReadOnlyCollection<IReadOnlyCollection<int>> tree, int root = 0)
            : this(tree.Count, root)
        {
            _tree = tree.Select(x => x.ToList()).ToArray();
        }

        public LowestCommonAncestor(int length, int root = 0)
        {
            if (root < 0 || length <= root) throw new ArgumentOutOfRangeException(nameof(root));
            _length = length;
            _root = root;
            while (_length >> _log > 0) _log++;
            _depths = new int[length];
            _costs = new long[length];
            _parents = new int[length][].Select(x => new int[_log]).ToArray();
            _tree = new List<int>[length].Select(_ => new List<int>()).ToArray();
            _costTree = new long[length].Select(_ => new long[length]).ToArray();
        }

        public void AddEdge(int u, int v, long cost = 0)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            _tree[u].Add(v);
            _tree[v].Add(u);
            _costTree[u][v] = _costTree[v][u] = cost;
            _isUpdated = false;
        }

        public int Find(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (!_isUpdated) Build();

            if (_depths[u] > _depths[v]) (u, v) = (v, u);
            v = GetAncestor(v, _depths[v] - _depths[u]);
            if (u == v) return u;
            for (var i = _log - 1; i >= 0; i--)
                if (_parents[u][i] != _parents[v][i])
                    (u, v) = (_parents[u][i], _parents[v][i]);

            return _parents[u][0];
        }

        public int GetAncestor(int v, int height)
        {
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            if (!_isUpdated) Build();

            var parent = v;
            for (var i = 0; i < _log && parent != -1; i++)
                if ((height >> i & 1) == 1)
                    parent = _parents[parent][i];

            return parent;
        }

        public int GetDistance(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var p = Find(u, v);
            return _depths[u] + _depths[v] - _depths[p] * 2;
        }

        public long GetCost(int u, int v)
        {
            if (u < 0 || _length <= u) throw new ArgumentOutOfRangeException(nameof(u));
            if (v < 0 || _length <= v) throw new ArgumentOutOfRangeException(nameof(v));
            var p = Find(u, v);
            return _costs[u] + _costs[v] - _costs[p] * 2;
        }

        private void Build()
        {
            _isUpdated = false;
            var queue = new Queue<(int current, int from, int distance, long cost)>();
            queue.Enqueue((_root, -1, 0, 0));
            var used = new bool[_length];
            used[_root] = true;
            while (queue.Any())
            {
                var (current, from, depth, cost) = queue.Dequeue();
                _parents[current][0] = from;
                _depths[current] = depth;
                _costs[current] = cost;
                foreach (var next in _tree[current].Where(next => !used[next]))
                {
                    used[next] = true;
                    queue.Enqueue((next, current, depth + 1, cost + _costTree[current][next]));
                }
            }

            for (var i = 0; i < _log - 1; i++)
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