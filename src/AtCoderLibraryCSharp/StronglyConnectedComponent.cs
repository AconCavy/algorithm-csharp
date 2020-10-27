using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class StronglyConnectedComponent
    {
        private readonly struct Edge
        {
            public readonly int To;
            public Edge(int to) => To = to;
        }

        private readonly int _length;
        private readonly List<(int, Edge)> _edges;

        public StronglyConnectedComponent(int length = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;
            _edges = new List<(int, Edge)>();
        }

        public void AddEdge(int from, int to)
        {
            if (from < 0 || _length <= from) throw new ArgumentOutOfRangeException(nameof(from));
            if (to < 0 || _length <= to) throw new ArgumentOutOfRangeException(nameof(to));
            _edges.Add((from, new Edge(to)));
        }

        public (int, IEnumerable<int>) GetIds()
        {
            var g = new CompressedSparseRow<Edge>(_length, _edges);
            var (nowOrd, groupNum) = (0, 0);
            var visited = new Stack<int>(_length);
            var low = new int[_length];
            var ord = new int[_length];
            Array.Fill(ord, -1);
            var ids = new int[_length];

            void Dfs(int v)
            {
                low[v] = ord[v] = nowOrd++;
                visited.Push(v);
                for (var i = g.Start[v]; i < g.Start[v + 1]; i++)
                {
                    var to = g.Edges[i].To;
                    if (ord[to] == -1)
                    {
                        Dfs(to);
                        low[v] = System.Math.Min(low[v], low[to]);
                    }
                    else low[v] = System.Math.Min(low[v], ord[to]);
                }

                if (low[v] != ord[v]) return;
                while (true)
                {
                    var u = visited.Pop();
                    ord[u] = _length;
                    ids[u] = groupNum;
                    if (u == v) break;
                }

                groupNum++;
            }

            for (var i = 0; i < _length; i++)
                if (ord[i] == -1)
                    Dfs(i);

            for (var i = 0; i < _length; i++) ids[i] = groupNum - 1 - ids[i];

            return (groupNum, ids);
        }

        public IEnumerable<IEnumerable<int>> GetGraph()
        {
            var (groupNum, identities) = GetIds();
            var ids = identities.ToArray();
            var groups = new List<int>[groupNum].Select(_ => new List<int>()).ToArray();
            foreach (var (id, index) in ids.Select((x, i) => (x, i))) groups[id].Add(index);
            return groups;
        }
    }
}