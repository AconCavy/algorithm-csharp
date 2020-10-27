using System;

namespace AtCoderLibraryCSharp
{
    public class FenwickTree
    {
        private readonly int _length;
        private readonly long[] _data;

        private delegate bool Compare(long item, long data);

        private readonly Compare _lowerBound = (item, data) => item <= data;
        private readonly Compare _upperBound = (item, data) => item < data;

        public FenwickTree(int length = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;
            _data = new long[length];
        }

        public void Add(int index, long item)
        {
            if (index < 0 || _length <= index) throw new ArgumentOutOfRangeException(nameof(index));
            index++;
            while (index <= _length)
            {
                _data[index - 1] += item;
                index += index & -index;
            }
        }

        public long Sum(int length)
        {
            if (length < 0 || _length < length) throw new ArgumentOutOfRangeException(nameof(length));
            var s = 0L;
            while (length > 0)
            {
                s += _data[length - 1];
                length -= length & -length;
            }

            return s;
        }

        public long Sum(int left, int right)
        {
            if (left < 0 || right < left || _length < right) throw new ArgumentOutOfRangeException();
            return Sum(right) - Sum(left);
        }

        public int LowerBound(long item) => CommonBound(item, _lowerBound);
        public int UpperBound(long item) => CommonBound(item, _upperBound);

        private int CommonBound(long item, Compare compare)
        {
            if (compare(item, 0)) return 0;
            var x = 0;
            var r = 1;
            while (r < _length) r <<= 1;
            for (var k = r; k > 0; k >>= 1)
            {
                if (x + k - 1 >= _length || compare(item, _data[x + k - 1])) continue;
                item -= _data[x + k - 1];
                x += k;
            }

            return x;
        }
    }
}