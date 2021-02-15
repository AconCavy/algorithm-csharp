using System;
using System.Linq;

namespace AlgorithmSharp.Examples
{
    public static class B
    {
        public static void Solve()
        {
            var NQ = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, Q) = (NQ[0], NQ[1]);
            var A = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();

            var ft = new FenwickTree(N);
            for (var i = 0; i < N; i++) ft.Add(i, A[i]);

            for (var i = 0; i < Q; i++)
            {
                var TPX = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (t, p, x) = (TPX[0], TPX[1], TPX[2]);
                if (t == 0) ft.Add(p, x);
                else Console.WriteLine(ft.Sum(p, x));
            }
        }
    }
}