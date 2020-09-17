using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class L
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(int.Parse)
                .Select(x => x == 0 ? new S(1, 0, 0) : new S(0, 1, 0)).ToArray();

            static S Operation(S l, S r) =>
                new S(l.Zero + r.Zero, l.One + r.One, l.Inversion + r.Inversion + l.One * r.Zero);

            var identityS = new S(0, 0, 0);
            static S Mapping(bool l, S r) => l ? new S(r.One, r.Zero, r.One * r.Zero - r.Inversion) : r;
            static bool Composition(bool l, bool r) => (l && !r) || (!l && r);
            const bool identityF = false;

            var lst = new LazySegmentTree<S, bool>(A, Operation, identityS, Mapping, Composition, identityF);
            for (var i = 0; i < Q; i++)
            {
                var TLR = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (T, L, R) = (TLR[0], TLR[1], TLR[2]);
                L--;
                if (T == 1) lst.Apply(L, R, true);
                else Console.WriteLine(lst.Query(L, R).Inversion);
            }
        }

        public struct S
        {
            public long Zero;
            public long One;
            public long Inversion;

            public S(long zero, long one, long inversion)
            {
                Zero = zero;
                One = one;
                Inversion = inversion;
            }
        }
    }
}