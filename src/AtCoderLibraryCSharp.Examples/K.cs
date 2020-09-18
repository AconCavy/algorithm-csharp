using System;
using System.IO;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class K
    {
        public static void Solve()
        {
            var sw = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = false};
             Console.SetOut(sw);

             var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
             var (N, Q) = (NQ[0], NQ[1]);
             var A = Console.ReadLine().Split(" ").Select(x => new S(int.Parse(x), 1)).ToArray();

             var identityS = new S(0, 0);
             var identityF = new F(1, 0);

             static S Operation(S l, S r) => new S(l.A + r.A, l.Size + r.Size);
             static S Mapping(F l, S r) => new S(r.A * l.A + r.Size * l.B, r.Size);
             static F Composition(F l, F r) => new F(r.A * l.A, r.B * l.A + l.B);

             var lst = new LazySegmentTree<S, F>(A, Operation, identityS, Mapping, Composition, identityF);

             for (var i = 0; i < Q; i++)
             {
                 var q = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                 if (q[0] == 0) lst.Apply(q[1], q[2], new F(q[3], q[4]));
                 else Console.WriteLine(lst.Query(q[1], q[2]).A);
             }

             Console.Out.Flush();
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