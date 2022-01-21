using System;
using System.Linq;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class DisjointSetUnionTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new DisjointSetUnion(2));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new DisjointSetUnion(-1));
        }

        [Test]
        public void SimpleTest()
        {
            var dsu = new DisjointSetUnion(2);
            var x = dsu.Merge(0, 1);
            Assert.That(dsu.LeaderOf(0), Is.EqualTo(x));
            Assert.That(dsu.LeaderOf(1), Is.EqualTo(x));
            Assert.That(dsu.IsSame(0, 1), Is.True);
            Assert.That(dsu.SizeOf(0), Is.EqualTo(2));
        }

        [Test]
        public void LineTest()
        {
            const int n = 500000;
            var dsu = new DisjointSetUnion(n);
            for (var i = 0; i < n - 1; i++)
                dsu.Merge(i, i + 1);
            Assert.That(dsu.SizeOf(0), Is.EqualTo(n));
            Assert.That(dsu.GetGroups().Count, Is.EqualTo(1));
        }

        [Test]
        public void LineReverseTest()
        {
            const int n = 500000;
            var dsu = new DisjointSetUnion(n);
            for (var i = n - 2; i >= 0; i--)
                dsu.Merge(i, i + 1);
            Assert.That(dsu.SizeOf(0), Is.EqualTo(n));
            Assert.That(dsu.GetGroups().Count, Is.EqualTo(1));
        }

        [TestCase(-1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 2)]
        [TestCase(-1, 2)]
        public void ArgumentOutOfRangeInMergeAndIsSameTest(int u, int v)
        {
            var dsu = new DisjointSetUnion(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => dsu.Merge(u, v));
            Assert.Throws<ArgumentOutOfRangeException>(() => dsu.IsSame(u, v));
        }

        [Test]
        public void ArgumentOutOfRangeInLeaderOfAndSizeOfTest([Values(-1, 2)] int v)
        {
            var dsu = new DisjointSetUnion(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => dsu.LeaderOf(v));
            Assert.Throws<ArgumentOutOfRangeException>(() => dsu.SizeOf(v));
        }


        [Test]
        public void SameLeaderTest()
        {
            var dsu = new DisjointSetUnion(3);
            dsu.Merge(0, 1);
            dsu.Merge(0, 2);
            Assert.That(dsu.Merge(1, 2), Is.Zero);
        }
    }
}