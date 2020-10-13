using System;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class LowestCommonAncestorTests
    {
        [Test]
        public void InitializeTest()
        {
            var tree = new[] {new[] {1}, new int[0]};
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(tree));

            tree = new[] {new[] {1}, new[] {0}};
            Assert.DoesNotThrow(() => _ = new LowestCommonAncestor(tree));
        }

        [Test]
        public void FindTest()
        {
            var tree = new[]
            {
                new[] {1, 2},
                new[] {0, 3, 4}, new[] {0, 5},
                new[] {1}, new[] {1}, new[] {2}
            };
            var lca = new LowestCommonAncestor(tree);
            Assert.That(lca.Find(0, 1), Is.EqualTo(0));
            Assert.That(lca.Find(1, 2), Is.EqualTo(0));
            Assert.That(lca.Find(3, 5), Is.EqualTo(0));
            Assert.That(lca.Find(1, 3), Is.EqualTo(1));
            Assert.That(lca.Find(1, 4), Is.EqualTo(1));
            Assert.That(lca.Find(3, 4), Is.EqualTo(1));
            Assert.That(lca.Find(4, 3), Is.EqualTo(1));
        }

        [Test]
        public void GetAncestorTest()
        {
            var tree = new[]
            {
                new[] {1, 2},
                new[] {0, 3, 4}, new[] {0, 5},
                new[] {1}, new[] {1}, new[] {2}
            };
            var lca = new LowestCommonAncestor(tree);
            Assert.That(lca.GetAncestor(0, 0), Is.EqualTo(0));
            Assert.That(lca.GetAncestor(0, 3), Is.EqualTo(-1));
            Assert.That(lca.GetAncestor(1, 1), Is.EqualTo(0));
            Assert.That(lca.GetAncestor(3, 1), Is.EqualTo(1));
            Assert.That(lca.GetAncestor(3, 2), Is.EqualTo(0));
        }

        [Test]
        public void GetDistanceTest()
        {
            var tree = new[]
            {
                new[] {1, 2},
                new[] {0, 3, 4}, new[] {0, 5},
                new[] {1}, new[] {1}, new[] {2}
            };
            var lca = new LowestCommonAncestor(tree);
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
        public void RootNot0Test()
        {
            var tree = new[]
            {
                new[] {1, 2},
                new[] {0, 3, 4}, new[] {0, 5},
                new[] {1}, new[] {1}, new[] {2}
            };
            var lca = new LowestCommonAncestor(tree, 2);
            Assert.That(lca.Find(0, 5), Is.EqualTo(2));
            Assert.That(lca.Find(1, 5), Is.EqualTo(2));
            Assert.That(lca.Find(3, 5), Is.EqualTo(2));
            Assert.That(lca.GetAncestor(0, 0), Is.EqualTo(0));
            Assert.That(lca.GetAncestor(0, 3), Is.EqualTo(-1));
            Assert.That(lca.GetAncestor(5, 3), Is.EqualTo(-1));
            Assert.That(lca.GetAncestor(3, 3), Is.EqualTo(2));
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
        public void OneWayTreeTest()
        {
            var tree = new[]
            {
                new[] {1, 2},
                new[] {3, 4}, new[] {5},
                new int[0], new int[0], new int[0]
            };
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
        public void InvalidArgumentsTest()
        {
            var tree = new[] {new[] {1}, new int[0]};
            Assert.Throws<IndexOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, -1));
            Assert.Throws<IndexOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, 2));

            tree = new[] {new[] {1}, new[] {0}};
            Assert.Throws<IndexOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, -1));
            Assert.Throws<IndexOutOfRangeException>(() => _ = new LowestCommonAncestor(tree, 2));

            tree = new[]
            {
                new[] {1, 2},
                new[] {3, 4}, new[] {5},
                new int[0], new int[0], new int[0]
            };
            var lca = new LowestCommonAncestor(tree);
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.Find(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.Find(6, 0));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.Find(0, -1));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.Find(0, 6));

            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetAncestor(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetAncestor(6, 0));

            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetDistance(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetDistance(6, 0));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetDistance(0, -1));
            Assert.Throws<IndexOutOfRangeException>(() => _ = lca.GetDistance(0, 6));
        }
    }
}