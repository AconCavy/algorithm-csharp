using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
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

        private readonly int _n;
        private readonly List<InternalEdge>[] _edges;
        private readonly List<(int X, int Y)> _pos;

        public MaxFlowGraph(int n = 0)
        {
            _n = n;
            _edges = new List<InternalEdge>[n].Select(x => new List<InternalEdge>()).ToArray();
            _pos = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long capacity)
        {
            if (from < 0 || _n <= from) throw new IndexOutOfRangeException(nameof(from));
            if (to < 0 || _n <= to) throw new IndexOutOfRangeException(nameof(to));
            if (capacity < 0) throw new ArgumentException(nameof(capacity));
            var m = _pos.Count;
            _pos.Add((from, _edges[from].Count));
            _edges[from].Add(new InternalEdge(to, _edges[to].Count, capacity));
            _edges[to].Add(new InternalEdge(from, _edges[from].Count - 1, 0));
            return m;
        }

        public Edge GetEdge(int i)
        {
            var m = _pos.Count;
            if (i < 0 || m <= i) throw new IndexOutOfRangeException(nameof(i));
            var e = _edges[_pos[i].X][_pos[i].Y];
            var re = _edges[e.To][e.Rev];
            return new Edge(_pos[i].X, e.To, e.Capacity + re.Capacity, re.Capacity);
        }

        public IEnumerable<Edge> GetEdges()
        {
            for (var i = 0; i < _pos.Count; i++) yield return GetEdge(i);
        }

        public void ChangeEdge(int i, long newCapacity, long newFlow)
        {
            var m = _pos.Count;
            if (i < 0 || m <= i) throw new IndexOutOfRangeException(nameof(i));
            if (newFlow < 0 || newCapacity < newFlow) throw new ArgumentException();
            var e = _edges[_pos[i].X][_pos[i].Y];
            var re = _edges[e.To][e.Rev];
            _edges[_pos[i].X][_pos[i].Y] = new InternalEdge(e.To, e.Rev, newCapacity - newFlow);
            _edges[e.To][e.Rev] = new InternalEdge(re.To, re.Rev, newFlow);
        }

        public long Flow(int s, int t) => Flow(s, t, long.MaxValue);

        public long Flow(int s, int t, long flowLimit)
        {
            if (s < 0 || _n <= s) throw new IndexOutOfRangeException(nameof(s));
            if (t < 0 || _n <= t) throw new IndexOutOfRangeException(nameof(t));
            var queue = new Queue<int>();
            int[] depth;
            int[] iter;

            void Bfs()
            {
                depth = Enumerable.Repeat(-1, _n).ToArray();
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
                    if (ret == up) break;
                }

                return ret;
            }

            var flow = 0L;
            while (flow < flowLimit)
            {
                Bfs();
                if (depth[t] == -1) break;
                iter = new int[_n];
                while (flow < flowLimit)
                {
                    var f = Dfs(t, flowLimit - flow);
                    if (f == 0) break;
                    flow += f;
                }
            }

            return flow;
        }

        public IEnumerable<bool> MinCut(int s)
        {
            var visited = new bool[_n];
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