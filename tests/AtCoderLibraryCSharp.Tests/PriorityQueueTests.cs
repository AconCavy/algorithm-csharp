using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class PriorityQueueTests
    {
        [Test]
        public void InitializeTest()
        {
            var queue = new PriorityQueue<int>();
            Assert.That(queue.Count, Is.EqualTo(0));

            queue = new PriorityQueue<int>(new[] {0, 1, 2});
            Assert.That(queue.Count, Is.EqualTo(3));

            queue = new PriorityQueue<int>(new DescendingComparer<int>());
            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void AscendingTest()
        {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(0);
            queue.Enqueue(4);
            queue.Enqueue(3);
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.That(queue.Dequeue(), Is.EqualTo(0));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
            Assert.That(queue.Dequeue(), Is.EqualTo(2));
            Assert.That(queue.Dequeue(), Is.EqualTo(3));
            Assert.That(queue.Dequeue(), Is.EqualTo(4));
        }

        [Test]
        public void DescendingTest()
        {
            var queue = new PriorityQueue<int>(new DescendingComparer<int>());
            queue.Enqueue(0);
            queue.Enqueue(4);
            queue.Enqueue(3);
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.That(queue.Dequeue(), Is.EqualTo(4));
            Assert.That(queue.Dequeue(), Is.EqualTo(3));
            Assert.That(queue.Dequeue(), Is.EqualTo(2));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
            Assert.That(queue.Dequeue(), Is.EqualTo(0));
        }

        [Test]
        public void SameValuesTest()
        {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(0);
            queue.Enqueue(1);
            queue.Enqueue(1);
            queue.Enqueue(1);
            Assert.That(queue.Dequeue(), Is.EqualTo(0));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
            Assert.That(queue.Dequeue(), Is.EqualTo(1));
        }

        [Test]
        public void QueueNothingTest()
        {
            var queue = new PriorityQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void TryTest()
        {
            var queue = new PriorityQueue<int>();
            Assert.That(queue.TryPeek(out var result), Is.False);
            Assert.That(result, Is.EqualTo(default(int)));
            Assert.That(queue.TryDequeue(out result), Is.False);
            Assert.That(result, Is.EqualTo(default(int)));

            queue.Enqueue(1);
            Assert.That(queue.TryPeek(out result), Is.True);
            Assert.That(result, Is.EqualTo(1));
            Assert.That(queue.TryDequeue(out result), Is.True);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ClearTest()
        {
            var queue = new PriorityQueue<int>();
            queue.Clear();
            Assert.That(queue.Count, Is.EqualTo(0));

            queue.Enqueue(0);
            Assert.That(queue.Count, Is.EqualTo(1));
            queue.Clear();
            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void ContainsTest()
        {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(0);
            Assert.That(queue.Contains(0), Is.True);
            Assert.That(queue.Contains(1), Is.False);
        }

        private class DescendingComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y) => Comparer<T>.Default.Compare(y, x);
        }
    }
}