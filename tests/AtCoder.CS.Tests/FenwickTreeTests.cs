using System;
using NUnit.Framework;

namespace AtCoder.CS.Tests
{
    public class FenwickTreeTests
    {
        [Test]
        public void EmptyTest()
        {
            var ft = new FenwickTree();
            Assert.That(ft.Sum(0, 0), Is.Zero);
        }

        [Test]
        public void NativeTest()
        {
            const int n = 50;
            for (var k = 0; k <= n; k++)
            {
                var ft = new FenwickTree(k);
                for (var i = 0; i < k; i++) ft.Add(i, i * i);

                for (var l = 0; l <= k; l++)
                {
                    for (var r = l; r <= k; r++)
                    {
                        var sum = 0L;
                        for (var i = l; i < r; i++) sum += i * i;
                        Assert.That(ft.Sum(l, r), Is.EqualTo(sum));
                    }
                }
            }
        }

        [Test]
        public void InvalidTest()
        {
            Assert.Throws<OverflowException>(() => _ = new FenwickTree(-1));
            
            var ft = new FenwickTree(10);
            Assert.Throws<IndexOutOfRangeException>(() => ft.Add(-1, 0));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Add(10, 0));

            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(-1, 3));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(3, 11));
            Assert.Throws<IndexOutOfRangeException>(() => ft.Sum(5, 3));
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