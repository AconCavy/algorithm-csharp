using System;

namespace Algorithm
{
    public class FenwickTree
    {
        public int Length { get; }

        private readonly long[] _data;

        public FenwickTree(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            _data = new long[length];
        }

        public void Add(int index, long value)
        {
            if (index < 0 || Length <= index) throw new ArgumentOutOfRangeException(nameof(index));
            index++;
            while (index <= Length)
            {
                _data[index - 1] += value;
                index += index & -index;
            }
        }

        public long Sum(int length)
        {
            if (length < 0 || Length < length) throw new ArgumentOutOfRangeException(nameof(length));
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
            if (left < 0 || right < left || Length < right) throw new ArgumentOutOfRangeException();
            return Sum(right) - Sum(left);
        }

        public int LowerBound(long value) => Bound(value, (x, y) => x <= y);

        public int UpperBound(long value) => Bound(value, (x, y) => x < y);

        private int Bound(long value, Func<long, long, bool> compare)
        {
            if (Length == 0 || compare(value, _data[0])) return 0;
            var x = 0;
            var r = 1;
            while (r < Length) r <<= 1;
            for (var k = r; k > 0; k >>= 1)
            {
                if (x + k - 1 >= Length || compare(value, _data[x + k - 1])) continue;
                value -= _data[x + k - 1];
                x += k;
            }

            return x;
        }
    }
}