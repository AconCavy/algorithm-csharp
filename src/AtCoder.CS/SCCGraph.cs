using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class SCCGraph
    {
        private struct Edge
        {
            public int To;
        }

        private readonly int _n;
        private readonly List<(int, Edge)> _edges;

        public SCCGraph(int n = 0)
        {
            _n = n;
            _edges = new List<(int, Edge)>();
        }

        public void AddEdge(int from, int to)
        {
            if (from < 0 || _n <= from) throw new IndexOutOfRangeException(nameof(from));
            if (to < 0 || _n <= to) throw new IndexOutOfRangeException(nameof(to));
            _edges.Add((from, new Edge {To = to}));
        }

        public (int, IEnumerable<int>) Ids()
        {
            var g = new CSR<Edge>(_n, _edges);
            var (nowOrd, groupNum) = (0, 0);
            var visited = new Stack<int>(_n);
            var low = new int[_n];
            var ord = Enumerable.Repeat(-1, _n).ToArray();
            var ids = new int[_n];

            void DFS(int v)
            {
                low[v] = ord[v] = nowOrd++;
                visited.Push(v);
                for (var i = g.Start[v]; i < g.Start[v + 1]; i++)
                {
                    var to = g.Edges[i].To;
                    if (ord[to] == -1)
                    {
                        DFS(to);
                        low[v] = System.Math.Min(low[v], low[to]);
                    }
                    else
                    {
                        low[v] = System.Math.Min(low[v], ord[to]);
                    }
                }

                if (low[v] != ord[v]) return;
                while (true)
                {
                    var u = visited.Pop();
                    ord[u] = _n;
                    ids[u] = groupNum;
                    if (u == v) break;
                }

                groupNum++;
            }

            for (var i = 0; i < _n; i++)
                if (ord[i] == -1)
                    DFS(i);

            for (var i = 0; i < _n; i++) ids[i] = groupNum - 1 - ids[i];

            return (groupNum, ids);
        }

        public IEnumerable<IEnumerable<int>> GetSCC()
        {
            var (groupNum, tmp) = Ids();
            var ids = tmp.ToArray();
            var groups = new List<int>[groupNum].Select(x => new List<int>()).ToArray();
            foreach (var (id, i) in ids.Select((x, i) => (x, i))) groups[id].Add(i);
            return groups;
        }
    }
}