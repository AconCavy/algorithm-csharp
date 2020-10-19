using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class LazySegmentTreeTests
    {
        [Test]
        public void InitializeTest([Values(0, 10)] int n)
        {
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(n, SimpleOperationDelegate, SimpleMonoidId,
                SimpleMappingDelegate, SimpleCompositionDelegate, SimpleMapId));
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(Enumerable.Range(1, n),
                SimpleOperationDelegate, SimpleMonoidId, SimpleMappingDelegate, SimpleCompositionDelegate,
                SimpleMapId));
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(n, SimpleOperation, SimpleMonoidId,
                SimpleMapping, SimpleComposition, SimpleMapId));
            Assert.DoesNotThrow(() => _ = new LazySegmentTree<int, int>(Enumerable.Range(1, n), SimpleOperation,
                SimpleMonoidId, SimpleMapping, SimpleComposition, SimpleMapId));
        }

        [Test]
        public void ZeroTest([Values(0, 10)] int n)
        {
            var lst = new LazySegmentTree<int, int>(n, SimpleOperation, SimpleMonoidId, SimpleMapping,
                SimpleComposition, SimpleMapId);
            Assert.That(lst.QueryToAll(), Is.EqualTo(-(int) 1e9));
        }

        [Test]
        public void InvalidArgumentsTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _ = new LazySegmentTree<int, int>(-1, SimpleOperation, SimpleMonoidId, SimpleMapping, SimpleComposition,
                    SimpleMapId));
            var lst = new LazySegmentTree<int, int>(10, SimpleOperation, SimpleMonoidId, SimpleMapping,
                SimpleComposition, SimpleMapId);
            Assert.Throws<IndexOutOfRangeException>(() => lst.Set(-1, 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Set(10, 1));

            Assert.Throws<IndexOutOfRangeException>(() => lst.Get(-1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Get(10));

            Assert.Throws<IndexOutOfRangeException>(() => lst.Query(-1, -1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Query(3, 2));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Query(0, 11));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Query(-1, 11));

            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(-1, map: 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(11, map: 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(-1, -1, 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(3, 2, 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(0, 11, 1));
            Assert.Throws<IndexOutOfRangeException>(() => lst.Apply(-1, 11, 1));

            Assert.Throws<IndexOutOfRangeException>(() => lst.MaxRight(-1, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => lst.MaxRight(11, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => lst.MinLeft(-1, s => true));
            Assert.Throws<IndexOutOfRangeException>(() => lst.MinLeft(11, s => true));
            Assert.Throws<ArgumentException>(() => lst.MaxRight(0, s => false));
            Assert.Throws<ArgumentException>(() => lst.MinLeft(10, s => false));
        }

        [Test]
        public void SimpleQueryNaiveTest([Range(0, 50)] int n)
        {
            var lst = new LazySegmentTree<int, int>(n, SimpleOperation, SimpleMonoidId, SimpleMapping,
                SimpleComposition, SimpleMapId);
            var p = new int[n];
            for (var i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                lst.Set(i, p[i]);
            }

            for (var l = 0; l <= n; l++)
            {
                for (var r = l; r <= n; r++)
                {
                    var e = -(int) 1e9;
                    for (var i = l; i < r; i++) e = System.Math.Max(e, p[i]);
                    Assert.That(lst.Query(l, r), Is.EqualTo(e));
                }
            }
        }

        [Test]
        public void QueryNaiveStructTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<MonoidStruct, MapStruct>(n, Operation, MonoidStructId, Mapping,
                    Composition, MapStructId);
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new MonoidStruct(i, i + 1, -1));
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
                            lst.Apply(l, r, new MapStruct(now));
                            timeManager.Action(l, r, now);
                            break;
                        case 3:
                            now++;
                            lst.Apply(l, new MapStruct(now));
                            timeManager.Action(l, l + 1, now);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        [Test]
        public void QueryNaiveClassTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<MonoidClass, MapClass>(n, Operation, MonoidClassId, Mapping,
                    Composition, MapClassId);
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new MonoidClass(i, i + 1, -1));
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
                            lst.Apply(l, r, new MapClass(now));
                            timeManager.Action(l, r, now);
                            break;
                        case 3:
                            now++;
                            lst.Apply(l, new MapClass(now));
                            timeManager.Action(l, l + 1, now);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        [Test]
        public void SimpleUsageTest()
        {
            var lst = new LazySegmentTree<int, int>(new int[10], SimpleOperation,
                SimpleMonoidId, SimpleMapping, SimpleComposition, SimpleMapId);
            Assert.That(lst.QueryToAll(), Is.Zero);
            lst.Apply(0, 3, 5);
            Assert.That(lst.QueryToAll(), Is.EqualTo(5));
            lst.Apply(2, -10);
            Assert.That(lst.Query(2, 3), Is.EqualTo(-5));
            Assert.That(lst.Query(2, 4), Is.Zero);
        }

        [Test]
        public void MaxRightTest([Range(1, 30)] int n)
        {
            for (var ph = 0; ph < 10; ph++)
            {
                var lst = new LazySegmentTree<MonoidStruct, MapStruct>(n, Operation, MonoidStructId, Mapping,
                    Composition, MapStructId);
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new MonoidStruct(i, i + 1, -1));
                var now = 0;
                for (var q = 0; q < 1000; q++)
                {
                    var ty = Utilities.RandomInteger(0, 2);
                    var (l, r) = Utilities.RandomPair(0, n);
                    if (ty == 0)
                    {
                        bool F(MonoidStruct s)
                        {
                            if (s.L == -1) return true;
                            return s.R <= r;
                        }

                        Assert.That(lst.MaxRight(l, F), Is.EqualTo(r));
                    }
                    else
                    {
                        now++;
                        lst.Apply(l, r, new MapStruct(now));
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
                var lst = new LazySegmentTree<MonoidStruct, MapStruct>(n, Operation, MonoidStructId, Mapping,
                    Composition, MapStructId);
                var timeManager = new TimeManager(n);
                for (var i = 0; i < n; i++) lst.Set(i, new MonoidStruct(i, i + 1, -1));
                var now = 0;
                for (var q = 0; q < 1000; q++)
                {
                    var ty = Utilities.RandomInteger(0, 2);
                    var (l, r) = Utilities.RandomPair(0, n);
                    if (ty == 0)
                    {
                        bool F(MonoidStruct s)
                        {
                            if (s.L == -1) return true;
                            return l <= s.L;
                        }

                        Assert.That(lst.MinLeft(r, F), Is.EqualTo(l));
                    }
                    else
                    {
                        now++;
                        lst.Apply(l, r, new MapStruct(now));
                        timeManager.Action(l, r, now);
                    }
                }
            }
        }

        [Test]
        public void EdgeTest()
        {
            var lst = new LazySegmentTree<MonoidStruct, MapStruct>(10, Operation, MonoidStructId, Mapping, Composition,
                MapStructId);
            for (var i = 0; i < 10; i++) lst.Set(i, new MonoidStruct(i, i + 1, -1));
            Assert.That(lst.MaxRight(10, x => true), Is.EqualTo(10));
            Assert.That(lst.MinLeft(0, x => true), Is.Zero);
        }

        private const int SimpleMonoidId = -(int) 1e9;
        private const int SimpleMapId = 0;
        private static readonly LazySegmentTree<int, int>.Operation SimpleOperationDelegate = SimpleOperation;
        private static readonly LazySegmentTree<int, int>.Mapping SimpleMappingDelegate = SimpleMapping;
        private static readonly LazySegmentTree<int, int>.Composition SimpleCompositionDelegate = SimpleComposition;
        private static int SimpleOperation(int a, int b) => System.Math.Max(a, b);
        private static int SimpleMapping(int a, int b) => a + b;
        private static int SimpleComposition(int a, int b) => a + b;

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
                for (var i = l; i < r; i++) ret = System.Math.Max(ret, _times[i]);
                return ret;
            }
        }

        private readonly struct MonoidStruct
        {
            public readonly int L;
            public readonly int R;
            public readonly int Time;

            public MonoidStruct(int l, int r, int time)
            {
                L = l;
                R = r;
                Time = time;
            }
        }

        private readonly struct MapStruct
        {
            public readonly int NewTime;
            public MapStruct(int newTime) => NewTime = newTime;
        }

        private static MonoidStruct MonoidStructId => new MonoidStruct(-1, -1, -1);
        private static MapStruct MapStructId => new MapStruct(-1);

        private static MonoidStruct Operation(MonoidStruct l, MonoidStruct r)
        {
            if (l.L == -1) return r;
            if (r.L == -1) return l;
            if (l.R != r.L) throw new ArgumentException();
            return new MonoidStruct(l.L, r.R, System.Math.Max(l.Time, r.Time));
        }

        private static MonoidStruct Mapping(MapStruct l, MonoidStruct r)
        {
            if (l.NewTime == -1) return r;
            if (r.Time >= l.NewTime) throw new ArgumentException();
            return new MonoidStruct(r.L, r.R, l.NewTime);
        }

        private static MapStruct Composition(MapStruct l, MapStruct r)
        {
            if (l.NewTime == -1) return r;
            if (r.NewTime == -1) return l;
            if (l.NewTime <= r.NewTime) throw new ArgumentException();
            return l;
        }

        private class MonoidClass
        {
            public readonly int L;
            public readonly int R;
            public readonly int Time;

            public MonoidClass(int l, int r, int time)
            {
                L = l;
                R = r;
                Time = time;
            }
        }

        private class MapClass
        {
            public readonly int NewTime;
            public MapClass(int newTime) => NewTime = newTime;
        }

        private static MonoidClass MonoidClassId => new MonoidClass(-1, -1, -1);
        private static MapClass MapClassId => new MapClass(-1);

        private static MonoidClass Operation(MonoidClass l, MonoidClass r)
        {
            if (l.L == -1) return r;
            if (r.L == -1) return l;
            if (l.R != r.L) throw new ArgumentException();
            return new MonoidClass(l.L, r.R, System.Math.Max(l.Time, r.Time));
        }

        private static MonoidClass Mapping(MapClass l, MonoidClass r)
        {
            if (l.NewTime == -1) return r;
            if (r.Time >= l.NewTime) throw new ArgumentException();
            return new MonoidClass(r.L, r.R, l.NewTime);
        }

        private static MapClass Composition(MapClass l, MapClass r)
        {
            if (l.NewTime == -1) return r;
            if (r.NewTime == -1) return l;
            if (l.NewTime <= r.NewTime) throw new ArgumentException();
            return l;
        }
    }
}