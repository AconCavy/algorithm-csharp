using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class MaxFlowGraphTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new MaxFlowGraph());
            Assert.DoesNotThrow(() => _ = new MaxFlowGraph(10));
        }

        [Test]
        public void SimpleFlowTest()
        {
            var mfg = new MaxFlowGraph(4);
            Assert.That(mfg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(mfg.AddEdge(0, 2, 1), Is.EqualTo(1));
            Assert.That(mfg.AddEdge(1, 3, 1), Is.EqualTo(2));
            Assert.That(mfg.AddEdge(2, 3, 1), Is.EqualTo(3));
            Assert.That(mfg.AddEdge(1, 2, 1), Is.EqualTo(4));
            Assert.That(mfg.Flow(0, 3), Is.EqualTo(2));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 1, 1));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(0, 2, 1, 1));
            AssertEdge(mfg.GetEdge(2), new MaxFlowGraph.Edge(1, 3, 1, 1));
            AssertEdge(mfg.GetEdge(3), new MaxFlowGraph.Edge(2, 3, 1, 1));
            AssertEdge(mfg.GetEdge(4), new MaxFlowGraph.Edge(1, 2, 1, 0));
        }

        [Test]
        public void ComplexFlowTest()
        {
            var mfg = new MaxFlowGraph(2);
            Assert.That(mfg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(mfg.AddEdge(0, 1, 2), Is.EqualTo(1));
            Assert.That(mfg.AddEdge(0, 1, 3), Is.EqualTo(2));
            Assert.That(mfg.AddEdge(0, 1, 4), Is.EqualTo(3));
            Assert.That(mfg.AddEdge(0, 1, 5), Is.EqualTo(4));
            Assert.That(mfg.AddEdge(0, 0, 6), Is.EqualTo(5));
            Assert.That(mfg.AddEdge(1, 1, 7), Is.EqualTo(6));
            Assert.That(mfg.Flow(0, 1), Is.EqualTo(15));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 1, 1));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(0, 1, 2, 2));
            AssertEdge(mfg.GetEdge(2), new MaxFlowGraph.Edge(0, 1, 3, 3));
            AssertEdge(mfg.GetEdge(3), new MaxFlowGraph.Edge(0, 1, 4, 4));
            AssertEdge(mfg.GetEdge(4), new MaxFlowGraph.Edge(0, 1, 5, 5));

            var minCut = mfg.MinCut(0).ToArray();
            Assert.That(minCut[0], Is.True);
            Assert.That(minCut[1], Is.False);
        }

        [Test]
        public void CutTest()
        {
            var mfg = new MaxFlowGraph(3);
            Assert.That(mfg.AddEdge(0, 1, 2), Is.Zero);
            Assert.That(mfg.AddEdge(1, 2, 1), Is.EqualTo(1));
            Assert.That(mfg.Flow(0, 2), Is.EqualTo(1));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 2, 1));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(1, 2, 1, 1));

            var minCut = mfg.MinCut(0).ToArray();
            Assert.That(minCut[0], Is.True);
            Assert.That(minCut[1], Is.True);
            Assert.That(minCut[2], Is.False);
        }

        [Test]
        public void TwiceTest()
        {
            var mfg = new MaxFlowGraph(3);
            Assert.That(mfg.AddEdge(0, 1, 1), Is.Zero);
            Assert.That(mfg.AddEdge(0, 2, 1), Is.EqualTo(1));
            Assert.That(mfg.AddEdge(1, 2, 1), Is.EqualTo(2));
            Assert.That(mfg.Flow(0, 2), Is.EqualTo(2));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 1, 1));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(0, 2, 1, 1));
            AssertEdge(mfg.GetEdge(2), new MaxFlowGraph.Edge(1, 2, 1, 1));

            mfg.ChangeEdge(0, 100, 10);
            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 100, 10));

            Assert.That(mfg.Flow(0, 2), Is.Zero);
            Assert.That(mfg.Flow(0, 1), Is.EqualTo(90));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 100, 100));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(0, 2, 1, 1));
            AssertEdge(mfg.GetEdge(2), new MaxFlowGraph.Edge(1, 2, 1, 1));

            Assert.That(mfg.Flow(2, 0), Is.EqualTo(2));

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 1, 100, 99));
            AssertEdge(mfg.GetEdge(1), new MaxFlowGraph.Edge(0, 2, 1, 0));
            AssertEdge(mfg.GetEdge(2), new MaxFlowGraph.Edge(1, 2, 1, 0));
        }

        [Test]
        public void SelfLoopTest()
        {
            var mfg = new MaxFlowGraph(3);
            Assert.That(mfg.AddEdge(0, 0, 100), Is.Zero);

            AssertEdge(mfg.GetEdge(0), new MaxFlowGraph.Edge(0, 0, 100, 0));
        }

        [Test]
        public void InvalidArgumentsTest()
        {
            var mfg = new MaxFlowGraph(3);
            Assert.Throws<IndexOutOfRangeException>(() => mfg.AddEdge(-1, 1, 10));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.AddEdge(3, 1, 10));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.AddEdge(0, -1, 10));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.AddEdge(0, 3, 10));
            Assert.Throws<ArgumentException>(() => mfg.AddEdge(0, 1, -10));

            Assert.Throws<IndexOutOfRangeException>(() => mfg.GetEdge(-1));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.GetEdge(3));

            mfg.AddEdge(0, 1, 100);
            mfg.AddEdge(1, 2, 100);
            Assert.Throws<IndexOutOfRangeException>(() => mfg.ChangeEdge(-1, 100, 10));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.ChangeEdge(3, 100, 10));
            Assert.Throws<ArgumentException>(() => mfg.ChangeEdge(1, 10, -10));
            Assert.Throws<ArgumentException>(() => mfg.ChangeEdge(1, 10, 100));

            Assert.Throws<IndexOutOfRangeException>(() => mfg.Flow(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.Flow(0, 3));
            Assert.Throws<IndexOutOfRangeException>(() => mfg.Flow(-1, 3));
            Assert.Throws<ArgumentException>(() => mfg.Flow(0, 0));
            Assert.Throws<ArgumentException>(() => mfg.Flow(0, 0, 0));
        }

        [Test]
        public void StressTest()
        {
            for (var ph = 0; ph < 10000; ph++)
            {
                var n = Utilities.RandomInteger(2, 20);
                var m = Utilities.RandomInteger(1, 100);
                var (s, t) = Utilities.RandomPair(0, n - 1);
                if (Utilities.RandomBool()) (s, t) = (t, s);

                var mfg = new MaxFlowGraph(n);
                for (var i = 0; i < m; i++)
                {
                    var u = Utilities.RandomInteger(0, n - 1);
                    var v = Utilities.RandomInteger(0, n - 1);
                    var c = Utilities.RandomInteger(0, 10000);
                    mfg.AddEdge(u, v, c);
                }

                var flow = mfg.Flow(s, t);
                var dual = 0L;
                var minCut = mfg.MinCut(s).ToArray();
                var flows = new long[n];
                foreach (var edge in mfg.GetEdges())
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

        private static void AssertEdge(MaxFlowGraph.Edge actual, MaxFlowGraph.Edge expected)
        {
            Assert.That(actual.From, Is.EqualTo(expected.From));
            Assert.That(actual.To, Is.EqualTo(expected.To));
            Assert.That(actual.Capacity, Is.EqualTo(expected.Capacity));
            Assert.That(actual.Flow, Is.EqualTo(expected.Flow));
        }
    }
}