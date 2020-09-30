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
        public void InvalidArgumentsTest()
        {
            var mcfg = new MinCostFlowGraph(2);
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.AddEdge(-1, 0, 10, 1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.AddEdge(2, 0, 10, 1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.AddEdge(0, -1, 10, 1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.AddEdge(0, 2, 10, 1));
            Assert.Throws<ArgumentException>(() => mcfg.AddEdge(0, 0, -1, 0));
            Assert.Throws<ArgumentException>(() => mcfg.AddEdge(0, 0, 0, -1));

            Assert.Throws<IndexOutOfRangeException>(() => mcfg.GetEdge(-1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.GetEdge(1));

            Assert.Throws<IndexOutOfRangeException>(() => mcfg.Slope(-1, 1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.Slope(2, 1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.Slope(1, -1));
            Assert.Throws<IndexOutOfRangeException>(() => mcfg.Slope(1, 2));
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