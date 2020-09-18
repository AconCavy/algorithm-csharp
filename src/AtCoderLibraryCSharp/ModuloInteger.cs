using System;

namespace AtCoderLibraryCSharp
{
    public readonly struct ModuloInteger
    {
        public long Value { get; }
        // for performance
        // public const long Modulo = 998244353;
        public static long Modulo { get; private set; } = 998244353;

        public ModuloInteger(long data) => Value = (0 <= data ? data : data + Modulo) % Modulo;
        public static implicit operator long(ModuloInteger mint) => mint.Value;
        public static implicit operator int(ModuloInteger mint) => (int) mint.Value;
        public static implicit operator ModuloInteger(long val) => new ModuloInteger(val);
        public static implicit operator ModuloInteger(int val) => new ModuloInteger(val);
        public static ModuloInteger operator +(ModuloInteger a, ModuloInteger b) => a.Value + b.Value;
        public static ModuloInteger operator +(ModuloInteger a, long b) => a.Value + b;
        public static ModuloInteger operator +(ModuloInteger a, int b) => a.Value + b;
        public static ModuloInteger operator -(ModuloInteger a, ModuloInteger b) => a.Value - b.Value;
        public static ModuloInteger operator -(ModuloInteger a, long b) => a.Value - b;
        public static ModuloInteger operator -(ModuloInteger a, int b) => a.Value - b;
        public static ModuloInteger operator *(ModuloInteger a, ModuloInteger b) => a.Value * b.Value;
        public static ModuloInteger operator *(ModuloInteger a, long b) => a.Value * (b % Modulo);
        public static ModuloInteger operator *(ModuloInteger a, int b) => a.Value * (b % Modulo);
        public static ModuloInteger operator /(ModuloInteger a, ModuloInteger b) => a * b.Inverse();
        public static ModuloInteger operator /(ModuloInteger a, long b) => a.Value * Inverse(b);
        public static ModuloInteger operator /(ModuloInteger a, int b) => a.Value * Inverse(b);
        public static bool operator ==(ModuloInteger a, ModuloInteger b) => a.Value == b.Value;
        public static bool operator !=(ModuloInteger a, ModuloInteger b) => a.Value != b.Value;
        public bool Equals(ModuloInteger other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ModuloInteger other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public ModuloInteger Inverse() => Inverse(Value);

        public static ModuloInteger Inverse(long a)
        {
            if (a == 0) return 0;
            var p = Modulo;
            var (x1, y1, x2, y2) = (1L, 0L, 0L, 1L);
            while (true)
            {
                if (p == 1) return (x2 % Modulo + Modulo) % Modulo;
                var div = a / p;
                x1 -= x2 * div;
                y1 -= y2 * div;
                a %= p;
                if (a == 1) return (x1 % Modulo + Modulo) % Modulo;
                div = p / a;
                x2 -= x1 * div;
                y2 -= y1 * div;
                p %= a;
            }
        }

        public ModuloInteger Power(long n) => Power(Value, n);

        public static ModuloInteger Power(ModuloInteger x, long n)
        {
            if (n < 0) throw new ArgumentException();
            var r = new ModuloInteger(1);
            while (n > 0)
            {
                if ((n & 1) > 0) r *= x;
                x *= x;
                n >>= 1;
            }

            return r;
        }

        public static void SetMod(long m) => Modulo = m;
        public static void SetMod998244353() => SetMod(998244353);
        public static void SetMod1000000007() => SetMod(1000000007);
    }
}