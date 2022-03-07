using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class RandomizedBinarySearchTreeTests
    {
        [Test]
        public void InitializeTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            Assert.That(tree.Count, Is.EqualTo(0));

            tree = new RandomizedBinarySearchTree<int>(1);
            Assert.That(tree.Count, Is.EqualTo(0));

            tree = new RandomizedBinarySearchTree<int>(Comparer<int>.Default);
            Assert.That(tree.Count, Is.EqualTo(0));

            tree = new RandomizedBinarySearchTree<int>((Comparer<int>.Default.Compare));
            Assert.That(tree.Count, Is.EqualTo(0));
        }

        [Test]
        public void InsertTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(0);
            Assert.That(tree.Count, Is.EqualTo(1));
            tree.Insert(1);
            Assert.That(tree.Count, Is.EqualTo(2));
            tree.Insert(-1);
            Assert.That(tree.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(1);
            Assert.That(tree.Count, Is.EqualTo(1));

            Assert.That(tree.Remove(1), Is.True);
            Assert.That(tree.Count, Is.EqualTo(0));

            Assert.That(tree.Remove(0), Is.False);
            Assert.That(tree.Remove(1), Is.False);

            tree.Insert(1);
            tree.Insert(1);
            Assert.That(tree.Count, Is.EqualTo(2));

            Assert.That(tree.Remove(1), Is.True);
            Assert.That(tree.Count, Is.EqualTo(1));
        }

        [Test]
        public void ElementAtTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => tree.ElementAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => tree.ElementAt(1));

            tree.Insert(0);
            tree.Insert(1);
            tree.Insert(-1);
            tree.Insert(2);
            tree.Insert(-1);

            Assert.That(tree.ElementAt(0), Is.EqualTo(-1));
            Assert.That(tree.ElementAt(1), Is.EqualTo(-1));
            Assert.That(tree.ElementAt(2), Is.EqualTo(0));
            Assert.That(tree.ElementAt(3), Is.EqualTo(1));
            Assert.That(tree.ElementAt(4), Is.EqualTo(2));

            Assert.That(tree.Remove(0), Is.True);
            Assert.That(tree.ElementAt(0), Is.EqualTo(-1));
            Assert.That(tree.ElementAt(1), Is.EqualTo(-1));
            Assert.That(tree.ElementAt(2), Is.EqualTo(1));
            Assert.That(tree.ElementAt(3), Is.EqualTo(2));

            Assert.That(tree.Remove(-1), Is.True);
            Assert.That(tree.ElementAt(0), Is.EqualTo(-1));
            Assert.That(tree.ElementAt(1), Is.EqualTo(1));
            Assert.That(tree.ElementAt(2), Is.EqualTo(2));
        }

        [Test]
        public void ContainsTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(0);
            tree.Insert(1);
            tree.Insert(-1);
            tree.Insert(2);
            tree.Insert(-2);

            Assert.That(tree.Contains(0), Is.True);
            Assert.That(tree.Contains(-3), Is.False);
            Assert.That(tree.Contains(3), Is.False);
        }

        [Test]
        public void LowerBoundTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(0);
            tree.Insert(1);
            tree.Insert(-1);
            tree.Insert(2);
            tree.Insert(-2);

            Assert.That(tree.LowerBound(-3), Is.EqualTo(0));
            Assert.That(tree.LowerBound(-2), Is.EqualTo(0));
            Assert.That(tree.LowerBound(-1), Is.EqualTo(1));
            Assert.That(tree.LowerBound(0), Is.EqualTo(2));
            Assert.That(tree.LowerBound(1), Is.EqualTo(3));
            Assert.That(tree.LowerBound(2), Is.EqualTo(4));
            Assert.That(tree.LowerBound(3), Is.EqualTo(5));
        }

        [Test]
        public void UpperBoundTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(0);
            tree.Insert(1);
            tree.Insert(-1);
            tree.Insert(2);
            tree.Insert(-2);

            Assert.That(tree.UpperBound(-3), Is.EqualTo(0));
            Assert.That(tree.UpperBound(-2), Is.EqualTo(1));
            Assert.That(tree.UpperBound(-1), Is.EqualTo(2));
            Assert.That(tree.UpperBound(0), Is.EqualTo(3));
            Assert.That(tree.UpperBound(1), Is.EqualTo(4));
            Assert.That(tree.UpperBound(2), Is.EqualTo(5));
            Assert.That(tree.UpperBound(3), Is.EqualTo(5));
        }

        [Test]
        public void EnumerateTest()
        {
            var tree = new RandomizedBinarySearchTree<int>();
            tree.Insert(0);
            tree.Insert(1);
            tree.Insert(-1);
            tree.Insert(2);
            tree.Insert(-2);

            var expected = new[] { -2, -1, 0, 1, 2 };
            Assert.That(tree.ToArray(), Is.EqualTo(expected));
        }
    }
}