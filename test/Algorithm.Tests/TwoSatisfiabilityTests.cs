using System;
using System.Linq;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class TwoSatisfiabilityTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new TwoSatisfiability());
            Assert.DoesNotThrow(() => _ = new TwoSatisfiability(2));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new TwoSatisfiability(-1));
        }

        [Test]
        public void EmptyTest()
        {
            var ts = new TwoSatisfiability();
            Assert.That(new bool[] { }, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void OneTest()
        {
            var ts = new TwoSatisfiability(1);
            ts.AddClause(0, true, 0, true);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.False);

            ts = new TwoSatisfiability(1);
            ts.AddClause(0, true, 0, true);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] { true }, Is.EqualTo(ts.Answer));

            ts = new TwoSatisfiability(1);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] { false }, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void StressTest()
        {
            for (var ph = 0; ph < 10000; ph++)
            {
                var n = Utilities.RandomInteger(1, 20);
                var m = Utilities.RandomInteger(1, 100);
                var expected = new bool[n].Select(_ => Utilities.RandomBool()).ToArray();
                var ts = new TwoSatisfiability(n);
                var xs = new int[m];
                var ys = new int[m];
                var types = new int[m];
                for (var i = 0; i < m; i++)
                {
                    var x = Utilities.RandomInteger(0, n - 1);
                    var y = Utilities.RandomInteger(0, n - 1);
                    var type = Utilities.RandomInteger(0, 2);
                    xs[i] = x;
                    ys[i] = y;
                    types[i] = type;
                    switch (type)
                    {
                        case 0:
                            ts.AddClause(x, expected[x], y, expected[y]);
                            break;
                        case 1:
                            ts.AddClause(x, !expected[x], y, expected[y]);
                            break;
                        default:
                            ts.AddClause(x, expected[x], y, !expected[y]);
                            break;
                    }
                }

                Assert.That(ts.IsSatisfiable, Is.True);
                var actual = ts.Answer;
                for (var i = 0; i < m; i++)
                {
                    var (x, y, type) = (xs[i], ys[i], types[i]);
                    switch (type)
                    {
                        case 0:
                            Assert.That(actual[x] == expected[x] || actual[y] == expected[y], Is.True);
                            break;
                        case 1:
                            Assert.That(actual[x] != expected[x] || actual[y] == expected[y], Is.True);
                            break;
                        default:
                            Assert.That(actual[x] == expected[x] || actual[y] != expected[y], Is.True);
                            break;
                    }
                }
            }
        }

        [TestCase(-1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 2)]
        [TestCase(-1, 2)]
        [TestCase(2, -1)]
        public void InvalidArgumentsTest(int i, int j)
        {
            var ts = new TwoSatisfiability(2);
            Assert.Throws<ArgumentOutOfRangeException>(() => ts.AddClause(i, true, j, true));
        }
    }
}