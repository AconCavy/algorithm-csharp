using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class SegmentTree<T>
    {
        private readonly int _n;
        private readonly int _size;
        private readonly int _log;
        private readonly T[] _data;
        private readonly Func<T, T, T> _operation;
        private readonly T _identity;

        public SegmentTree(int n, Func<T, T, T> operation, T identity)
            : this(Enumerable.Repeat(identity, n), operation, identity)
        {
        }

        public SegmentTree(IEnumerable<T> data, Func<T, T, T> operation, T identity)
        {
            var d = data.ToArray();
            _n = d.Length;
            _operation = operation;
            _identity = identity;
            while (1 << _log < _n) _log++;
            _size = 1 << _log;
            _data = Enumerable.Repeat(identity, _size << 1).ToArray();
            d.CopyTo(_data, _size);
            for (var i = _size - 1; i >= 1; i--) Update(i);
        }

        public void Set(int p, T x)
        {
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            p += _size;
            _data[p] = x;
            for (var i = 1; i <= _log; i++) Update(p >> i);
        }

        public T Get(int p)
        {
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            return _data[p + _size];
        }

        public T Query(int l, int r)
        {
            if (l < 0 || r < l || _n < r) throw new IndexOutOfRangeException();
            var (sml, smr) = (_identity, _identity);
            l += _size;
            r += _size;
            while (l < r)
            {
                if ((l & 1) == 1) sml = _operation(sml, _data[l++]);
                if ((r & 1) == 1) smr = _operation(_data[--r], smr);
                l >>= 1;
                r >>= 1;
            }

            return _operation(sml, smr);
        }

        public T QueryToAll() => _data[1];

        public int MaxRight(int l, Func<T, bool> func)
        {
            if (l < 0 || _n <= l) throw new IndexOutOfRangeException(nameof(l));
            if (!func(_identity)) throw new ArgumentException(nameof(func));
            if (l == _n) return _n;
            l += _size;
            var sm = _identity;
            do
            {
                while ((l & 1) == 0) l >>= 1;
                if (!func(_operation(sm, _data[l])))
                {
                    while (l < _size)
                    {
                        l <<= 1;
                        var tmp = _operation(sm, _data[l]);
                        if (!func(tmp)) continue;
                        sm = tmp;
                        l++;
                    }

                    return l - _size;
                }

                sm = _operation(sm, _data[l]);
                l++;
            } while ((l & -l) != l);

            return _n;
        }

        public int MinLeft(int r, Func<T, bool> func)
        {
            if (r < 0 || _n <= r) throw new IndexOutOfRangeException(nameof(r));
            if (!func(_identity)) throw new ArgumentException(nameof(func));
            if (r == 0) return 0;
            r += _size;
            var sm = _identity;
            do
            {
                r--;
                while (r > 1 && (r & 1) == 0) r >>= 1;
                if (!func(_operation(_data[r], sm)))
                {
                    while (r < _size)
                    {
                        r = (r << 1) + 1;
                        var tmp = _operation(_data[r], sm);
                        if (!func(tmp)) continue;
                        sm = tmp;
                        r--;
                    }

                    return r + 1 - _size;
                }

                sm = _operation(_data[r], sm);
                r++;
            } while ((r & -r) != r);

            return 0;
        }

        private void Update(int k) => _data[k] = _operation(_data[k << 1], _data[(k << 1) + 1]);
    }
}