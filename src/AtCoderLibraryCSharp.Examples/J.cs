using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class J
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var st = new SegmentTree<int>(A, (a, b) => System.Math.Max(a, b), -1);

            for (var i = 0; i < Q; i++)
            {
                var TXY = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (T, X, V) = (TXY[0], TXY[1], TXY[2]);
                X--;
                switch (T)
                {
                    case 1:
                        st.Set(X, V);
                        break;
                    case 2:
                        Console.WriteLine(st.Query(X, V));
                        break;
                    case 3:
                        Console.WriteLine(st.MaxRight(X, v => v < V) + 1);
                        break;
                }
            }
        }
    }
}