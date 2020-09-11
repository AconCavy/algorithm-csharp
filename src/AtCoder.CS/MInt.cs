using System;

namespace AtCoder.CS
{
    public readonly struct MInt
    {
        public long Value { get; }
        public static long Mod { get; private set; } = 998244353;

        public MInt(long data) => Value = (0 <= data ? data : data + Mod) % Mod;
        public static implicit operator long(MInt mint) => mint.Value;
        public static implicit operator MInt(long val) => new MInt(val);
        public static MInt operator +(MInt a, MInt b) => a.Value + b.Value;
        public static MInt operator +(MInt a, long b) => a.Value + b;
        public static MInt operator -(MInt a, MInt b) => a.Value - b.Value;
        public static MInt operator -(MInt a, long b) => a.Value - b;
        public static MInt operator *(MInt a, MInt b) => a.Value * b.Value;
        public static MInt operator *(MInt a, long b) => a.Value * (b % Mod);
        public static MInt operator /(MInt a, MInt b) => a.Value * b.Inverse();
        public static MInt operator /(MInt a, long b) => a.Value * Inverse(b);
        public static bool operator ==(MInt a, MInt b) => a.Value == b.Value;
        public static bool operator !=(MInt a, MInt b) => a.Value != b.Value;
        public bool Equals(MInt other) => Value == other.Value;
        public override bool Equals(object obj) => obj is MInt other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public long Inverse() => Inverse(Value);

        public static long Inverse(long a)
        {
            var p = Mod;
            var (x1, y1, x2, y2) = (1L, 0L, 0L, 1L);
            while (true)
            {
                if (p == 1) return (x2 % Mod + Mod) % Mod;
                var div = a / p;
                x1 -= x2 * div;
                y1 -= y2 * div;
                a %= p;
                if (a == 1) return (x1 % Mod + Mod) % Mod;
                div = p / a;
                x2 -= x1 * div;
                y2 -= y1 * div;
                p %= a;
            }
        }

        public MInt Power(long n)
        {
            if (n < 0) throw new ArgumentException();
            var x = Value;
            var r = new MInt(1);
            while (n > 0)
            {
                if ((n & 1) > 0) r *= x;
                x *= x;
                n >>= 1;
            }

            return r;
        }

        public static void SetMod(long m) => Mod = m;
        public static void SetMod998244353() => SetMod(998244353);
        public static void SetMod1000000007() => SetMod(1000000007);
    }
}