using System;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class FenwickTreeTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FenwickTree(-1));
            Assert.That(new FenwickTree(0).Length, Is.Zero);
            Assert.That(new FenwickTree(10).Length, Is.EqualTo(10));
        }

        [Test]
        public void EmptyTest()
        {
            var ft = new FenwickTree(0);
            Assert.That(ft.Sum(0), Is.Zero);
            Assert.That(ft.Sum(0, 0), Is.Zero);
        }

        [Test]
        public void NativeTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);

            for (var i = 0; i <= n; i++)
            {
                var expected = 0L;
                for (var j = 0; j < i; j++) expected += j * j;
                Assert.That(ft.Sum(i), Is.EqualTo(expected));
            }

            for (var l = 0; l <= n; l++)
            {
                for (var r = l; r <= n; r++)
                {
                    var expected = 0L;
                    for (var i = l; i < r; i++) expected += i * i;
                    Assert.That(ft.Sum(l, r), Is.EqualTo(expected));
                }
            }
        }

        [Test]
        public void LowerBoundTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);
            var sum = new int[n + 1];
            for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + i * i;
            for (var i = 0; i < n; i++)
            {
                Assert.That(ft.LowerBound(sum[i + 1]), Is.EqualTo(i));
                Assert.That(ft.LowerBound(sum[i + 1] + 1), Is.EqualTo(Math.Min(n, i + 1)));
            }
        }

        [Test]
        public void UpperBoundTest([Range(0, 50)] int n)
        {
            var ft = new FenwickTree(n);
            for (var i = 0; i < n; i++) ft.Add(i, i * i);
            var sum = new int[n + 1];
            for (var i = 0; i < n; i++) sum[i + 1] = sum[i] + i * i;
            for (var i = 0; i < n; i++)
            {
                Assert.That(ft.UpperBound(sum[i + 1]), Is.EqualTo(Math.Min(n, i + 1)));
                Assert.That(ft.UpperBound(sum[i + 1] - 1), Is.EqualTo(i));
            }
        }

        [Test]
        public void ArgumentOutOfRangeInAddTest([Values(-1, 10)] int i)
        {
            var ft = new FenwickTree(10);
            Assert.Throws<ArgumentOutOfRangeException>(() => ft.Add(i, 0));
        }

        [Test]
        public void ArgumentOutOfRangeInSumTest1([Values(-1, 11)] int i)
        {
            var ft = new FenwickTree(10);
            Assert.Throws<ArgumentOutOfRangeException>(() => ft.Sum(i));
        }

        [TestCase(-1, 0)]
        [TestCase(0, 11)]
        [TestCase(1, 0)]
        public void ArgumentOutOfRangeInSumTest2(int l, int r)
        {
            var ft = new FenwickTree(10);
            Assert.Throws<ArgumentOutOfRangeException>(() => ft.Sum(l, r));
        }


        [Test]
        public void BoundTest()
        {
            var ft = new FenwickTree(10);
            ft.Add(3, long.MaxValue);
            ft.Add(5, long.MinValue);
            Assert.That(ft.Sum(0, 10), Is.EqualTo(-1));
            Assert.That(ft.Sum(0, 10), Is.EqualTo(-1));
            Assert.That(ft.Sum(3, 4), Is.EqualTo(long.MaxValue));
            Assert.That(ft.Sum(4, 10), Is.EqualTo(long.MinValue));
        }
    }
}