using System;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class LowestCommonAncestorTests
    {
        [Test]
        public void InitializeTest()
        {
            var tree = new[] {new[] {1}, new int[0]};
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(tree));
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(tree, 1));

            tree = new[] {new[] {1}, new[] {0}};
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(tree));

            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(1));
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(2, 1));
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
            var (byArray, byLength) = InitializeLca();

            Assert.That(byArray.Find(u, v), Is.EqualTo(p));
            Assert.That(byLength.Find(u, v), Is.EqualTo(p));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 3, -1)]
        [TestCase(1, 1, 0)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 2, 0)]
        public void GetAncestorTest(int v, int height, int p)
        {
            var (byArray, byLength) = InitializeLca();

            Assert.That(byArray.GetAncestor(v, height), Is.EqualTo(p));
            Assert.That(byLength.GetAncestor(v, height), Is.EqualTo(p));
        }

        [TestCase(0, 5, 2)]
        [TestCase(1, 5, 2)]
        [TestCase(3, 5, 2)]
        public void FindInRoot2Test(int u, int v, int p)
        {
            var (byArray, byLength) = InitializeLca(2);

            Assert.That(byArray.Find(u, v), Is.EqualTo(p));
            Assert.That(byLength.Find(u, v), Is.EqualTo(p));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 3, -1)]
        [TestCase(5, 3, -1)]
        [TestCase(3, 2, 0)]
        public void GetAncestorInRoot2Test(int v, int h, int p)
        {
            var (byArray, byLength) = InitializeLca(2);

            Assert.That(byArray.GetAncestor(v, h), Is.EqualTo(p));
            Assert.That(byLength.GetAncestor(v, h), Is.EqualTo(p));
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        [TestCase(0, 3, 2)]
        [TestCase(0, 4, 2)]
        [TestCase(0, 5, 2)]
        [TestCase(3, 0, 2)]
        [TestCase(3, 2, 3)]
        [TestCase(3, 5, 4)]
        public void GetDistanceTest(int u, int v, int d)
        {
            var lca1 = new LowestCommonAncestor(6);
            lca1.AddEdge(0, 1);
            lca1.AddEdge(0, 2);
            lca1.AddEdge(1, 3);
            lca1.AddEdge(1, 4);
            lca1.AddEdge(2, 5);
            var lca2 = new LowestCommonAncestor(6, 2);
            lca2.AddEdge(0, 1);
            lca2.AddEdge(0, 2);
            lca2.AddEdge(1, 3);
            lca2.AddEdge(1, 4);
            lca2.AddEdge(2, 5);

            Assert.That(lca1.GetDistance(u, v), Is.EqualTo(d));
            Assert.That(lca2.GetDistance(u, v), Is.EqualTo(d));
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        [TestCase(0, 3, 2)]
        [TestCase(0, 4, 2)]
        [TestCase(0, 5, 2)]
        [TestCase(3, 0, 2)]
        [TestCase(3, 2, 3)]
        [TestCase(3, 5, 4)]
        public void GetCostTest(int u, int v, int c)
        {
            var lca1 = new LowestCommonAncestor(6);
            lca1.AddEdge(0, 1, 1);
            lca1.AddEdge(0, 2, 1);
            lca1.AddEdge(1, 3, 1);
            lca1.AddEdge(1, 4, 1);
            lca1.AddEdge(2, 5, 1);
            var lca2 = new LowestCommonAncestor(6, 2);
            lca2.AddEdge(0, 1, 1);
            lca2.AddEdge(0, 2, 1);
            lca2.AddEdge(1, 3, 1);
            lca2.AddEdge(1, 4, 1);
            lca2.AddEdge(2, 5, 1);

            Assert.That(lca1.GetCost(u, v), Is.EqualTo(c));
            Assert.That(lca2.GetCost(u, v), Is.EqualTo(c));
        }

        [Test]
        public void OneWayTreeTest()
        {
            var tree = new[] {new[] {1, 2}, new[] {3, 4}, new[] {5}, new int[0], new int[0], new int[0]};
            var lca = new LowestCommonAncestor(tree);
            Assert.That(lca.Find(0, 1), Is.EqualTo(0));
            Assert.That(lca.Find(1, 2), Is.EqualTo(0));
            Assert.That(lca.Find(3, 5), Is.EqualTo(0));
            Assert.That(lca.Find(1, 3), Is.EqualTo(1));
            Assert.That(lca.Find(1, 4), Is.EqualTo(1));
            Assert.That(lca.Find(3, 4), Is.EqualTo(1));
            Assert.That(lca.Find(4, 3), Is.EqualTo(1));
            Assert.That(lca.GetAncestor(0, 0), Is.EqualTo(0));
            Assert.That(lca.GetAncestor(0, 3), Is.EqualTo(-1));
            Assert.That(lca.GetAncestor(1, 1), Is.EqualTo(0));
            Assert.That(lca.GetAncestor(3, 1), Is.EqualTo(1));
            Assert.That(lca.GetAncestor(3, 2), Is.EqualTo(0));
            Assert.That(lca.GetDistance(0, 1), Is.EqualTo(1));
            Assert.That(lca.GetDistance(0, 2), Is.EqualTo(1));
            Assert.That(lca.GetDistance(0, 3), Is.EqualTo(2));
            Assert.That(lca.GetDistance(0, 4), Is.EqualTo(2));
            Assert.That(lca.GetDistance(0, 5), Is.EqualTo(2));
            Assert.That(lca.GetDistance(3, 0), Is.EqualTo(2));
            Assert.That(lca.GetDistance(3, 2), Is.EqualTo(3));
            Assert.That(lca.GetDistance(3, 5), Is.EqualTo(4));
        }

        [Test]
        public void ArgumentOutOfRangeInInitializeTest([Values(-1, 2)] int root)
        {
            var tree = new[] {new[] {1}, new int[0]};
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, root));
            tree = new[] {new[] {1}, new[] {0}};
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, root));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LowestCommonAncestor(1, root));
        }

        [TestCase(-1, 0)]
        [TestCase(6, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 6)]
        public void ArgumentOutOfRangeInMethodTest(int u, int v)
        {
            const int length = 6;
            var lca = new LowestCommonAncestor(length);
            Assert.Throws<ArgumentOutOfRangeException>(() => lca.AddEdge(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lca.Find(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lca.GetDistance(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lca.GetCost(u, v));
            if (v < 0 || length <= v) return;
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lca.GetAncestor(u, 0));
        }

        private static (LowestCommonAncestor byArray, LowestCommonAncestor byLength) InitializeLca(int root = 0)
        {
            var lca1 = new LowestCommonAncestor(
                new[] {new[] {1, 2}, new[] {0, 3, 4}, new[] {0, 5}, new[] {1}, new[] {1}, new[] {2}}, root);
            var lca2 = new LowestCommonAncestor(6, root);
            lca2.AddEdge(0, 1);
            lca2.AddEdge(0, 2);
            lca2.AddEdge(1, 3);
            lca2.AddEdge(1, 4);
            lca2.AddEdge(2, 5);

            return (lca1, lca2);
        }
    }
}