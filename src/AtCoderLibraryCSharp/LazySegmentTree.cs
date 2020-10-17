using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class LazySegmentTree<TMonoid, TMap>
    {
        public delegate TMonoid Operation(TMonoid a, TMonoid b);

        public delegate TMonoid Mapping(TMap f, TMonoid x);

        public delegate TMap Composition(TMap f, TMap g);

        private readonly int _length;
        private readonly int _size;
        private readonly int _log;
        private readonly TMonoid[] _data;
        private readonly TMap[] _lazy;
        private readonly Operation _operation;
        private readonly Mapping _mapping;
        private readonly Composition _composition;
        private readonly TMonoid _monoidId;
        private readonly TMap _mapId;

        public LazySegmentTree(int length, Operation operation, TMonoid monoidId,
            Mapping mapping, Composition composition, TMap mapId) :
            this(Enumerable.Repeat(monoidId, length), operation, monoidId, mapping, composition, mapId)
        {
        }

        public LazySegmentTree(IEnumerable<TMonoid> data, Operation operation, TMonoid monoidId,
            Mapping mapping, Composition composition, TMap mapId)
        {
            var d = data.ToArray();
            _length = d.Length;
            _operation = operation;
            _monoidId = monoidId;
            _mapping = mapping;
            _composition = composition;
            _mapId = mapId;
            while (1 << _log < _length) _log++;
            _size = 1 << _log;
            _data = new TMonoid[_size << 1];
            for (var i = 0; i < _data.Length; i++) _data[i] = monoidId;
            _lazy = new TMap[_size];
            for (var i = 0; i < _lazy.Length; i++) _lazy[i] = mapId;
            d.CopyTo(_data, _size);
            for (var i = _size - 1; i >= 1; i--) Update(i);
        }

        public void Set(int index, TMonoid monoid)
        {
            if (index < 0 || _length <= index) throw new IndexOutOfRangeException(nameof(index));
            index += _size;
            for (var i = _log; i >= 1; i--) Push(index >> i);
            _data[index] = monoid;
            for (var i = 1; i <= _log; i++) Update(index >> i);
        }

        public TMonoid Get(int index)
        {
            if (index < 0 || _length <= index) throw new IndexOutOfRangeException(nameof(index));
            index += _size;
            for (var i = _log; i >= 1; i--) Push(index >> i);
            return _data[index];
        }

        public TMonoid Query(int left, int right)
        {
            if (left < 0 || right < left || _length < right) throw new IndexOutOfRangeException();
            if (left == right) return _monoidId;
            left += _size;
            right += _size;
            for (var i = _log; i >= 1; i--)
            {
                if ((left >> i) << i != left) Push(left >> i);
                if ((right >> i) << i != right) Push(right >> i);
            }

            var (sml, smr) = (_monoidId, _monoidId);
            while (left < right)
            {
                if ((left & 1) == 1) sml = _operation(sml, _data[left++]);
                if ((right & 1) == 1) smr = _operation(_data[--right], smr);
                left >>= 1;
                right >>= 1;
            }

            return _operation(sml, smr);
        }

        public TMonoid QueryToAll() => _data[1];

        public void Apply(int index, TMap map)
        {
            if (index < 0 || _length <= index) throw new IndexOutOfRangeException(nameof(index));
            index += _size;
            for (var i = _log; i >= 1; i--) Push(index >> i);
            _data[index] = _mapping(map, _data[index]);
            for (var i = 1; i <= _log; i++) Update(index >> i);
        }

        public void Apply(int left, int right, TMap map)
        {
            if (left < 0 || right < left || _length < right) throw new IndexOutOfRangeException();
            if (left == right) return;
            left += _size;
            right += _size;
            for (var i = _log; i >= 1; i--)
            {
                if ((left >> i) << i != left) Push(left >> i);
                if ((right >> i) << i != right) Push((right - 1) >> i);
            }

            var (l, r) = (left, right);
            while (l < r)
            {
                if ((l & 1) == 1) ApplyToAll(l++, map);
                if ((r & 1) == 1) ApplyToAll(--r, map);
                l >>= 1;
                r >>= 1;
            }

            for (var i = 1; i <= _log; i++)
            {
                if ((left >> i) << i != left) Update(left >> i);
                if ((right >> i) << i != right) Update((right - 1) >> i);
            }
        }

        public int MaxRight(int left, Predicate<TMonoid> predicate)
        {
            if (left < 0 || _length < left) throw new IndexOutOfRangeException(nameof(left));
            if (!predicate(_monoidId)) throw new ArgumentException(nameof(predicate));
            if (left == _length) return _length;
            left += _size;
            for (var i = _log; i >= 1; i--) Push(left >> i);
            var sm = _monoidId;
            do
            {
                while ((left & 1) == 0) left >>= 1;
                if (!predicate(_operation(sm, _data[left])))
                {
                    while (left < _size)
                    {
                        Push(left);
                        left <<= 1;
                        var tmp = _operation(sm, _data[left]);
                        if (!predicate(tmp)) continue;
                        sm = tmp;
                        left++;
                    }

                    return left - _size;
                }

                sm = _operation(sm, _data[left]);
                left++;
            } while ((left & -left) != left);

            return _length;
        }

        public int MinLeft(int right, Predicate<TMonoid> predicate)
        {
            if (right < 0 || _length < right) throw new IndexOutOfRangeException(nameof(right));
            if (!predicate(_monoidId)) throw new ArgumentException(nameof(predicate));
            if (right == 0) return 0;
            right += _size;
            for (var i = _log; i >= 1; i--) Push((right - 1) >> i);
            var sm = _monoidId;
            do
            {
                right--;
                while (right > 1 && (right & 1) == 1) right >>= 1;
                if (!predicate(_operation(_data[right], sm)))
                {
                    while (right < _size)
                    {
                        Push(right);
                        right = (right << 1) + 1;
                        var tmp = _operation(_data[right], sm);
                        if (!predicate(tmp)) continue;
                        sm = tmp;
                        right--;
                    }

                    return right + 1 - _size;
                }

                sm = _operation(_data[right], sm);
            } while ((right & -right) != right);

            return 0;
        }

        private void Update(int k) => _data[k] = _operation(_data[k << 1], _data[(k << 1) + 1]);

        private void ApplyToAll(int k, TMap map)
        {
            _data[k] = _mapping(map, _data[k]);
            if (k < _size) _lazy[k] = _composition(map, _lazy[k]);
        }

        private void Push(int k)
        {
            ApplyToAll(k << 1, _lazy[k]);
            ApplyToAll((k << 1) + 1, _lazy[k]);
            _lazy[k] = _mapId;
        }
    }
}