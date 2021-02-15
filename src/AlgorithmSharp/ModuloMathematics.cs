using System;

namespace AlgorithmSharp
{
    public static class ModuloMathematics
    {
        private const int DefaultLength = 32;
        private static ModuloInteger[] _factorial;
        private static ModuloInteger[] _factorialInverse;
        private static int _length;

        static ModuloMathematics()
        {
            _factorial = new ModuloInteger[DefaultLength + 1];
            _factorial[0] = 1;
            _factorialInverse = new ModuloInteger[DefaultLength + 1];
            _factorialInverse[0] = 1;
            Initialize(DefaultLength);
        }

        public static ModuloInteger Combination(int n, int r)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (r < 0 || n < r) throw new ArgumentException(nameof(r));
            if (_length < n) Initialize(n);

            return _factorial[n] * _factorialInverse[r] * _factorialInverse[n - r];
        }

        public static ModuloInteger Permutation(int n, int r)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (r < 0 || n < r) throw new ArgumentException(nameof(r));
            if (_length < n) Initialize(n);

            return _factorial[n] * _factorialInverse[n - r];
        }

        public static ModuloInteger Factorial(int n)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            if (_length < n) Initialize(n);
            return _factorial[n];
        }

        private static void Initialize(int n)
        {
            if (_length < n)
            {
                Array.Resize(ref _factorial, n + 1);
                Array.Resize(ref _factorialInverse, n + 1);
            }

            for (var i = _length + 1; i <= n; i++)
            {
                _factorial[i] = _factorial[i - 1] * i;
                _factorialInverse[i] = 1 / _factorial[i];
            }

            _length = n;
        }
    }
}