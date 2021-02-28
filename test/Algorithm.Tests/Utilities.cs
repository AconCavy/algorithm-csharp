using System;

namespace Algorithm.Tests
{
    public static class Utilities
    {
        private static readonly Random Random = new Random(100);

        public static int RandomInteger(int lower, int upper) => Random.Next(lower, upper + 1);

        public static (int, int) RandomPair(int lower, int upper)
        {
            var (a, b) = (0, 0);
            do
            {
                a = RandomInteger(lower, upper);
                b = RandomInteger(lower, upper);
            } while (a == b);

            return a <= b ? (a, b) : (b, a);
        }

        public static bool RandomBool() => RandomInteger(0, 1) == 0;
    }
}