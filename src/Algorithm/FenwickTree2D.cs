using System;

namespace Algorithm
{
    public class FenwickTree2D
    {
        public int Height { get; }
        public int Width { get; }

        private readonly long[] _data;

        public FenwickTree2D(int height, int width)
        {
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            Height = height;
            Width = width;
            _data = new long[Height * Width];
        }

        public void Add(int height, int width, long value)
        {
            if (height < 0 || Height <= height) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0 || Width <= width) throw new ArgumentOutOfRangeException(nameof(width));
            for (var i = height + 1; i <= Height; i += i & -i)
            {
                for (var j = width + 1; j <= Width; j += j & -j)
                {
                    _data[(i - 1) * Width + (j - 1)] += value;
                }
            }
        }

        public long Sum(int height, int width)
        {
            if (height < 0 || Height < height) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0 || Width < width) throw new ArgumentOutOfRangeException(nameof(width));
            var sum = 0L;
            for (var i = height; i > 0; i -= i & -i)
            {
                for (var j = width; j > 0; j -= j & -j)
                {
                    sum += _data[(i - 1) * Width + (j - 1)];
                }
            }

            return sum;
        }

        public long Sum(int height1, int width1, int height2, int width2)
        {
            if (height1 < 0 || Height < height1) throw new ArgumentOutOfRangeException(nameof(height1));
            if (width1 < 0 || Width < width1) throw new ArgumentOutOfRangeException(nameof(width1));
            if (height2 < 0 || Height < height2) throw new ArgumentOutOfRangeException(nameof(height2));
            if (width2 < 0 || Width < width2) throw new ArgumentOutOfRangeException(nameof(width2));
            return Sum(height1, width1) + Sum(height2, width2) - Sum(height2, width1) - Sum(height1, width2);
        }
    }
}