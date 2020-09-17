using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class K
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(x => new S(int.Parse(x), 1)).ToArray();
            static S Operation(S l, S r) => new S(l.A + r.A, l.Size + r.Size);
            var identityS = new S(0, 0);
            static S Mapping(F l, S r) => new S(r.A * l.A + r.Size * l.B, r.Size);
            static F Composition(F l, F r) => new F(r.A * l.A, r.B * l.A + l.B);
            var identityF = new F(1, 0);

            var lst = new LazySegmentTree<S, F>(A, Operation, identityS, Mapping, Composition, identityF);

            for (var i = 0; i < Q; i++)
            {
                var q = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                if (q[0] == 0)
                {
                    var (l, r, c, d) = (q[1], q[2], q[3], q[4]);
                    lst.Apply(l, r, new F(c, d));
                }
                else
                {
                    var (l, r) = (q[1], q[2]);
                    Console.WriteLine(lst.Query(l, r).A);
                }
            }
        }

        public readonly struct S
        {
            public readonly ModuloInteger A;
            public readonly int Size;

            public S(ModuloInteger a, int size)
            {
                A = a;
                Size = size;
            }
        }

        public readonly struct F
        {
            public readonly ModuloInteger A;
            public readonly ModuloInteger B;

            public F(ModuloInteger a, ModuloInteger b)
            {
                A = a;
                B = b;
            }
        }
    }
}