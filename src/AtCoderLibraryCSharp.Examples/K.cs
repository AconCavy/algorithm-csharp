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

            var lst = new LazySegmentTree<S, F>(A, new Oracle());

            for (var i = 0; i < Q; i++)
            {
                var q = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                if (q[0] == 0) lst.Apply(q[1], q[2], new F(q[3], q[4]));
                else Console.WriteLine(lst.Query(q[1], q[2]).A);
            }
        }

        public readonly struct S
        {
            public readonly ModuloInteger A;
            public readonly int Size;
            public S(ModuloInteger a, int size) => (A, Size) = (a, size);
        }

        public readonly struct F
        {
            public readonly ModuloInteger A;
            public readonly ModuloInteger B;
            public F(ModuloInteger a, ModuloInteger b) => (A, B) = (a, b);
        }

        public class Oracle : IOracle<S, F>
        {
            public S MonoidIdentity { get; } = new S(0, 0);
            public S Operate(in S a, in S b) => new S(a.A + b.A, a.Size + b.Size);

            public F MapIdentity { get; } = new F(1, 0);
            public S Map(in F f, in S x) => new S(f.A * x.A + f.B * x.Size, x.Size);
            public F Compose(in F f, in F g) => new F(f.A * g.A, f.A * g.B + f.B);
        }
    }
}