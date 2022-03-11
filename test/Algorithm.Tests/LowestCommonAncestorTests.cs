using System;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class LowestCommonAncestorTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(1, 1));
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(1));
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(2, 1));
            Assert.That(new LowestCommonAncestor(1).Length, Is.EqualTo(1));
        }

        [TestCase(0, 1, 0)]
        [TestCase(1, 2, 0)]
        [TestCase(3, 5, 0)]
        [TestCase(1, 3, 1)]
        [TestCase(1, 4, 1)]
        [TestCase(3, 4, 1)]
        [TestCase(4, 3, 1)]
        public void FindTest(int u, int v, int p)
        {
            var sut = InitializeLca();

            Assert.That(sut.Find(u, v), Is.EqualTo(p));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 3, -1)]
        [TestCase(1, 1, 0)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 2, 0)]
        public void GetAncestorTest(int v, int height, int p)
        {
            var sut = InitializeLca();

            Assert.That(sut.GetAncestor(v, height), Is.EqualTo(p));
        }

        [TestCase(0, 5, 2)]
        [TestCase(1, 5, 2)]
        [TestCase(3, 5, 2)]
        public void FindInRoot2Test(int u, int v, int p)
        {
            var sut = InitializeLca(2);

            Assert.That(sut.Find(u, v), Is.EqualTo(p));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 3, -1)]
        [TestCase(5, 3, -1)]
        [TestCase(3, 2, 0)]
        public void GetAncestorInRoot2Test(int v, int h, int p)
        {
            var sut = InitializeLca(2);

            Assert.That(sut.GetAncestor(v, h), Is.EqualTo(p));
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        [TestCase(0, 3, 2)]
        [TestCase(0, 4, 2)]
        [TestCase(0, 5, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 3, 1)]
        [TestCase(1, 4, 1)]
        [TestCase(1, 5, 3)]
        [TestCase(2, 3, 3)]
        [TestCase(2, 4, 3)]
        [TestCase(2, 5, 1)]
        [TestCase(3, 4, 2)]
        [TestCase(3, 5, 4)]
        [TestCase(4, 5, 4)]
        public void GetDistanceAndCostTest(int u, int v, int c)
        {
            const int n = 6;
            for (var i = 0; i < n; i++)
            {
                var sut = new LowestCommonAncestor(6, i);
                sut.AddEdge(0, 1);
                sut.AddEdge(0, 2);
                sut.AddEdge(1, 3);
                sut.AddEdge(1, 4);
                sut.AddEdge(2, 5);

                Assert.That(sut.GetDistance(u, v), Is.EqualTo(c));
                Assert.That(sut.GetDistance(v, u), Is.EqualTo(c));
                Assert.That(sut.GetCost(u, v), Is.EqualTo(c));
                Assert.That(sut.GetCost(v, u), Is.EqualTo(c));
            }
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 3)]
        [TestCase(0, 3, 8)]
        [TestCase(0, 4, 5)]
        [TestCase(0, 5, 15)]
        [TestCase(1, 2, 4)]
        [TestCase(1, 3, 9)]
        [TestCase(1, 4, 6)]
        [TestCase(1, 5, 16)]
        [TestCase(2, 3, 5)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 5, 12)]
        [TestCase(3, 4, 7)]
        [TestCase(3, 5, 7)]
        [TestCase(4, 5, 14)]
        public void GetCostTest(int u, int v, int c)
        {
            const int n = 6;
            const int m = 7;
            for (var i = 0; i < n; i++)
            {
                var sut = new LowestCommonAncestor(n, i);
                sut.AddEdge(0, 1, 1);
                sut.AddEdge(0, 2, 3);
                sut.AddEdge(2, 3, 5);
                sut.AddEdge(2, 4, 2);
                sut.AddEdge(3, 5, 7);

                Assert.That(sut.GetCost(u, v), Is.EqualTo(c));
                Assert.That(sut.GetCost(u, v, m), Is.EqualTo(c % m));
                Assert.That(sut.GetCost(v, u), Is.EqualTo(c));
            }
        }

        [Test]
        public void OneWayTreeTest()
        {
            var sut = InitializeLca();

            Assert.That(sut.Find(0, 1), Is.EqualTo(0));
            Assert.That(sut.Find(1, 2), Is.EqualTo(0));
            Assert.That(sut.Find(3, 5), Is.EqualTo(0));
            Assert.That(sut.Find(1, 3), Is.EqualTo(1));
            Assert.That(sut.Find(1, 4), Is.EqualTo(1));
            Assert.That(sut.Find(3, 4), Is.EqualTo(1));
            Assert.That(sut.Find(4, 3), Is.EqualTo(1));
            Assert.That(sut.GetAncestor(0, 0), Is.EqualTo(0));
            Assert.That(sut.GetAncestor(0, 3), Is.EqualTo(-1));
            Assert.That(sut.GetAncestor(1, 1), Is.EqualTo(0));
            Assert.That(sut.GetAncestor(3, 1), Is.EqualTo(1));
            Assert.That(sut.GetAncestor(3, 2), Is.EqualTo(0));
            Assert.That(sut.GetDistance(0, 1), Is.EqualTo(1));
            Assert.That(sut.GetDistance(0, 2), Is.EqualTo(1));
            Assert.That(sut.GetDistance(0, 3), Is.EqualTo(2));
            Assert.That(sut.GetDistance(0, 4), Is.EqualTo(2));
            Assert.That(sut.GetDistance(0, 5), Is.EqualTo(2));
            Assert.That(sut.GetDistance(3, 0), Is.EqualTo(2));
            Assert.That(sut.GetDistance(3, 2), Is.EqualTo(3));
            Assert.That(sut.GetDistance(3, 5), Is.EqualTo(4));
        }

        [TestCase(-1, 0)]
        [TestCase(6, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 6)]
        public void ArgumentOutOfRangeInMethodTest(int u, int v)
        {
            const int length = 6;
            var sut = new LowestCommonAncestor(length);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.AddEdge(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = sut.Find(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = sut.GetDistance(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = sut.GetCost(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = sut.GetCost(u, v, 7));
            if (v < 0 || length <= v) return;
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = sut.GetAncestor(u, 0));
        }

        private static LowestCommonAncestor InitializeLca(int root = 0)
        {
            var lca = new LowestCommonAncestor(6, root);
            lca.AddEdge(0, 1);
            lca.AddEdge(0, 2);
            lca.AddEdge(1, 3);
            lca.AddEdge(1, 4);
            lca.AddEdge(2, 5);

            return lca;
        }
    }
}