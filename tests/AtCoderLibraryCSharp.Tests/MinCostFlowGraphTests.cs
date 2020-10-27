using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class MinCostFlowGraphTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new MinCostFlowGraph());
            Assert.DoesNotThrow(() => _ = new MinCostFlowGraph(10));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new MinCostFlowGraph(-1));
        }

        [Test]
        public void SimpleFlowTest()
        {
            var mcfg = new MinCostFlowGraph(4);
            mcfg.AddEdge(0, 1, 1, 1);
            mcfg.AddEdge(0, 2, 1, 1);
            mcfg.AddEdge(1, 3, 1, 1);
            mcfg.AddEdge(2, 3, 1, 1);
            mcfg.AddEdge(1, 2, 1, 1);
            var actual = mcfg.Slope(0, 3, 10).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((2, 4)));

            AssertEdge(mcfg.GetEdge(0), new MinCostFlowGraph.Edge(0, 1, 1, 1, 1));
            AssertEdge(mcfg.GetEdge(1), new MinCostFlowGraph.Edge(0, 2, 1, 1, 1));
            AssertEdge(mcfg.GetEdge(2), new MinCostFlowGraph.Edge(1, 3, 1, 1, 1));
            AssertEdge(mcfg.GetEdge(3), new MinCostFlowGraph.Edge(2, 3, 1, 1, 1));
            AssertEdge(mcfg.GetEdge(4), new MinCostFlowGraph.Edge(1, 2, 1, 0, 1));
        }

        [Test]
        public void UsageTest()
        {
            var mcfg = new MinCostFlowGraph(2);
            mcfg.AddEdge(0, 1, 1, 2);
            Assert.That(mcfg.Flow(0, 1), Is.EqualTo((1, 2)));

            mcfg = new MinCostFlowGraph(2);
            mcfg.AddEdge(0, 1, 1, 2);
            var actual = mcfg.Slope(0, 1).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((1, 2)));
        }

        [Test]
        public void SelfLoopTest()
        {
            var mcfg = new MinCostFlowGraph(3);
            Assert.That(mcfg.AddEdge(0, 0, 100, 123), Is.Zero);
            AssertEdge(mcfg.GetEdge(0), new MinCostFlowGraph.Edge(0, 0, 100, 0, 123));
        }

        [Test]
        public void SameCostPathsTest()
        {
            var mcfg = new MinCostFlowGraph(3);
            Assert.That(mcfg.AddEdge(0, 1, 1, 1), Is.Zero);
            Assert.That(mcfg.AddEdge(1, 2, 1, 0), Is.EqualTo(1));
            Assert.That(mcfg.AddEdge(0, 2, 2, 1), Is.EqualTo(2));
            var actual = mcfg.Slope(0, 2).ToArray();
            Assert.That(actual[0], Is.EqualTo((0, 0)));
            Assert.That(actual[1], Is.EqualTo((3, 3)));
        }

        [Test]
        public void StressTest()
        {
            for (var ph = 0; ph < 1000; ph++)
            {
                var n = Utilities.RandomInteger(2, 20);
                var m = Utilities.RandomInteger(1, 100);
                var (s, t) = Utilities.RandomPair(0, n - 1);
                if (Utilities.RandomBool()) (s, t) = (t, s);

                var mfg = new MaxFlowGraph(n);
                var mcfg = new MinCostFlowGraph(n);
                for (var i = 0; i < m; i++)
                {
                    var u = Utilities.RandomInteger(0, n - 1);
                    var v = Utilities.RandomInteger(0, n - 1);
                    var cap = Utilities.RandomInteger(0, 10);
                    var cost = Utilities.RandomInteger(0, 10000);
                    mcfg.AddEdge(u, v, cap, cost);
                    mfg.AddEdge(u, v, cap);
                }

                var (flow, cost1) = mcfg.Flow(s, t);
                Assert.That(flow, Is.EqualTo(mfg.Flow(s, t)));

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
                {
                    if (i == s) Assert.That(capacities[i], Is.EqualTo(-flow));
                    else if (i == t) Assert.That(capacities[i], Is.EqualTo(flow));
                    else Assert.That(capacities[i], Is.EqualTo(0));
                }

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
        public void ArgumentOutOfRangeInAddEdge([Values(-1, 2)] int v)
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => mcfg.AddEdge(v, 0, 10, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => mcfg.AddEdge(0, v, 10, 1));
        }

        [Test]
        public void InvalidArgumentInAddEdge()
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<ArgumentException>(() => mcfg.AddEdge(0, 0, -1, 0));
            Assert.Throws<ArgumentException>(() => mcfg.AddEdge(0, 0, 0, -1));
        }

        [Test]
        public void ArgumentOutOfRangeInGetEdge([Values(-1, 1)] int v)
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => mcfg.GetEdge(v));
        }

        [TestCase(-1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 2)]
        public void ArgumentOutOfRangeInSlope(int u, int v)
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => mcfg.Slope(u, v));
        }

        [Test]
        public void InvalidArgumentInSlope()
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<ArgumentException>(() => mcfg.Slope(1, 1));
        }

        private static void AssertEdge(MinCostFlowGraph.Edge actual, MinCostFlowGraph.Edge expected)
        {
            Assert.That(actual.From, Is.EqualTo(expected.From));
            Assert.That(actual.To, Is.EqualTo(expected.To));
            Assert.That(actual.Capacity, Is.EqualTo(expected.Capacity));
            Assert.That(actual.Flow, Is.EqualTo(expected.Flow));
            Assert.That(actual.Cost, Is.EqualTo(expected.Cost));
        }
    }
}