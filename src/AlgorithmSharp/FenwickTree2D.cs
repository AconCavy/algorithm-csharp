using System;

namespace AlgorithmSharp
{
    public class FenwickTree2D
    {
        private readonly long[,] _data;
        private readonly int _height;
        private readonly int _width;

        public FenwickTree2D(int h, int w)
        {
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h));
            if (w <= 0) throw new ArgumentOutOfRangeException(nameof(w));
            _height = h;
            _width = w;
            _data = new long[_height, _width];
        }

        public void Add(int h, int w, long value)
        {
            if (h < 0 || _height <= h) throw new ArgumentOutOfRangeException(nameof(h));
            if (w < 0 || _width <= w) throw new ArgumentOutOfRangeException(nameof(w));
            for (var i = h + 1; i <= _height; i += i & -i)
            for (var j = w + 1; j <= _width; j += j & -j)
                _data[i - 1, j - 1] += value;
        }

        public long Sum(int h, int w)
        {
            if (h < 0 || _height <= h) throw new ArgumentOutOfRangeException(nameof(h));
            if (w < 0 || _width <= w) throw new ArgumentOutOfRangeException(nameof(w));
            var sum = 0L;
            for (var i = h + 1; i > 0; i -= i & -i)
            for (var j = w + 1; j > 0; j -= j & -j)
                sum += _data[i - 1, j - 1];
            return sum;
        }

        public long Sum(int h1, int w1, int h2, int w2)
        {
            if (h1 < 0 || _height <= h1) throw new ArgumentOutOfRangeException(nameof(h1));
            if (w1 < 0 || _width <= w1) throw new ArgumentOutOfRangeException(nameof(w1));
            if (h2 < 0 || _height <= h2) throw new ArgumentOutOfRangeException(nameof(h2));
            if (w2 < 0 || _width <= w2) throw new ArgumentOutOfRangeException(nameof(w2));
            return Sum(h1, w1) + Sum(h2, w2) - Sum(h2, w1) - Sum(h1, w2);
        }
    }
}