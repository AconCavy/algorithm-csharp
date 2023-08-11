using System;
using System.Numerics;

namespace Algorithm
{
    public readonly struct ModuloInteger : IEquatable<ModuloInteger>,
        IAdditionOperators<ModuloInteger, ModuloInteger, ModuloInteger>,
        IDivisionOperators<ModuloInteger, ModuloInteger, ModuloInteger>,
        IMultiplyOperators<ModuloInteger, ModuloInteger, ModuloInteger>,
        ISubtractionOperators<ModuloInteger, ModuloInteger, ModuloInteger>,
        IEqualityOperators<ModuloInteger, ModuloInteger, bool>
    {
        public long Value { get; }
        // The modulo will be used as an editable property.
        public static long Modulo { get; set; } = 998244353;
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
        public static implicit operator ModuloInteger(int value) => new(value);
        public static implicit operator ModuloInteger(long value) => new(value);
        public static ModuloInteger operator +(ModuloInteger left, ModuloInteger right) => left.Value + right.Value;
        public static ModuloInteger operator -(ModuloInteger left, ModuloInteger right) => left.Value - right.Value;
        public static ModuloInteger operator *(ModuloInteger left, ModuloInteger right) => left.Value * right.Value;
        public static ModuloInteger operator /(ModuloInteger left, ModuloInteger right) => left * right.Inverse();
        public static bool operator ==(ModuloInteger left, ModuloInteger right) => left.Equals(right);
        public static bool operator !=(ModuloInteger left, ModuloInteger right) => !left.Equals(right);
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