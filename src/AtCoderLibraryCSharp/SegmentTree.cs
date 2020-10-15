using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class SegmentTree<TMonoid>
    {
        public delegate TMonoid Operation(TMonoid a, TMonoid b);

        private readonly int _length;
        private readonly int _size;
        private readonly int _log;
        private readonly TMonoid[] _data;
        private readonly Operation _operation;
        private readonly TMonoid _monoidId;

        public SegmentTree(int length, Operation operation, TMonoid monoidId)
            : this(Enumerable.Repeat(monoidId, length), operation, monoidId)
        {
        }

        public SegmentTree(IEnumerable<TMonoid> data, Operation operation, TMonoid monoidId)
        {
            var d = data.ToArray();
            _length = d.Length;
            _operation = operation;
            _monoidId = monoidId;
            while (1 << _log < _length) _log++;
            _size = 1 << _log;
            _data = Enumerable.Repeat(monoidId, _size << 1).ToArray();
            d.CopyTo(_data, _size);
            for (var i = _size - 1; i >= 1; i--) Update(i);
        }

        public void Set(int index, TMonoid monoid)
        {
            if (index < 0 || _length <= index) throw new IndexOutOfRangeException(nameof(index));
            index += _size;
            _data[index] = monoid;
            for (var i = 1; i <= _log; i++) Update(index >> i);
        }

        public TMonoid Get(int index)
        {
            if (index < 0 || _length <= index) throw new IndexOutOfRangeException(nameof(index));
            return _data[index + _size];
        }

        public TMonoid Query(int left, int right)
        {
            if (left < 0 || right < left || _length < right) throw new IndexOutOfRangeException();
            var (sml, smr) = (_monoidId, _monoidId);
            left += _size;
            right += _size;
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

        public int MaxRight(int left, Predicate<TMonoid> predicate)
        {
            if (left < 0 || _length < left) throw new IndexOutOfRangeException(nameof(left));
            if (!predicate(_monoidId)) throw new ArgumentException(nameof(predicate));
            if (left == _length) return _length;
            left += _size;
            var sm = _monoidId;
            do
            {
                while ((left & 1) == 0) left >>= 1;
                if (!predicate(_operation(sm, _data[left])))
                {
                    while (left < _size)
                    {
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
            var sm = _monoidId;
            do
            {
                right--;
                while (right > 1 && (right & 1) == 1) right >>= 1;
                if (!predicate(_operation(_data[right], sm)))
                {
                    while (right < _size)
                    {
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
    }
}