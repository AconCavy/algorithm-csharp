namespace AtCoder.CS
{
    public readonly struct MInt
    {
        private const int Mod = (int) 1e9 + 7;
        private readonly long _data;
        public MInt(long data) => _data = (0 <= data ? data : data + Mod) % Mod;
        public static implicit operator long(MInt mint) => mint._data;
        public static implicit operator MInt(long val) => new MInt(val);
        public static MInt operator +(MInt a, MInt b) => a._data + b._data;
        public static MInt operator -(MInt a, MInt b) => a._data - b._data;
        public static MInt operator *(MInt a, MInt b) => a._data * b._data;
        public static MInt operator /(MInt a, MInt b) => a._data / b._data;
        public static bool operator ==(MInt a, MInt b) => a._data == b._data;
        public static bool operator !=(MInt a, MInt b) => a._data != b._data;
        public override bool Equals(object obj) => _data.Equals(obj);
        public override int GetHashCode() => _data.GetHashCode();
        public override string ToString() => _data.ToString();
    }
}