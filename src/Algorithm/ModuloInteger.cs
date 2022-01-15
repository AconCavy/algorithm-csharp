using System;

namespace Algorithm
{
    public readonly struct ModuloInteger : IEquatable<ModuloInteger>
    {
        public long Value { get; }
        public static long Modulo { get; set; } = 998244353;
        // The modulo will be used as an editable property.
        // The constant modulo will be recommended to use for performances in use cases.
        // public const long Modulo = 1000000007;

        public ModuloInteger(int value)
        {
            Value = value % Modulo;
            if (Value < 0) Value += Modulo;
        }

        public ModuloInteger(long value)
        {
            Value = value % Modulo;
            if (Value < 0) Value += Modulo;
        }

        public static implicit operator int(ModuloInteger mint) => (int)mint.Value;
        public static implicit operator long(ModuloInteger mint) => mint.Value;
        public static implicit operator ModuloInteger(int value) => new ModuloInteger(value);
        public static implicit operator ModuloInteger(long value) => new ModuloInteger(value);
        public static ModuloInteger operator +(ModuloInteger a, ModuloInteger b) => a.Value + b.Value;
        public static ModuloInteger operator -(ModuloInteger a, ModuloInteger b) => a.Value - b.Value;
        public static ModuloInteger operator *(ModuloInteger a, ModuloInteger b) => a.Value * b.Value;
        public static ModuloInteger operator /(ModuloInteger a, ModuloInteger b) => a * b.Inverse();
        public static ModuloInteger operator +(ModuloInteger a, int b) => a.Value + b;
        public static ModuloInteger operator +(int a, ModuloInteger b) => a + b.Value;
        public static ModuloInteger operator -(ModuloInteger a, int b) => a.Value - b;
        public static ModuloInteger operator -(int a, ModuloInteger b) => a - b.Value;
        public static ModuloInteger operator *(ModuloInteger a, int b) => a.Value * b;
        public static ModuloInteger operator *(int a, ModuloInteger b) => a * b.Value;
        public static ModuloInteger operator /(ModuloInteger a, int b) => a * Inverse(b);
        public static ModuloInteger operator /(int a, ModuloInteger b) => a * b.Inverse();
        public static ModuloInteger operator +(ModuloInteger a, long b) => a.Value + b;
        public static ModuloInteger operator +(long a, ModuloInteger b) => a + b.Value;
        public static ModuloInteger operator -(ModuloInteger a, long b) => a.Value - b;
        public static ModuloInteger operator -(long a, ModuloInteger b) => a - b.Value;
        public static ModuloInteger operator *(ModuloInteger a, long b) => a.Value * b;
        public static ModuloInteger operator *(long a, ModuloInteger b) => a * b.Value;
        public static ModuloInteger operator /(ModuloInteger a, long b) => a * Inverse(b);
        public static ModuloInteger operator /(long a, ModuloInteger b) => a * b.Inverse();
        public static bool operator ==(ModuloInteger a, ModuloInteger b) => a.Equals(b);
        public static bool operator !=(ModuloInteger a, ModuloInteger b) => !a.Equals(b);
        public bool Equals(ModuloInteger other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ModuloInteger other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public ModuloInteger Inverse() => Inverse(Value);

        public static ModuloInteger Inverse(long value)
        {
            if (value == 0) return 0;
            var (s, t, m0, m1) = (Modulo, value, 0L, 1L);
            while (t > 0)
            {
                var u = s / t;
                s -= t * u;
                m0 -= m1 * u;
                (s, t) = (t, s);
                (m0, m1) = (m1, m0);
            }

            if (m0 < 0) m0 += Modulo / s;
            return m0;
        }

        public ModuloInteger Power(long n) => Power(Value, n);

        public static ModuloInteger Power(long value, long n)
        {
            if (n < 0) throw new ArgumentException(nameof(n));
            var result = 1L;
            while (n > 0)
            {
                if ((n & 1) > 0) result = result * value % Modulo;
                value = value * value % Modulo;
                n >>= 1;
            }

            return result;
        }
    }
}