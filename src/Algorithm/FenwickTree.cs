using System;

namespace Algorithm
{
    public class FenwickTree
    {
        private readonly long[] _data;
        private readonly int _length;

        public FenwickTree(int length)
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

        public int LowerBound(long item) => CommonBound(item, LessThanOrEqual);

        public int UpperBound(long item) => CommonBound(item, LessThan);

        private int CommonBound(long item, Func<long, long, bool> compare)
        {
            if (compare(item, _data[0])) return 0;
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

        private static bool LessThanOrEqual(long x, long y) => x <= y;
        private static bool LessThan(long x, long y) => x < y;
    }
}