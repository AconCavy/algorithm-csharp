using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class MaxFlowGraph
    {
        public readonly struct Edge
        {
            public readonly int From;
            public readonly int To;
            public readonly long Capacity;
            public readonly long Flow;

            public Edge(int from, int to, long capacity, long flow)
            {
                From = from;
                To = to;
                Capacity = capacity;
                Flow = flow;
            }
        }

        private readonly struct InternalEdge
        {
            public readonly int To;
            public readonly int Rev;
            public readonly long Capacity;

            public InternalEdge(int to, int rev, long capacity)
            {
                To = to;
                Rev = rev;
                Capacity = capacity;
            }
        }

        private readonly int _length;
        private readonly List<InternalEdge>[] _edges;
        private readonly List<(int X, int Y)> _positions;

        public MaxFlowGraph(int length = 0)
        {
            _length = length;
            _edges = new List<InternalEdge>[length].Select(x => new List<InternalEdge>()).ToArray();
            _positions = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long capacity)
        {
            if (from < 0 || _length <= from) throw new IndexOutOfRangeException(nameof(from));
            if (to < 0 || _length <= to) throw new IndexOutOfRangeException(nameof(to));
            if (capacity < 0) throw new ArgumentException(nameof(capacity));
            var count = _positions.Count;
            _positions.Add((from, _edges[from].Count));
            var fromId = _edges[from].Count;
            var toId = _edges[to].Count;
            if (from == to) toId++;
            _edges[from].Add(new InternalEdge(to, toId, capacity));
            _edges[to].Add(new InternalEdge(from, fromId, 0));
            return count;
        }

        public Edge GetEdge(int index)
        {
            var m = _positions.Count;
            if (index < 0 || m <= index) throw new IndexOutOfRangeException(nameof(index));
            var e = _edges[_positions[index].X][_positions[index].Y];
            var re = _edges[e.To][e.Rev];
            return new Edge(_positions[index].X, e.To, e.Capacity + re.Capacity, re.Capacity);
        }

        public IEnumerable<Edge> GetEdges()
        {
            for (var i = 0; i < _positions.Count; i++) yield return GetEdge(i);
        }

        public void ChangeEdge(int index, long newCapacity, long newFlow)
        {
            var count = _positions.Count;
            if (index < 0 || count <= index) throw new IndexOutOfRangeException(nameof(index));
            if (newFlow < 0 || newCapacity < newFlow) throw new ArgumentException();
            var e = _edges[_positions[index].X][_positions[index].Y];
            var re = _edges[e.To][e.Rev];
            _edges[_positions[index].X][_positions[index].Y] = new InternalEdge(e.To, e.Rev, newCapacity - newFlow);
            _edges[e.To][e.Rev] = new InternalEdge(re.To, re.Rev, newFlow);
        }

        public long Flow(int s, int t) => Flow(s, t, long.MaxValue);

        public long Flow(int s, int t, long flowLimit)
        {
            if (s < 0 || _length <= s) throw new IndexOutOfRangeException(nameof(s));
            if (t < 0 || _length <= t) throw new IndexOutOfRangeException(nameof(t));
            if (s == t) throw new ArgumentException();
            var queue = new Queue<int>();
            var depth = new int[_length];
            var iter = new int[_length];

            void Bfs()
            {
                Array.Fill(depth, -1);
                depth[s] = 0;
                queue.Clear();
                queue.Enqueue(s);
                while (queue.Any())
                {
                    var v = queue.Dequeue();
                    foreach (var edge in _edges[v].Where(edge => edge.Capacity != 0 && depth[edge.To] < 0))
                    {
                        depth[edge.To] = depth[v] + 1;
                        if (edge.To == t) return;
                        queue.Enqueue(edge.To);
                    }
                }
            }

            long Dfs(int v, long up)
            {
                if (v == s) return up;
                var ret = 0L;
                var dv = depth[v];
                for (var i = iter[v]; i < _edges[v].Count; i++)
                {
                    var e = _edges[v][i];
                    if (dv <= depth[e.To] || _edges[e.To][e.Rev].Capacity == 0) continue;
                    var d = Dfs(e.To, System.Math.Min(up - ret, _edges[e.To][e.Rev].Capacity));
                    if (d <= 0) continue;
                    e = _edges[v][i];
                    _edges[v][i] = new InternalEdge(e.To, e.Rev, e.Capacity + d);
                    var re = _edges[e.To][e.Rev];
                    _edges[e.To][e.Rev] = new InternalEdge(re.To, re.Rev, re.Capacity - d);
                    ret += d;
                    if (ret == up) return ret;
                }

                depth[v] = _length;
                return ret;
            }

            var flow = 0L;
            while (flow < flowLimit)
            {
                Bfs();
                if (depth[t] == -1) break;
                Array.Fill(iter, 0);
                var f = Dfs(t, flowLimit - flow);
                if (f == 0) break;
                flow += f;
            }

            return flow;
        }

        public IEnumerable<bool> MinCut(int s)
        {
            var visited = new bool[_length];
            var queue = new Queue<int>();
            queue.Enqueue(s);
            while (queue.Any())
            {
                var p = queue.Dequeue();
                visited[p] = true;
                foreach (var edge in _edges[p].Where(edge => edge.Capacity > 0 && !visited[edge.To]))
                {
                    visited[edge.To] = true;
                    queue.Enqueue(edge.To);
                }
            }

            return visited;
        }
    }
}