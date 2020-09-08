using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class MinCostFlowGraph
    {
        public readonly struct Edge
        {
            public readonly int From;
            public readonly int To;
            public readonly long Cap;
            public readonly long Flow;
            public readonly long Cost;

            public Edge(int from, int to, long cap, long flow, long cost)
            {
                From = from;
                To = to;
                Cap = cap;
                Flow = flow;
                Cost = cost;
            }
        }

        private readonly struct InternalEdge
        {
            public readonly int To;
            public readonly int Rev;
            public readonly long Cap;
            public readonly long Cost;

            public InternalEdge(int to, int rev, long cap, long cost)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Cost = cost;
            }
        }

        private int _n;
        private List<InternalEdge>[] _edges;
        private List<(int X, int Y)> _pos;

        public MinCostFlowGraph(int n = 0)
        {
            _n = n;
            _edges = new List<InternalEdge>[n];
            _pos = new List<(int X, int Y)>();
        }

        public int AddEdge(int from, int to, long cap, long cost)
        {
            if (from < 0 || _n <= from) throw new ArgumentException(nameof(from));
            if (to < 0 || _n <= to) throw new ArgumentException(nameof(to));
            if (cap < 0) throw new ArgumentException(nameof(cap));
            var m = _pos.Count;
            _pos.Add((from, _edges.Length));
            _edges[from].Add(new InternalEdge(to, _edges[to].Count, cap, cost));
            _edges[to].Add(new InternalEdge(from, _edges[from].Count - 1, 0, -cost));
            return m;
        }

        public Edge GetEdge(int i)
        {
            if (i < 0 || _n <= i) throw new ArgumentException(nameof(i));
            var m = _pos.Count;
            var e = _edges[_pos[i].X][_pos[i].Y];
            var re = _edges[e.To][e.Rev];
            return new Edge(_pos[i].X, e.To, e.Cap + re.Cap, re.Cap, e.Cost);
        }

        public IEnumerable<Edge> GetEdges()
        {
            for (var i = 0; i < _pos.Count; i++) yield return GetEdge(i);
        }

        public (long, long) FlowOf(int s, int t) => FlowOf(s, t, long.MaxValue);

        public (long, long) FlowOf(int s, int t, long flowLimit) => SlopeOf(s, t, flowLimit).Last();

        public IEnumerable<(long, long)> SlopeOf(int s, int t) => SlopeOf(s, t, long.MaxValue);

        public IEnumerable<(long, long)> SlopeOf(int s, int t, long flowLimit)
        {
            if (s < 0 || _n <= s) throw new ArgumentException(nameof(s));
            if (t < 0 || _n <= t) throw new ArgumentException(nameof(t));
            if (s == t) throw new ArgumentException();
            var dual = new long[_n];
            var visited = new bool[_n];
            int[] pv;
            int[] pe;

            bool DualRef()
            {
                var dist = Enumerable.Repeat(long.MaxValue, _n).ToArray();
                pv = Enumerable.Repeat(-1, _n).ToArray();
                pe = Enumerable.Repeat(-1, _n).ToArray();
                var queue = new PriorityQueue<(long Key, int To)>();
                dist[s] = 0;
                queue.Enqueue((0, s));
                while (queue.Any())
                {
                    var v = queue.Dequeue().To;
                    if (visited[v]) continue;
                    visited[v] = true;
                    if (v == t) break;
                    for (var i = 0; i < _edges[v].Count; i++)
                    {
                        var e = _edges[v][i];
                        if (visited[e.To] || e.Cap == 0) continue;
                        var c = e.Cost - dual[e.To] + dual[v];
                        if (dist[e.To] <= dist[v] + c) continue;
                        dist[e.To] = dist[v] + c;
                        pv[e.To] = v;
                        pe[e.To] = i;
                        queue.Enqueue((dist[e.To], e.To));
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
            while (flow < flowLimit)
            {
                if (!DualRef()) break;
                var c = flowLimit - flow;
                for (var v = t; v != s; v = pv[v])
                {
                    c = System.Math.Min(c, _edges[pv[v]][pe[v]].Cap);
                }

                for (var v = t; v != s; v = pv[v])
                {
                    var e = _edges[pv[v]][pe[v]];
                    _edges[pv[v]][pe[v]] = new InternalEdge(e.To, e.Rev, e.Cap - c, e.Cost);
                    var re = _edges[v][e.Rev];
                    _edges[v][e.Rev] = new InternalEdge(re.To, re.Rev, re.Cap + c, re.Cost);
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