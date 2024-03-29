using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class StronglyConnectedComponent
    {
        public int Length { get; }

        private readonly List<(int, Edge)> _edges;

        public StronglyConnectedComponent(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            _edges = new List<(int, Edge)>();
        }

        public void AddEdge(int from, int to)
        {
            if (from < 0 || Length <= from) throw new ArgumentOutOfRangeException(nameof(from));
            if (to < 0 || Length <= to) throw new ArgumentOutOfRangeException(nameof(to));
            _edges.Add((from, new Edge(to)));
        }

        public (int GroupCount, int[] IDs) GetIDs()
        {
            var g = new CompressedSparseRow<Edge>(Length, _edges);
            var (nowOrd, groupCount) = (0, 0);
            var visited = new Stack<int>(Length);
            var low = new int[Length];
            var ord = new int[Length];
            Array.Fill(ord, -1);
            var ids = new int[Length];

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
                        low[v] = Math.Min(low[v], low[to]);
                    }
                    else
                    {
                        low[v] = Math.Min(low[v], ord[to]);
                    }
                }

                if (low[v] != ord[v]) return;
                while (true)
                {
                    var u = visited.Pop();
                    ord[u] = Length;
                    ids[u] = groupCount;
                    if (u == v) break;
                }

                groupCount++;
            }

            for (var i = 0; i < Length; i++)
            {
                if (ord[i] == -1)
                    Dfs(i);
            }

            for (var i = 0; i < Length; i++)
            {
                ids[i] = groupCount - 1 - ids[i];
            }

            return (groupCount, ids);
        }

        public IReadOnlyList<IReadOnlyList<int>> GetGraph()
        {
            var (groupCount, ids) = GetIDs();
            var groups = new List<int>[groupCount];
            for (var i = 0; i < groups.Length; i++)
            {
                groups[i] = new List<int>();
            }

            foreach (var (id, index) in ids.Select((x, i) => (x, i)))
            {
                groups[id].Add(index);
            }

            return groups;
        }

        private readonly record struct Edge(int To);
    }
}