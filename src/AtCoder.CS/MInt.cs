namespace AtCoder.CS
{
    public readonly struct MInt
    {
        public long Value => _data;
        private const int Mod = 998244353;
        private readonly long _data;
        public MInt(long data) => _data = (0 <= data ? data : data + Mod) % Mod;
        public static implicit operator long(MInt mint) => mint._data;
        public static implicit operator MInt(long val) => new MInt(val);
        public static MInt operator +(MInt a, MInt b) => new MInt(a._data + b._data);
        public static MInt operator +(MInt a, long b) => a._data + b;
        public static MInt operator -(MInt a, MInt b) => new MInt(a._data - b._data);
        public static MInt operator -(MInt a, long b) => a._data - b;
        public static MInt operator *(MInt a, MInt b) => new MInt(a._data * b._data);
        public static MInt operator *(MInt a, long b) => a._data * (b % Mod);
        public static MInt operator /(MInt a, MInt b) => new MInt(a._data * GetInv(b));
        public static bool operator ==(MInt a, MInt b) => a._data == b._data;
        public static bool operator !=(MInt a, MInt b) => a._data != b._data;
        public override bool Equals(object obj) => _data.Equals(obj);
        public override int GetHashCode() => _data.GetHashCode();
        public override string ToString() => _data.ToString();

        private static long GetInv(long a)
        {
            a %= Mod;
            if (a < 0) a += Mod;
            if (a == 0) return 0;
            var (s, t, m0, m1) = ((long)Mod, a, 0L, 1L);
            while (t > 1)
            {
                var u = s / t;
                s -= t * u;
                m0 -= m1 * u;
                (s, t) = (t, s);
                (m0, m1) = (m1, m0);
            }

            if (m0 < 0) m0 += Mod / s;
            return m0;
        }
    }
}