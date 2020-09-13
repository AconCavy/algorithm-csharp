using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class LazySegTree<T, U>
    {
        private readonly int _n;
        private readonly int _size;
        private readonly int _log;
        private readonly T[] _data;
        private readonly U[] _lazy;
        private readonly Func<T, T, T> _operation;
        private readonly Func<U, T, T> _mapping;
        private readonly Func<U, U, U> _composition;
        private readonly T _identityT;
        private readonly U _identityU;

        public LazySegTree(int n, Func<T, T, T> operation, T identityT, Func<U, T, T> mapping,
            Func<U, U, U> composition, U identityU) :
            this(new T[n], operation, identityT, mapping, composition, identityU)
        {
        }

        public LazySegTree(IEnumerable<T> data, Func<T, T, T> operation, T identityT, Func<U, T, T> mapping,
            Func<U, U, U> composition, U identityU)
        {
            var d = data.ToArray();
            _n = d.Length;
            _operation = operation;
            _identityT = identityT;
            _mapping = mapping;
            _composition = composition;
            _identityU = identityU;
            while (1 << _log < _n) _log++;
            _size = 1 << _log;
            _data = Enumerable.Repeat(identityT, _size * 2).ToArray();
            _lazy = Enumerable.Repeat(identityU, _size).ToArray();
            for (var i = 0; i < _n; i++) _data[_size + i] = d[i];
            for (var i = _size - 1; i >= 1; i--) Update(i);
        }

        public void Set(int p, T x)
        {
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            p += _size;
            for (var i = _log; i >= 1; i--) Push(p >> i);
            _data[p] = x;
            for (var i = 1; i <= _log; i++) Update(p >> i);
        }

        public T Get(int p)
        {
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            p += _size;
            for (var i = _log; i >= 1; i--) Push(p >> i);
            return _data[p];
        }

        public T Prod(int l, int r)
        {
            if (l < 0 || r < l || _n < r) throw new IndexOutOfRangeException();
            if (l == r) return _identityT;
            l += _size;
            r += _size;
            for (var i = _log; i >= 1; i--)
            {
                if ((l >> i) << i != l) Push(l >> i);
                if ((r >> i) << i != r) Push(r >> i);
            }

            var (sml, smr) = (_identityT, _identityT);
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

        public void Apply(int p, U u)
        {
            if (p < 0 || _n <= p) throw new IndexOutOfRangeException(nameof(p));
            p += _size;
            for (var i = _log; i >= 1; i--) Push(p >> i);
            _data[p] = _mapping(u, _data[p]);
            for (var i = 1; i <= _log; i++) Update(p >> i);
        }

        public void Apply(int l, int r, U u)
        {
            if (l < 0 || r < l || _n < r) throw new IndexOutOfRangeException();
            if (l == r) return;
            l += _size;
            r += _size;
            for (var i = _log; i >= 1; i--)
            {
                if ((l >> i) << i != l) Push(l >> i);
                if ((r >> i) << i != r) Push((r - 1) >> i);
            }

            var (l2, r2) = (l, r);
            while (l2 < r2)
            {
                if ((l2 & 1) == 1) AllApply(l2++, u);
                if ((r2 & 1) == 1) AllApply(--r2, u);
                l2 >>= 1;
                r2 >>= 1;
            }

            for (var i = 1; i <= _log; i++)
            {
                if ((l >> i) << i != l) Update(l >> i);
                if ((r >> i) << i != r) Update((r - 1) >> i);
            }
        }

        public int MaxRight(int l, Func<T, bool> func)
        {
            if (l < 0 || _n <= l) throw new IndexOutOfRangeException(nameof(l));
            if (!func(_identityT)) throw new ArgumentException(nameof(func));
            if (l == _n) return _n;
            l += _size;
            var sm = _identityT;
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
            if (r < 0 || _n <= r) throw new IndexOutOfRangeException(nameof(r));
            if (!func(_identityT)) throw new ArgumentException(nameof(func));
            if (r == 0) return 0;
            r += _size;
            var sm = _identityT;
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

        private void AllApply(int k, U u)
        {
            _data[k] = _mapping(u, _data[k]);
            if (k < _size) _lazy[k] = _composition(u, _lazy[k]);
        }

        private void Push(int k)
        {
            AllApply(k * 2, _lazy[k]);
            AllApply(k * 2 + 1, _lazy[k]);
            _lazy[k] = _identityU;
        }
    }
}