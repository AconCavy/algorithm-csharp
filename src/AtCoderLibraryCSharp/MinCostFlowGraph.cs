using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class MinCostFlowGraph
    {
        public readonly struct Edge
        {
            public readonly int From;
            public readonly int To;
            public readonly long Capacity;
            public readonly long Flow;
            public readonly long Cost;

            public Edge(int from, int to, long capacity, long flow, long cost) =>
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

        private readonly struct Q : IComparable<Q>
        {
            public readonly long Key;
            public readonly int To;

            public Q(long key, int to) => (Key, To) = (key, to);

            public int CompareTo(Q other)
            {
                var ret = Comparer<long>.Default.Compare(Key, other.Key);
                if (ret == 0) ret = Comparer<int>.Default.Compare(To, other.To);
                return ret;
            }
        }

        private readonly int _length;
        private readonly List<InternalEdge>[] _edges;
        private readonly List<(int X, int Y)> _positions;

        public MinCostFlowGraph(int length = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;
            _edges = new List<InternalEdge>[length].Select(x => new List<InternalEdge>()).ToArray();
            _positions = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long capacity, long cost)
        {
            if (from < 0 || _length <= from) throw new ArgumentOutOfRangeException(nameof(from));
            if (to < 0 || _length <= to) throw new ArgumentOutOfRangeException(nameof(to));
            if (capacity < 0) throw new ArgumentException(nameof(capacity));
            if (cost < 0) throw new ArgumentException(nameof(cost));
            var m = _positions.Count;
            _positions.Add((from, _edges[from].Count));
            var fromId = _edges[from].Count;
            var toId = _edges[to].Count;
            if (from == to) toId++;
            _edges[from].Add(new InternalEdge(to, toId, capacity, cost));
            _edges[to].Add(new InternalEdge(from, fromId, 0, -cost));
            return m;
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

        public (long, long) Flow(int s, int t) => Flow(s, t, long.MaxValue);

        public (long, long) Flow(int s, int t, long flowLimit) => Slope(s, t, flowLimit).Last();

        public IEnumerable<(long, long)> Slope(int s, int t) => Slope(s, t, long.MaxValue);

        public IEnumerable<(long, long)> Slope(int s, int t, long flowLimit)
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
                var queue = new PriorityQueue<Q>();
                queue.Enqueue(new Q(0, s));
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
                        queue.Enqueue(new Q(dist[e.To], e.To));
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
                for (var v = t; v != s; v = pv[v]) c = System.Math.Min(c, _edges[pv[v]][pe[v]].Capacity);

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
    }
}