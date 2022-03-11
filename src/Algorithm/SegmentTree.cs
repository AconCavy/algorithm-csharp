using System;
using System.Collections.Generic;

namespace Algorithm
{
    public class SegmentTree<TMonoid>
    {
        public int Length { get; }

        private readonly IOracle<TMonoid> _oracle;
        private readonly TMonoid[] _data;
        private readonly int _log;
        private readonly int _dataSize;

        public SegmentTree(IReadOnlyCollection<TMonoid> source, IOracle<TMonoid> oracle) : this(source.Count, oracle)
        {
            var idx = _dataSize;
            foreach (var value in source) _data[idx++] = value;
            for (var i = _dataSize - 1; i >= 1; i--) Update(i);
        }

        public SegmentTree(int length, IOracle<TMonoid> oracle)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            _oracle = oracle;
            while (1 << _log < Length) _log++;
            _dataSize = 1 << _log;
            _data = new TMonoid[_dataSize << 1];
            Array.Fill(_data, oracle.MonoidIdentity);
        }

        public void Set(int index, TMonoid value)
        {
            if (index < 0 || Length <= index) throw new ArgumentOutOfRangeException(nameof(index));
            index += _dataSize;
            _data[index] = value;
            for (var i = 1; i <= _log; i++) Update(index >> i);
        }

        public TMonoid Get(int index)
        {
            if (index < 0 || Length <= index) throw new ArgumentOutOfRangeException(nameof(index));
            return _data[index + _dataSize];
        }

        public TMonoid Query(int left, int right)
        {
            if (left < 0 || right < left || Length < right) throw new ArgumentOutOfRangeException();
            var (sml, smr) = (_oracle.MonoidIdentity, _oracle.MonoidIdentity);
            left += _dataSize;
            right += _dataSize;
            while (left < right)
            {
                if ((left & 1) == 1) sml = _oracle.Operate(sml, _data[left++]);
                if ((right & 1) == 1) smr = _oracle.Operate(_data[--right], smr);
                left >>= 1;
                right >>= 1;
            }

            return _oracle.Operate(sml, smr);
        }

        public TMonoid QueryToAll() => _data[1];

        public int MaxRight(int left, Func<TMonoid, bool> predicate)
        {
            if (left < 0 || Length < left) throw new ArgumentOutOfRangeException(nameof(left));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (!predicate(_oracle.MonoidIdentity)) throw new ArgumentException(nameof(predicate));
            if (left == Length) return Length;
            left += _dataSize;
            var sm = _oracle.MonoidIdentity;
            do
            {
                while ((left & 1) == 0) left >>= 1;
                if (!predicate(_oracle.Operate(sm, _data[left])))
                {
                    while (left < _dataSize)
                    {
                        left <<= 1;
                        var tmp = _oracle.Operate(sm, _data[left]);
                        if (!predicate(tmp)) continue;
                        sm = tmp;
                        left++;
                    }

                    return left - _dataSize;
                }

                sm = _oracle.Operate(sm, _data[left]);
                left++;
            } while ((left & -left) != left);

            return Length;
        }

        public int MinLeft(int right, Func<TMonoid, bool> predicate)
        {
            if (right < 0 || Length < right) throw new ArgumentOutOfRangeException(nameof(right));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (!predicate(_oracle.MonoidIdentity)) throw new ArgumentException(nameof(predicate));
            if (right == 0) return 0;
            right += _dataSize;
            var sm = _oracle.MonoidIdentity;
            do
            {
                right--;
                while (right > 1 && (right & 1) == 1) right >>= 1;
                if (!predicate(_oracle.Operate(_data[right], sm)))
                {
                    while (right < _dataSize)
                    {
                        right = (right << 1) + 1;
                        var tmp = _oracle.Operate(_data[right], sm);
                        if (!predicate(tmp)) continue;
                        sm = tmp;
                        right--;
                    }

                    return right + 1 - _dataSize;
                }

                sm = _oracle.Operate(_data[right], sm);
            } while ((right & -right) != right);

            return 0;
        }

        private void Update(int k) => _data[k] = _oracle.Operate(_data[k << 1], _data[(k << 1) + 1]);
    }
}