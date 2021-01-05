using System;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class FlowGraphTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new FlowGraph());
            Assert.DoesNotThrow(() => _ = new FlowGraph(10));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FlowGraph(-1));
        }

        [Test]
        public void MaxFlowSimpleTest()
        {
            var fg = new FlowGraph(4);
            Assert.That(fg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(fg.AddEdge(0, 2, 1), Is.EqualTo(1));
            Assert.That(fg.AddEdge(1, 3, 1), Is.EqualTo(2));
            Assert.That(fg.AddEdge(2, 3, 1), Is.EqualTo(3));
            Assert.That(fg.AddEdge(1, 2, 1), Is.EqualTo(4));
            Assert.That(fg.MaxFlow(0, 3), Is.EqualTo(2));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 1, 1)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 2, 1, 1)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(1, 3, 1, 1)));
            Assert.That(fg.GetEdge(3), Is.EqualTo(new FlowGraph.Edge(2, 3, 1, 1)));
            Assert.That(fg.GetEdge(4), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 0)));
        }

        [Test]
        public void MaxFlowMinCutTest()
        {
            var fg = new FlowGraph(3);
            Assert.That(fg.AddEdge(0, 1, 2), Is.Zero);
            Assert.That(fg.AddEdge(1, 2, 1), Is.EqualTo(1));
            Assert.That(fg.MaxFlow(0, 2), Is.EqualTo(1));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 2, 1)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 1)));

            var minCut = fg.MinCut(0).ToArray();
            Assert.That(minCut[0], Is.True);
            Assert.That(minCut[1], Is.True);
            Assert.That(minCut[2], Is.False);
        }

        [Test]
        public void MaxFlowComplexTest()
        {
            var fg = new FlowGraph(2);
            Assert.That(fg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(fg.AddEdge(0, 1, 2), Is.EqualTo(1));
            Assert.That(fg.AddEdge(0, 1, 3), Is.EqualTo(2));
            Assert.That(fg.AddEdge(0, 1, 4), Is.EqualTo(3));
            Assert.That(fg.AddEdge(0, 1, 5), Is.EqualTo(4));
            Assert.That(fg.AddEdge(0, 0, 6), Is.EqualTo(5));
            Assert.That(fg.AddEdge(1, 1, 7), Is.EqualTo(6));
            Assert.That(fg.MaxFlow(0, 1), Is.EqualTo(15));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 1, 1)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 1, 2, 2)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(0, 1, 3, 3)));
            Assert.That(fg.GetEdge(3), Is.EqualTo(new FlowGraph.Edge(0, 1, 4, 4)));
            Assert.That(fg.GetEdge(4), Is.EqualTo(new FlowGraph.Edge(0, 1, 5, 5)));

            var minCut = fg.MinCut(0).ToArray();
            Assert.That(minCut[0], Is.True);
            Assert.That(minCut[1], Is.False);
        }

        [Test]
        public void MaxFlowTwiceTest()
        {
            var fg = new FlowGraph(3);
            Assert.That(fg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(fg.AddEdge(0, 2, 1), Is.EqualTo(1));
            Assert.That(fg.AddEdge(1, 2, 1), Is.EqualTo(2));
            Assert.That(fg.MaxFlow(0, 2), Is.EqualTo(2));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 1, 1)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 2, 1, 1)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 1)));

            fg.ChangeEdge(0, 100, 10);
            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 100, 10)));

            Assert.That(fg.MaxFlow(0, 2), Is.Zero);
            Assert.That(fg.MaxFlow(0, 1), Is.EqualTo(90));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 100, 100)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 2, 1, 1)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 1)));

            Assert.That(fg.MaxFlow(2, 0), Is.EqualTo(2));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 100, 99)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 2, 1, 0)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 0)));
        }

        [Test]
        public void MaxFlowSelfLoopTest()
        {
            var fg = new FlowGraph(3);
            Assert.That(fg.AddEdge(0, 0, 100), Is.Zero);

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 0, 100, 0)));
        }

        [Test]
        public void MaxFlowStressTest()
        {
            for (var ph = 0; ph < 10000; ph++)
            {
                var n = Utilities.RandomInteger(2, 20);
                var m = Utilities.RandomInteger(1, 100);
                var (s, t) = Utilities.RandomPair(0, n - 1);
                if (Utilities.RandomBool()) (s, t) = (t, s);

                var fg = new FlowGraph(n);
                for (var i = 0; i < m; i++)
                {
                    var u = Utilities.RandomInteger(0, n - 1);
                    var v = Utilities.RandomInteger(0, n - 1);
                    var c = Utilities.RandomInteger(0, 10000);
                    fg.AddEdge(u, v, c);
                }

                var flow = fg.MaxFlow(s, t);
                var dual = 0L;
                var minCut = fg.MinCut(s).ToArray();
                var flows = new long[n];
                foreach (var edge in fg.GetEdges())
                {
                    flows[edge.From] -= edge.Flow;
                    flows[edge.To] += edge.Flow;
                    if (minCut[edge.From] && !minCut[edge.To]) dual += edge.Capacity;
                }

                Assert.That(dual, Is.EqualTo(flow));
                Assert.That(flows[s], Is.EqualTo(-flow));
                Assert.That(flows[t], Is.EqualTo(flow));

                for (var i = 0; i < n; i++)
                {
                    if (i == s || i == t) continue;
                    Assert.That(flows[i], Is.Zero);
                }
            }
        }

        [Test]
        public void MinCostFlowSimpleTest()
        {
            var fg = new FlowGraph(4);
            fg.AddEdge(0, 1, 1, 1);
            fg.AddEdge(0, 2, 1, 1);
            fg.AddEdge(1, 3, 1, 1);
            fg.AddEdge(2, 3, 1, 1);
            fg.AddEdge(1, 2, 1, 1);
            var actual = fg.MinCostSlope(0, 3, 10).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((2, 4)));

            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 1, 1, 1, 1)));
            Assert.That(fg.GetEdge(1), Is.EqualTo(new FlowGraph.Edge(0, 2, 1, 1, 1)));
            Assert.That(fg.GetEdge(2), Is.EqualTo(new FlowGraph.Edge(1, 3, 1, 1, 1)));
            Assert.That(fg.GetEdge(3), Is.EqualTo(new FlowGraph.Edge(2, 3, 1, 1, 1)));
            Assert.That(fg.GetEdge(4), Is.EqualTo(new FlowGraph.Edge(1, 2, 1, 0, 1)));
        }

        [Test]
        public void UsageTest()
        {
            var fg = new FlowGraph(2);
            fg.AddEdge(0, 1, 1, 2);
            Assert.That(fg.MinCostFlow(0, 1), Is.EqualTo((1, 2)));

            fg = new FlowGraph(2);
            fg.AddEdge(0, 1, 1, 2);
            var actual = fg.MinCostSlope(0, 1).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((1, 2)));
        }

        [Test]
        public void MinCostFlowSelfLoopTest()
        {
            var fg = new FlowGraph(3);
            Assert.That(fg.AddEdge(0, 0, 100, 123), Is.Zero);
            Assert.That(fg.GetEdge(0), Is.EqualTo(new FlowGraph.Edge(0, 0, 100, 0, 123)));
        }

        [Test]
        public void MinCostFlowSameCostPathsTest()
        {
            var fg = new FlowGraph(3);
            Assert.That(fg.AddEdge(0, 1, 1, 1), Is.Zero);
            Assert.That(fg.AddEdge(1, 2, 1), Is.EqualTo(1));
            Assert.That(fg.AddEdge(0, 2, 2, 1), Is.EqualTo(2));
            var actual = fg.MinCostSlope(0, 2).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((3, 3)));
        }

        [Test]
        public void MinCostFlowStressTest()
        {
            for (var ph = 0; ph < 1000; ph++)
            {
                var n = Utilities.RandomInteger(2, 20);
                var m = Utilities.RandomInteger(1, 100);
                var (s, t) = Utilities.RandomPair(0, n - 1);
                if (Utilities.RandomBool()) (s, t) = (t, s);

                var mfg = new FlowGraph(n);
                var mcfg = new FlowGraph(n);
                for (var i = 0; i < m; i++)
                {
                    var u = Utilities.RandomInteger(0, n - 1);
                    var v = Utilities.RandomInteger(0, n - 1);
                    var cap = Utilities.RandomInteger(0, 10);
                    var cost = Utilities.RandomInteger(0, 10000);
                    mcfg.AddEdge(u, v, cap, cost);
                    mfg.AddEdge(u, v, cap);
                }

                var (flow, cost1) = mcfg.MinCostFlow(s, t);
                Assert.That(flow, Is.EqualTo(mfg.MaxFlow(s, t)));

                var cost2 = 0L;
                var capacities = new long[n];
                foreach (var edge in mcfg.GetEdges())
                {
                    capacities[edge.From] -= edge.Flow;
                    capacities[edge.To] += edge.Flow;
                    cost2 += edge.Flow * edge.Cost;
                }

                Assert.That(cost1, Is.EqualTo(cost2));

                for (var i = 0; i < n; i++)
                    if (i == s) Assert.That(capacities[i], Is.EqualTo(-flow));
                    else if (i == t) Assert.That(capacities[i], Is.EqualTo(flow));
                    else Assert.That(capacities[i], Is.EqualTo(0));

                Assert.DoesNotThrow(() =>
                {
                    var dist = new long[n];
                    while (true)
                    {
                        var update = false;
                        foreach (var edge in mcfg.GetEdges())
                        {
                            if (edge.Flow < edge.Capacity)
                            {
                                var ndist = dist[edge.From] + edge.Cost;
                                if (ndist < dist[edge.To])
                                {
                                    update = true;
                                    dist[edge.To] = ndist;
                                }
                            }

                            if (edge.Flow == 0) continue;
                            {
                                var ndist = dist[edge.To] - edge.Cost;
                                if (ndist < dist[edge.From])
                                {
                                    update = true;
                                    dist[edge.From] = ndist;
                                }
                            }
                        }

                        if (!update) break;
                    }
                });
            }
        }

        [Test]
        public void ArgumentOutOfRangeExceptionInAddEdgeAndGetEdge([Values(-1, 3)] int v)
        {
            var fg = new FlowGraph(3);
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.AddEdge(v, 1, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.AddEdge(0, v, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.GetEdge(v));
        }

        [Test]
        public void ArgumentExceptionInAddEdge()
        {
            var fg = new FlowGraph(2);
            Assert.Throws<ArgumentException>(() => fg.AddEdge(0, 0, -1));
            Assert.Throws<ArgumentException>(() => fg.AddEdge(0, 0, 0, -1));
        }

        [Test]
        public void ArgumentOutOfRangeExceptionInChangeEdge([Values(-1, 3)] int v)
        {
            var fg = new FlowGraph(3);
            fg.AddEdge(0, 1, 100);
            fg.AddEdge(1, 2, 100);
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.ChangeEdge(v, 100, 10));
        }

        [Test]
        public void ArgumentExceptionInChangeEdge([Values(-10, 100)] int newFlow)
        {
            var fg = new FlowGraph(3);
            fg.AddEdge(0, 1, 100);
            fg.AddEdge(1, 2, 100);
            Assert.Throws<ArgumentException>(() => fg.ChangeEdge(1, 10, newFlow));
        }

        [TestCase(-1, 0)]
        [TestCase(0, 3)]
        [TestCase(-1, 3)]
        public void ArgumentOutOfRangeExceptionInMaxFlow(int u, int v)
        {
            var fg = new FlowGraph(3);
            fg.AddEdge(0, 1, 100);
            fg.AddEdge(1, 2, 100);
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.MaxFlow(u, v));
        }

        [Test]
        public void ArgumentExceptionInMaxFlow()
        {
            var fg = new FlowGraph(3);
            fg.AddEdge(0, 1, 100);
            fg.AddEdge(1, 2, 100);
            Assert.Throws<ArgumentException>(() => fg.MaxFlow(0, 0));
            Assert.Throws<ArgumentException>(() => fg.MaxFlow(0, 0, 0));
        }

        [TestCase(-1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 2)]
        public void ArgumentOutOfRangeExceptionInMinCostSlope(int u, int v)
        {
            var fg = new FlowGraph(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => fg.MinCostSlope(u, v));
        }

        [Test]
        public void ArgumentExceptionInMinCostSlope()
        {
            var fg = new FlowGraph(2);
            Assert.Throws<ArgumentException>(() => fg.MinCostSlope(1, 1));
        }
    }
}