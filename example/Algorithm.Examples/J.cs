using System;
using System.Linq;

namespace Algorithm.Examples
{
    public static class J
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(x => new S(int.Parse(x))).ToArray();
            var st = new SegmentTree<S>(A, new Oracle());

            for (var i = 0; i < Q; i++)
            {
                var TXY = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (T, X, V) = (TXY[0], TXY[1], TXY[2]);
                X--;
                switch (T)
                {
                    case 1:
                        st.Set(X, new S(V));
                        break;
                    case 2:
                        Console.WriteLine(st.Query(X, V).Value);
                        break;
                    case 3:
                        Console.WriteLine(st.MaxRight(X, v => v.Value < V) + 1);
                        break;
                }
            }
        }

        public readonly struct S
        {
            public readonly int Value;

            public S(int value) => Value = value;
        }

        public class Oracle : IOracle<S>
        {
            public S MonoidIdentity { get; } = new S(-1);

            public S Operate(S a, S b) => new S(Math.Max(a.Value, b.Value));
        }
    }
}