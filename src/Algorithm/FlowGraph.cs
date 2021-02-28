using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class FlowGraph
    {
        private readonly List<InternalEdge>[] _edges;
        private readonly int _length;
        private readonly List<(int X, int Y)> _positions;

        public FlowGraph(int length = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;
            _edges = new List<InternalEdge>[length].Select(x => new List<InternalEdge>()).ToArray();
            _positions = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long capacity, long cost = 0)
        {
            if (from < 0 || _length <= from) throw new ArgumentOutOfRangeException(nameof(from));
            if (to < 0 || _length <= to) throw new ArgumentOutOfRangeException(nameof(to));
            if (capacity < 0) throw new ArgumentException(nameof(capacity));
            if (cost < 0) throw new ArgumentException(nameof(cost));
            var count = _positions.Count;
            _positions.Add((from, _edges[from].Count));
            var fromId = _edges[from].Count;
            var toId = _edges[to].Count;
            if (from == to) toId++;
            _edges[from].Add(new InternalEdge(to, toId, capacity, cost));
            _edges[to].Add(new InternalEdge(from, fromId, 0, -cost));
            return count;
        }

        public void ChangeEdge(int index, long newCapacity, long newFlow)
        {
            var count = _positions.Count;
            if (index < 0 || count <= index) throw new ArgumentOutOfRangeException(nameof(index));
            if (newFlow < 0 || newCapacity < newFlow) throw new ArgumentException();
            var e = _edges[_positions[index].X][_positions[index].Y];
            var re = _edges[e.To][e.Rev];
            _edges[_positions[index].X][_positions[index].Y] =
                new InternalEdge(e.To, e.Rev, newCapacity - newFlow, e.Cost);
            _edges[e.To][e.Rev] = new InternalEdge(re.To, re.Rev, newFlow, re.Cost);
        }

        public Edge GetEdge(int index)
        {
            if (index < 0 || _positions.Count <= index) throw new ArgumentOutOfRangeException(nameof(index));
            var e = _edges[_positions[index].X][_positions[index].Y];
            var re = _edges[e.To][e.Rev];
            return new Edge(_positions[index].X, e.To, e.Capacity + re.Capacity, re.Capacity, e.Cost);
        }

        public IEnumerable<Edge> GetEdges()
        {
            for (var i = 0; i < _positions.Count; i++) yield return GetEdge(i);
        }

        public long MaxFlow(int s, int t, long flowLimit = long.MaxValue)
        {
            if (s < 0 || _length <= s) throw new ArgumentOutOfRangeException(nameof(s));
            if (t < 0 || _length <= t) throw new ArgumentOutOfRangeException(nameof(t));
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
                    var d = Dfs(e.To, Math.Min(up - ret, _edges[e.To][e.Rev].Capacity));
                    if (d <= 0) continue;
                    e = _edges[v][i];
                    _edges[v][i] = new InternalEdge(e.To, e.Rev, e.Capacity + d, e.Cost);
                    var re = _edges[e.To][e.Rev];
                    _edges[e.To][e.Rev] = new InternalEdge(re.To, re.Rev, re.Capacity - d, re.Cost);
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

        public (long, long) MinCostFlow(int s, int t, long flowLimit = long.MaxValue) =>
            MinCostSlope(s, t, flowLimit).Last();

        public IEnumerable<(long, long)> MinCostSlope(int s, int t, long flowLimit = long.MaxValue)
        {
            if (s < 0 || _length <= s) throw new ArgumentOutOfRangeException(nameof(s));
            if (t < 0 || _length <= t) throw new ArgumentOutOfRangeException(nameof(t));
            if (s == t) throw new ArgumentException();
            var dist = new long[_length];
            var dual = new long[_length];
            var pv = new int[_length];
            var pe = new int[_length];
            var visited = new bool[_length];

            bool Bfs()
            {
                Array.Fill(dist, long.MaxValue);
                dist[s] = 0;
                Array.Fill(visited, false);
                var queue = new PriorityQueue<(long Key, int To)>((x, y) =>
                {
                    var comparison = x.Key.CompareTo(y.Key);
                    return comparison == 0 ? x.To.CompareTo(y.To) : comparison;
                });
                queue.Enqueue((0, s));
                while (queue.Any())
                {
                    var cur = queue.Dequeue();
                    var v = cur.To;
                    if (visited[v]) continue;
                    visited[v] = true;
                    if (v == t) break;
                    for (var i = 0; i < _edges[v].Count; i++)
                    {
                        var e = _edges[v][i];
                        if (visited[e.To] || e.Capacity <= 0) continue;
                        var c = e.Cost - dual[e.To] + dual[v];
                        if (dist[e.To] <= dist[v] + c) continue;
                        dist[e.To] = dist[v] + c;
                        pv[e.To] = v;
                        pe[e.To] = i;
                        queue.Enqueue((dist[e.To], e.To));
                    }
                }

                if (!visited[t]) return false;
                for (var v = 0; v < _length; v++)
                {
                    if (!visited[v]) continue;
                    dual[v] -= dist[t] - dist[v];
                }

                return true;
            }

            var (flow, cost, prev) = (0L, 0L, -1L);
            var ret = new Stack<(long, long)>();
            ret.Push((flow, cost));
            while (flow < flowLimit && Bfs())
            {
                var c = flowLimit - flow;
                for (var v = t; v != s; v = pv[v]) c = Math.Min(c, _edges[pv[v]][pe[v]].Capacity);

                for (var v = t; v != s; v = pv[v])
                {
                    var e = _edges[pv[v]][pe[v]];
                    _edges[pv[v]][pe[v]] = new InternalEdge(e.To, e.Rev, e.Capacity - c, e.Cost);
                    var re = _edges[v][e.Rev];
                    _edges[v][e.Rev] = new InternalEdge(re.To, re.Rev, re.Capacity + c, re.Cost);
                }

                var d = -dual[s];
                flow += c;
                cost += c * d;
                if (prev == d) ret.Pop();
                ret.Push((flow, cost));
                prev = d;
            }

            return ret.Reverse();
        }

        public readonly struct Edge
        {
            public readonly int From;
            public readonly int To;
            public readonly long Capacity;
            public readonly long Flow;
            public readonly long Cost;

            public Edge(int from, int to, long capacity, long flow, long cost = 0) =>
                (From, To, Capacity, Flow, Cost) = (from, to, capacity, flow, cost);
        }

        private readonly struct InternalEdge
        {
            public readonly int To;
            public readonly int Rev;
            public readonly long Capacity;
            public readonly long Cost;

            public InternalEdge(int to, int rev, long capacity, long cost) =>
                (To, Rev, Capacity, Cost) = (to, rev, capacity, cost);
        }
    }
}