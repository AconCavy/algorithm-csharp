using System;
using System.Linq;

namespace AlgorithmSharp.Examples
{
    public static class A
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var dsu = new DisjointSetUnion(N);
            for (var i = 0; i < Q; i++)
            {
                var TUV = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (t, u, v) = (TUV[0], TUV[1], TUV[2]);
                if (t == 0) dsu.Merge(u, v);
                else Console.WriteLine(dsu.IsSame(u, v) ? 1 : 0);
            }
        }
    }
}