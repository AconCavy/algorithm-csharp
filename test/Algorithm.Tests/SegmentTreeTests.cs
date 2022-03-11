using System;
using NUnit.Framework;

namespace Algorithm.Tests
{
    public class SegmentTreeTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new SegmentTree<Monoid>(0, new Oracle()));
            Assert.DoesNotThrow(() => _ = new SegmentTree<Monoid>(10, new Oracle()));
            Assert.DoesNotThrow(() => _ = new SegmentTree<Monoid>(Array.Empty<Monoid>(), new Oracle()));
            Assert.DoesNotThrow(() =>
                _ = new SegmentTree<Monoid>(new[] { new Monoid("a"), new Monoid("b"), new Monoid("c") }, new Oracle()));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new SegmentTree<Monoid>(-1, new Oracle()));
        }

        [Test]
        public void EmptyTest()
        {
            var oracle = new Oracle();
            var st = new SegmentTree<Monoid>(0, new Oracle());
            Assert.That(st.Length, Is.Zero);
            Assert.That(st.QueryToAll(), Is.EqualTo(oracle.MonoidIdentity));

            st = new SegmentTree<Monoid>(new Monoid[] { }, new Oracle());
            Assert.That(st.Length, Is.Zero);
            Assert.That(st.QueryToAll(), Is.EqualTo(oracle.MonoidIdentity));
        }

        [Test]
        public void OneTest()
        {
            var oracle = new Oracle();
            var identity = oracle.MonoidIdentity;
            var st = new SegmentTree<Monoid>(1, oracle);
            Assert.That(st.Length, Is.EqualTo(1));
            Assert.That(st.QueryToAll(), Is.EqualTo(identity));
            Assert.That(st.Get(0), Is.EqualTo(identity));
            Assert.That(st.Query(0, 1), Is.EqualTo(identity));

            var expected = new Monoid("dummy");
            st.Set(0, expected);
            Assert.That(st.Get(0), Is.EqualTo(expected));
            Assert.That(st.Query(0, 0), Is.EqualTo(identity));
            Assert.That(st.Query(0, 1), Is.EqualTo(expected));
            Assert.That(st.Query(1, 1), Is.EqualTo(identity));
        }

        [Test]
        public void CompareToNaiveTest([Range(0, 30)] int n)
        {
            var y = "";

            bool SimpleQuery(Monoid x) => x.Value.Length <= y.Length;

            var oracle = new Oracle();
            var st = new SegmentTree<Monoid>(n, oracle);
            Assert.That(st.Length, Is.EqualTo(n));
            var stn = new SegmentTreeNaive<Monoid>(n, oracle);
            for (var i = 0; i < n; i++)
            {
                var s = new Monoid($"a{i}");
                st.Set(i, s);
                stn.Set(i, s);
            }

            for (var i = 0; i < n; i++)
                Assert.That(st.Get(i), Is.EqualTo(stn.Get(i)));

            for (var l = 0; l <= n; l++)
            for (var r = l; r <= n; r++)
                Assert.That(st.Query(l, r), Is.EqualTo(stn.Query(l, r)));

            for (var l = 0; l <= n; l++)
            for (var r = l; r <= n; r++)
            {
                y = st.Query(l, r).Value;
                Assert.That(st.MaxRight(l, SimpleQuery), Is.EqualTo(stn.MaxRight(l, SimpleQuery)));
                Assert.That(st.MaxRight(l, SimpleQuery), Is.EqualTo(stn.MaxRight(l, x => x.Value.Length <= y.Length)));
            }

            for (var r = 0; r <= n; r++)
            for (var l = 0; l <= r; l++)
            {
                y = st.Query(l, r).Value;
                Assert.That(st.MinLeft(r, SimpleQuery), Is.EqualTo(stn.MinLeft(r, SimpleQuery)));
                Assert.That(st.MinLeft(r, SimpleQuery), Is.EqualTo(stn.MinLeft(r, x => x.Value.Length <= y.Length)));
            }
        }

        [Test]
        public void ArgumentOutOfRangeInSetGetTest([Values(-1, 10)] int i)
        {
            var st = new SegmentTree<Monoid>(10, new Oracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => st.Set(i, new Monoid("*")));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = st.Get(i));
        }

        [TestCase(-1, 0)]
        [TestCase(0, 11)]
        [TestCase(1, 0)]
        [TestCase(-1, 11)]
        public void ArgumentOutOfRangeInQueryTest(int l, int r)
        {
            var st = new SegmentTree<Monoid>(10, new Oracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = st.Query(l, r));
        }

        [Test]
        public void ArgumentOutOfRangeInMaxRightMinLeftTest([Values(-1, 11)] int i)
        {
            var st = new SegmentTree<Monoid>(10, new Oracle());
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = st.MaxRight(i, monoid => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = st.MinLeft(i, monoid => true));
        }

        [Test]
        public void InvalidArgumentInMaxRightMinLeftTest()
        {
            var st = new SegmentTree<Monoid>(10, new Oracle());
            Assert.Throws<ArgumentException>(() => _ = st.MaxRight(10, monoid => false));
            Assert.Throws<ArgumentException>(() => _ = st.MinLeft(0, monoid => false));
        }

        [Test]
        public void NullPredicateInMaxRightMinLeftTest()
        {
            var st = new SegmentTree<Monoid>(10, new Oracle());
            Assert.Throws<ArgumentNullException>(() => _ = st.MaxRight(10, null));
            Assert.Throws<ArgumentNullException>(() => _ = st.MinLeft(0, null));
        }

        private class SegmentTreeNaive<TMonoid> where TMonoid : struct
        {
            private readonly TMonoid[] _data;
            private readonly TMonoid _id;
            private readonly int _n;
            private readonly IOracle<TMonoid> _oracle;

            public SegmentTreeNaive(int n, IOracle<TMonoid> oracle)
            {
                _n = n;
                _data = new TMonoid[n];
                _oracle = oracle;
                _id = _oracle.MonoidIdentity;
            }

            public void Set(int p, TMonoid x) => _data[p] = x;

            public TMonoid Get(int p) => _data[p];

            public TMonoid Query(int l, int r)
            {
                var sum = _id;
                for (var i = l; i < r; i++) sum = _oracle.Operate(sum, _data[i]);
                return sum;
            }

            public int MaxRight(int l, Func<TMonoid, bool> func)
            {
                var sum = _id;
                for (var i = l; i < _n; i++)
                {
                    sum = _oracle.Operate(sum, _data[i]);
                    if (!func(sum)) return i;
                }

                return _n;
            }

            public int MinLeft(int r, Func<TMonoid, bool> func)
            {
                var sum = _id;
                for (var i = r - 1; i >= 0; i--)
                {
                    sum = _oracle.Operate(_data[i], sum);
                    if (!func(sum)) return i + 1;
                }

                return 0;
            }
        }

        private readonly struct Monoid : IEquatable<Monoid>
        {
            public readonly string Value;

            public Monoid(string value) => Value = value;

            public bool Equals(Monoid other) => Value == other.Value;

            public override bool Equals(object obj) => obj is Monoid other && Equals(other);

            public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;
        }

        private class Oracle : IOracle<Monoid>
        {
            public Monoid MonoidIdentity { get; } = new Monoid("$");

            public Monoid Operate(Monoid a, Monoid b)
            {
                if (a.Value == "$") return b;
                if (b.Value == "$") return a;
                return new Monoid(a.Value + b.Value);
            }
        }
    }
}