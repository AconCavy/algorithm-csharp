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

            public Edge(int from, int to, long capacity, long flow, long cost)
            {
                From = from;
                To = to;
                Capacity = capacity;
                Flow = flow;
                Cost = cost;
            }
        }

        private readonly struct InternalEdge
        {
            public readonly int To;
            public readonly int Rev;
            public readonly long Capacity;
            public readonly long Cost;

            public InternalEdge(int to, int rev, long capacity, long cost)
            {
                To = to;
                Rev = rev;
                Capacity = capacity;
                Cost = cost;
            }
        }

        private readonly struct Q : IComparable<Q>
        {
            public readonly long Key;
            public readonly int To;

            public Q(long key, int to)
            {
                Key = key;
                To = to;
            }

            public int CompareTo(Q other)
            {
                var ret = Comparer<long>.Default.Compare(Key, other.Key);
                if (ret == 0) ret = Comparer<int>.Default.Compare(To, other.To);
                return ret;
            }
        }

        private readonly int _n;
        private readonly List<InternalEdge>[] _edges;
        private readonly List<(int X, int Y)> _pos;

        public MinCostFlowGraph(int n = 0)
        {
            _n = n;
            _edges = new List<InternalEdge>[n].Select(x => new List<InternalEdge>()).ToArray();
            _pos = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long capacity, long cost)
        {
            if (from < 0 || _n <= from) throw new IndexOutOfRangeException(nameof(from));
            if (to < 0 || _n <= to) throw new IndexOutOfRangeException(nameof(to));
            var m = _pos.Count;
            _pos.Add((from, _edges[from].Count));
            _edges[from].Add(new InternalEdge(to, _edges[to].Count, capacity, cost));
            _edges[to].Add(new InternalEdge(from, _edges[from].Count - 1, 0, -cost));
            return m;
        }

        public Edge GetEdge(int i)
        {
            if (i < 0 || _pos.Count <= i) throw new IndexOutOfRangeException(nameof(i));
            var e = _edges[_pos[i].X][_pos[i].Y];
            var re = _edges[e.To][e.Rev];
            return new Edge(_pos[i].X, e.To, e.Capacity + re.Capacity, re.Capacity, e.Cost);
        }

        public IEnumerable<Edge> GetEdges()
        {
            for (var i = 0; i < _pos.Count; i++) yield return GetEdge(i);
        }

        public (long, long) Flow(int s, int t) => Flow(s, t, long.MaxValue);

        public (long, long) Flow(int s, int t, long flowLimit) => Slope(s, t, flowLimit).First();

        public IEnumerable<(long, long)> Slope(int s, int t) => Slope(s, t, long.MaxValue);

        public IEnumerable<(long, long)> Slope(int s, int t, long flowLimit)
        {
            if (s < 0 || _n <= s) throw new IndexOutOfRangeException(nameof(s));
            if (t < 0 || _n <= t) throw new IndexOutOfRangeException(nameof(t));
            if (s == t) throw new ArgumentException();
            var dual = new long[_n];
            int[] pv;
            int[] pe;

            bool Bfs()
            {
                var dist = Enumerable.Repeat(long.MaxValue, _n).ToArray();
                var visited = new bool[_n];
                pv = Enumerable.Repeat(-1, _n).ToArray();
                pe = Enumerable.Repeat(-1, _n).ToArray();
                var queue = new PriorityQueue<Q>();
                dist[s] = 0;
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
                for (var v = 0; v < _n; v++)
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
                for (var v = t; v != s; v = pv[v])
                {
                    c = System.Math.Min(c, _edges[pv[v]][pe[v]].Capacity);
                }

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
                prev = cost;
            }

            return ret;
        }
    }
}