using System;
using System.Linq;
using NUnit.Framework;

namespace AlgorithmSharp.Tests
{
    public class LazySegmentTreeTests
    {
        [Test]
        public void InitializeTest([Values(0, 10)] int n)
        {
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(n, new SimpleOracle()));
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(Enumerable.Range(1, n), new SimpleOracle()));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new LazySegmentTree<int, int>(-1, new SimpleOracle()));
        }

        [Test]
        public void ZeroTest([Values(0, 10)] int n)
        {
            var lst = new LazySegmentTree<int, int>(n, new SimpleOracle());
            Assert.That(lst.QueryToAll(), Is.EqualTo(-(int)1e9));
        }

        [Test]
        public void SimpleUsageTest()
        {
            var lst = new LazySegmentTree<int, int>(new int[10], new SimpleOracle());
            Assert.That(lst.QueryToAll(), Is.Zero);
            lst.Apply(0, 3, 5);
            Assert.That(lst.QueryToAll(), Is.EqualTo(5));
            lst.Apply(2, -10);
            Assert.That(lst.Query(2, 3), Is.EqualTo(-5));
            Assert.That(lst.Query(2, 4), Is.Zero);
        }

        [Test]
        public void SimpleQueryNaiveTest([Range(0, 50)] int n)
        {
            var lst = new LazySegmentTree<int, int>(n, new SimpleOracle());
            var p = new int[n];
            for (var i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                lst.Set(i, p[i]);
            }

            for (var l = 0; l <= n; l++)
            for (var r = l; r <= n; r++)
            {
                var e = -(int)1e9;
                for (var i = l; i < r; i++) e = Math.Max(e, p[i]);
                Assert.That(lst.Query(l, r), Is.EqualTo(e));
            }
        }

        [Test]
        public void QueryNaiveTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<Monoid, Map>(n, new Oracle());
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new Monoid(i, i + 1, -1));
                var now = 0;
                for (var q = 0; q < 3000; q++)
                {
                    var ty = Utilities.RandomInteger(0, 3);
                    var (l, r) = Utilities.RandomPair(0, n);
                    switch (ty)
                    {
                        case 0:
                        {
                            var result = lst.Query(l, r);
                            Assert.That(result.L, Is.EqualTo(l));
                            Assert.That(result.R, Is.EqualTo(r));
                            Assert.That(result.Time, Is.EqualTo(timeManager.Query(l, r)));
                            break;
                        }
                        case 1:
                        {
                            var result = lst.Get(l);
                            Assert.That(result.L, Is.EqualTo(l));
                            Assert.That(result.L + 1, Is.EqualTo(l + 1));
                            Assert.That(result.Time, Is.EqualTo(timeManager.Query(l, l + 1)));
                            break;
                        }
                        case 2:
                            now++;
                            lst.Apply(l, r, new Map(now));
                            timeManager.Action(l, r, now);
                            break;
                        case 3:
                            now++;
                            lst.Apply(l, new Map(now));
                            timeManager.Action(l, l + 1, now);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        [Test]
        public void MaxRightTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<Monoid, Map>(n, new Oracle());
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new Monoid(i, i + 1, -1));
                var now = 0;
                for (var q = 0; q < 1000; q++)
                {
                    var ty = Utilities.RandomInteger(0, 2);
                    var (l, r) = Utilities.RandomPair(0, n);
                    if (ty == 0)
                    {
                        bool F(Monoid s)
                        {
                            if (s.L == -1) return true;
                            return s.R <= r;
                        }

                        Assert.That(lst.MaxRight(l, F), Is.EqualTo(r));
                    }
                    else
                    {
                        now++;
                        lst.Apply(l, r, new Map(now));
                        timeManager.Action(l, r, now);
                    }
                }
            }
        }

        [Test]
        public void MaxLeftTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<Monoid, Map>(n, new Oracle());
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new Monoid(i, i + 1, -1));
                var now = 0;
                for (var q = 0; q < 1000; q++)
                {
                    var ty = Utilities.RandomInteger(0, 2);
                    var (l, r) = Utilities.RandomPair(0, n);
                    if (ty == 0)
                    {
                        bool F(Monoid s)
                        {
                            if (s.L == -1) return true;
                            return l <= s.L;
                        }

                        Assert.That(lst.MinLeft(r, F), Is.EqualTo(l));
                    }
                    else
                    {
                        now++;
                        lst.Apply(l, r, new Map(now));
                        timeManager.Action(l, r, now);
                    }
                }
            }
        }

        [Test]
        public void EdgeTest()
        {
            var lst = new LazySegmentTree<Monoid, Map>(10, new Oracle());
            for (var i = 0; i < 10; i++) lst.Set(i, new Monoid(i, i + 1, -1));
            Assert.That(lst.MaxRight(10, x => true), Is.EqualTo(10));
            Assert.That(lst.MinLeft(0, x => true), Is.Zero);
        }

        [Test]
        public void ArgumentOutOfRangeInSetGetTest([Values(-1, 10)] int i)
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => lst.Set(i, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => lst.Get(i));
        }

        [TestCase(-1, 0)]
        [TestCase(0, 11)]
        [TestCase(1, 0)]
        [TestCase(-1, 11)]
        public void ArgumentOutOfRangeInQueryTest(int l, int r)
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => lst.Query(l, r));
        }

        [TestCase(-1, 0)]
        [TestCase(0, 11)]
        [TestCase(1, 0)]
        [TestCase(-1, 11)]
        public void ArgumentOutOfRangeInApplyTest(int l, int r)
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            if (l < 0) Assert.Throws<ArgumentOutOfRangeException>(() => lst.Apply(l, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => lst.Apply(l, r, 1));
        }

        [Test]
        public void ArgumentOutOfRangeInMaxRightMinLeftTest([Values(-1, 11)] int i)
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lst.MaxRight(i, monoid => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = lst.MinLeft(i, monoid => true));
        }

        [Test]
        public void InvalidArgumentInMaxRightMinLeftTest()
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            Assert.Throws<ArgumentException>(() => _ = lst.MaxRight(10, monoid => false));
            Assert.Throws<ArgumentException>(() => _ = lst.MinLeft(0, monoid => false));
        }

        [Test]
        public void NullPredicateInMaxRightMinLeftTest()
        {
            var lst = new LazySegmentTree<int, int>(10, new SimpleOracle());
            Assert.Throws<ArgumentNullException>(() => _ = lst.MaxRight(10, null));
            Assert.Throws<ArgumentNullException>(() => _ = lst.MinLeft(0, null));
        }

        private class SimpleOracle : IOracle<int, int>
        {
            public int MonoidIdentity { get; } = -(int)1e9;

            public int Operate(in int a, in int b) => Math.Max(a, b);

            public int MapIdentity { get; } = 0;

            public int Map(in int f, in int x) => f + x;

            public int Compose(in int f, in int g) => f + g;
        }

        private class TimeManager
        {
            private readonly int[] _times;

            public TimeManager(int n) => _times = Enumerable.Repeat(-1, n).ToArray();

            public void Action(int l, int r, int time)
            {
                for (var i = l; i < r; i++) _times[i] = time;
            }

            public int Query(int l, int r)
            {
                var ret = -1;
                for (var i = l; i < r; i++) ret = Math.Max(ret, _times[i]);
                return ret;
            }
        }

        private readonly struct Monoid : IEquatable<Monoid>
        {
            public readonly int L;
            public readonly int R;
            public readonly int Time;

            public Monoid(int l, int r, int time) => (L, R, Time) = (l, r, time);

            public bool Equals(Monoid other) => L == other.L && R == other.R && Time == other.Time;

            public override bool Equals(object obj) => obj is Monoid other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(L, R, Time);
        }

        private readonly struct Map : IEquatable<Map>
        {
            public readonly int NewTime;

            public Map(int newTime) => NewTime = newTime;

            public bool Equals(Map other) => NewTime == other.NewTime;

            public override bool Equals(object obj) => obj is Map other && Equals(other);

            public override int GetHashCode() => NewTime;
        }

        private class Oracle : IOracle<Monoid, Map>
        {
            public Monoid MonoidIdentity { get; } = new Monoid(-1, -1, -1);

            public Monoid Operate(in Monoid a, in Monoid b)
            {
                if (a.L == -1) return b;
                if (b.L == -1) return a;
                if (a.R != b.L) throw new ArgumentException();
                return new Monoid(a.L, b.R, Math.Max(a.Time, b.Time));
            }

            public Map MapIdentity { get; } = new Map(-1);

            public Monoid Map(in Map f, in Monoid x)
            {
                if (f.NewTime == -1) return x;
                if (x.Time >= f.NewTime) throw new ArgumentException();
                return new Monoid(x.L, x.R, f.NewTime);
            }

            public Map Compose(in Map f, in Map g)
            {
                if (f.NewTime == -1) return g;
                if (g.NewTime == -1) return f;
                if (f.NewTime <= g.NewTime) throw new ArgumentException();
                return f;
            }
        }
    }
}