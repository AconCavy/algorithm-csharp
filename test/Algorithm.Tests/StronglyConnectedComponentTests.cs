using System;
using System.Linq;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class StronglyConnectedComponentTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new StronglyConnectedComponent(10));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new StronglyConnectedComponent(-1));
        }

        [Test]
        public void EmptyTest()
        {
            var scc0 = new StronglyConnectedComponent();
            Assert.That(scc0.GetGraph().Count, Is.Zero);

            var scc1 = new StronglyConnectedComponent();
            Assert.That(scc1.GetGraph().Count, Is.Zero);
        }

        [Test]
        public void SimpleGraphTest()
        {
            var scc = new StronglyConnectedComponent(2);
            scc.AddEdge(0, 1);
            scc.AddEdge(1, 0);
            var graph = scc.GetGraph();
            Assert.That(graph.Count, Is.EqualTo(1));
        }

        [Test]
        public void SelfLoopGraphTest()
        {
            var scc = new StronglyConnectedComponent(2);
            scc.AddEdge(0, 0);
            scc.AddEdge(0, 0);
            scc.AddEdge(1, 1);
            var graph = scc.GetGraph();
            Assert.That(graph.Count, Is.EqualTo(2));
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        public void InvalidAddEdgeTest(int u, int v)
        {
            var scc = new StronglyConnectedComponent(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => scc.AddEdge(u, v));
        }
    }
}