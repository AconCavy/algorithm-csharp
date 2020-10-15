using System;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class SegmentTreeTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(0, OperationDelegate, Id));
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(10, OperationDelegate, Id));

            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(0, Operation, Id));
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(10, Operation, Id));

            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(0, (a, b) =>
            {
                if (a == "$") return b;
                if (b == "$") return a;
                return a + b;
            }, Id));
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(10, (a, b) =>
            {
                if (a == "$") return b;
                if (b == "$") return a;
                return a + b;
            }, Id));

            Func<string, string, string> func = (a, b) =>
            {
                if (a == "$") return b;
                if (b == "$") return a;
                return a + b;
            };
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(0, func.Invoke, Id));
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(10, func.Invoke, Id));

            static string LocalFunc(string a, string b)
            {
                if (a == "$") return b;
                if (b == "$") return a;
                return a + b;
            }

            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(0, LocalFunc, Id));
            Assert.DoesNotThrow(() => _ = new SegmentTree<string>(10, LocalFunc, Id));
        }

        [Test]
        public void EmptyTest()
        {
            var st = new SegmentTree<string>(0, Operation, Id);
            Assert.That(st.QueryToAll(), Is.EqualTo("$"));
        }

        [Test]
        public void InvalidArgumentsTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new SegmentTree<string>(-1, Operation, Id));
            var st = new SegmentTree<string>(10, Operation, Id);
            Assert.Throws<IndexOutOfRangeException>(() => st.Set(-1, "*"));
            Assert.Throws<IndexOutOfRangeException>(() => st.Set(10, "*"));

            Assert.Throws<IndexOutOfRangeException>(() => st.Get(-1));
            Assert.Throws<IndexOutOfRangeException>(() => st.Get(10));

            Assert.Throws<IndexOutOfRangeException>(() => st.Query(-1, -1));
            Assert.Throws<IndexOutOfRangeException>(() => st.Query(3, 2));
            Assert.Throws<IndexOutOfRangeException>(() => st.Query(0, 11));
            Assert.Throws<IndexOutOfRangeException>(() => st.Query(-1, 11));

            Assert.Throws<IndexOutOfRangeException>(() => st.MaxRight(-1, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => st.MaxRight(11, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => st.MinLeft(-1, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => st.MinLeft(11, s => true));
            Assert.Throws<ArgumentException>(() => st.MaxRight(0, s => false));
            Assert.Throws<ArgumentException>(() => st.MinLeft(10, s => false));
        }

        [Test]
        public void OneTest()
        {
            var st = new SegmentTree<string>(1, Operation, Id);
            Assert.That(st.QueryToAll(), Is.EqualTo("$"));
            Assert.That(st.Get(0), Is.EqualTo("$"));
            Assert.That(st.Query(0, 1), Is.EqualTo("$"));
            st.Set(0, "dummy");
            Assert.That(st.Get(0), Is.EqualTo("dummy"));
            Assert.That(st.Query(0, 0), Is.EqualTo("$"));
            Assert.That(st.Query(0, 1), Is.EqualTo("dummy"));
            Assert.That(st.Query(1, 1), Is.EqualTo("$"));
        }

        [Test]
        public void CompareToNaiveTest()
        {
            bool SimpleQuery(string x) => x.Length <= _y.Length;
            for (var n = 0; n < 30; n++)
            {
                var st = new SegmentTree<string>(n, Operation, Id);
                var stn = new SegmentTreeNaive<string>(n, Operation, Id);
                for (var i = 0; i < n; i++)
                {
                    var s = $"a{i}";
                    st.Set(i, s);
                    stn.Set(i, s);
                }

                for (var l = 0; l <= n; l++)
                for (var r = l; r <= n; r++)
                    Assert.That(st.Query(l, r), Is.EqualTo(stn.Query(l, r)));

                for (var l = 0; l <= n; l++)
                for (var r = l; r <= n; r++)
                {
                    _y = st.Query(l, r);
                    Assert.That(st.MaxRight(l, SimpleQuery), Is.EqualTo(stn.MaxRight(l, SimpleQuery)));
                    Assert.That(st.MaxRight(l, SimpleQuery), Is.EqualTo(stn.MaxRight(l, x => x.Length <= _y.Length)));
                }

                for (var r = 0; r <= n; r++)
                for (var l = 0; l <= r; l++)
                {
                    _y = st.Query(l, r);
                    Assert.That(st.MinLeft(r, SimpleQuery), Is.EqualTo(stn.MinLeft(r, SimpleQuery)));
                    Assert.That(st.MinLeft(r, SimpleQuery), Is.EqualTo(stn.MinLeft(r, x => x.Length <= _y.Length)));
                }
            }
        }

        private string _y = "";
        private static readonly SegmentTree<string>.Operation OperationDelegate = Operation;

        private static string Operation(string a, string b)
        {
            if (a == "$") return b;
            if (b == "$") return a;
            return a + b;
        }

        private const string Id = "$";

        private class SegmentTreeNaive<TMonoid>
        {
            private readonly int _n;
            private readonly TMonoid[] _data;
            private readonly Func<TMonoid, TMonoid, TMonoid> _operation;
            private readonly TMonoid _id;

            public SegmentTreeNaive(int n, Func<TMonoid, TMonoid, TMonoid> operation, TMonoid id)
            {
                _n = n;
                _data = new TMonoid[n];
                _operation = operation;
                _id = id;
            }

            public void Set(int p, TMonoid x) => _data[p] = x;
            public TMonoid Get(int p) => _data[p];

            public TMonoid Query(int l, int r)
            {
                var sum = _id;
                for (var i = l; i < r; i++) sum = _operation(sum, _data[i]);
                return sum;
            }

            public int MaxRight(int l, Func<TMonoid, bool> func)
            {
                var sum = _id;
                for (var i = l; i < _n; i++)
                {
                    sum = _operation(sum, _data[i]);
                    if (!func(sum)) return i;
                }

                return _n;
            }

            public int MinLeft(int r, Func<TMonoid, bool> func)
            {
                var sum = _id;
                for (var i = r - 1; i >= 0; i--)
                {
                    sum = _operation(_data[i], sum);
                    if (!func(sum)) return i + 1;
                }

                return 0;
            }
        }
    }
}