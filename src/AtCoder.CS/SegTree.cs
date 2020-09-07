using System;
using System.Linq;

namespace AtCoder.CS
{
    public class SegTree<T>
    {
        private readonly int _n;
        private readonly int _size;
        private readonly int _log;
        private readonly T[] _data;
        private readonly Func<T, T, T> _operation;
        private readonly T _identity;

        public SegTree(int n, Func<T, T, T> operation, T identity)
        {
            _n = n;
            _operation = operation;
            _identity = identity;
            _log = CeilPow2(_n);
            _size = 1 << _log;
            _data = Enumerable.Repeat(identity, _size * 2).ToArray();
            for (var i = 0; i < _n; i++) _data[_size + i] = identity;
            for (var i = _size - 1; i >= 1; i--) Update(i);
        }

        public void Set(int p, T x)
        {
            if (p < 0 || _n <= p) throw new ArgumentException(nameof(p));
            p += _size;
            _data[p] = x;
            for (var i = 1; i <= _log; i++) Update(p >> i);
        }

        public T Get(int p)
        {
            if (p < 0 || _n <= p) throw new ArgumentException(nameof(p));
            return _data[p + _size];
        }

        public T Prod(int l, int r)
        {
            if (l < 0 || _n <= l) throw new ArgumentException(nameof(l));
            if (r < 0 || _n <= r) throw new ArgumentException(nameof(r));
            if (r < l) throw new ArgumentException();
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

        public T AllProd() => _data[1];

        public int MaxRight(int l, Func<T, bool> func)
        {
            if (l < 0 || _n <= l) throw new ArgumentException(nameof(l));
            if (!func(_identity)) throw new ArgumentException(nameof(func));
            if (l == _n) return _n;
            l += _size;
            var sm = _identity;
            do
            {
                while (l % 2 == 0) l >>= 1;
                if (!func(_operation(sm, _data[l])))
                {
                    while (l < _size)
                    {
                        l *= 2;
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
            if (r < 0 || _n <= r) throw new ArgumentException(nameof(r));
            if (!func(_identity)) throw new ArgumentException(nameof(func));
            if (r == 0) return 0;
            r += _size;
            var sm = _identity;
            do
            {
                r--;
                while (r > 1 && r % 2 == 0) r >>= 1;
                if (!func(_operation(_data[r], sm)))
                {
                    while (r < _size)
                    {
                        r = r * 2 + 1;
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

        private void Update(int k) => _data[k] = _operation(_data[k * 2], _data[k * 2 + 1]);

        private static int CeilPow2(int n)
        {
            var x = 0;
            while (1 << x < n) x++;
            return x;
        }
    }
}