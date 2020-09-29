using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class StronglyConnectedComponentTests
    {
        [Test]
        public void EmptyTest()
        {
            var scc0 = new StronglyConnectedComponent();
            Assert.That(scc0.GetGraph().Count(), Is.Zero);
            
            var scc1 = new StronglyConnectedComponent(0);
            Assert.That(scc1.GetGraph().Count(), Is.Zero);
        }

        [Test]
        public void AssignTest()
        {
            Assert.DoesNotThrow(() => _ = new StronglyConnectedComponent(10));
        }

        [Test]
        public void SimpleGraphTest()
        {
            var scc = new StronglyConnectedComponent(2);
            scc.AddEdge(0, 1);
            scc.AddEdge(1, 0);
            var graph = scc.GetGraph().ToArray();
            Assert.That(graph.Length, Is.EqualTo(1));
        }

        [Test]
        public void SelfLoopGraphTest()
        {
            var scc = new StronglyConnectedComponent(2);
            scc.AddEdge(0, 0);
            scc.AddEdge(0, 0);
            scc.AddEdge(1, 1);
            var graph = scc.GetGraph().ToArray();
            Assert.That(graph.Length, Is.EqualTo(2));
        }

        [Test]
        public void InvalidAddEdgeTest()
        {
            var scc = new StronglyConnectedComponent(2);
            Assert.Throws<IndexOutOfRangeException>(()=>scc.AddEdge(0, 10));
            Assert.Throws<IndexOutOfRangeException>(()=>scc.AddEdge(10, 0));
        }
    }
}