using System;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class FenwickTree2DTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new FenwickTree2D<long>(0, 0));
            Assert.DoesNotThrow(() => _ = new FenwickTree2D<long>(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FenwickTree2D<long>(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new FenwickTree2D<long>(0, -1));

            var cum = new FenwickTree2D<long>(0, 0);
            Assert.That(cum.Height, Is.Zero);
            Assert.That(cum.Width, Is.Zero);

            cum = new FenwickTree2D<long>(1, 1);
            Assert.That(cum.Height, Is.EqualTo(1));
            Assert.That(cum.Width, Is.EqualTo(1));
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void ArgumentOutOfRangeTest(int h, int w)
        {
            var cum = new FenwickTree2D<long>(1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Add(h, w, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(h, w));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(h, w, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => cum.Sum(0, 0, h, w));
        }

        [Test]
        public void AddTest()
        {
            var cum = new FenwickTree2D<long>(1, 1);
            const int expected = 1;
            cum.Add(0, 0, expected);
            var actual = cum.Sum(0, 0);
            Assert.That(actual, Is.Zero);

            actual = cum.Sum(1, 1);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SumTest()
        {
            const int height = 3;
            const int width = 3;
            var cum = new FenwickTree2D<long>(height, width);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    cum.Add(i, j, 1);
                }
            }

            for (var i = 0; i <= height; i++)
            {
                for (var j = 0; j <= width; j++)
                {
                    var expected = i * j;
                    var actual = cum.Sum(i, j);
                    Assert.That(actual, Is.EqualTo(expected));
                }
            }
        }

        [Test]
        public void SumRangeTest()
        {
            var random = new Random(0);
            const int height = 50;
            const int width = 50;
            var ft = new FenwickTree2D<long>(height, width);
            var data = new long[height, width];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var value = random.Next(0, 10);
                    ft.Add(i, j, value);
                    data[i, j] = value;
                }
            }

            var sum = new long[height + 1, width + 1];
            for (var i = 1; i <= height; i++)
            {
                for (var j = 1; j <= width; j++)
                {
                    sum[i, j] = sum[i, j - 1] + sum[i - 1, j] - sum[i - 1, j - 1] + data[i - 1, j - 1];
                }
            }

            for (var h1 = 0; h1 <= height; h1++)
            {
                for (var w1 = 0; w1 <= width; w1++)
                {
                    for (var h2 = h1; h2 <= height; h2++)
                    {
                        for (var w2 = w1; w2 <= width; w2++)
                        {
                            var sum1 = ft.Sum(h1, w1, h2, w2);
                            var sum2 = sum[h1, w1] + sum[h2, w2] - sum[h2, w1] - sum[h1, w2];
                            Assert.That(sum1, Is.EqualTo(sum2));
                        }
                    }
                }
            }
        }
    }
}