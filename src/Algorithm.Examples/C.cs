using System;
using System.Linq;

namespace Algorithm.Examples
{
    public static class C
    {
        public static void Solve()
        {
            var T = int.Parse(Console.ReadLine());
            var answer = new long[T];
            for (var i = 0; i < T; i++)
            {
                var NMAB = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (N, M, A, B) = (NMAB[0], NMAB[1], NMAB[2], NMAB[3]);
                answer[i] = Mathematics.FloorSum(N, M, A, B);
            }

            foreach (var ans in answer) Console.WriteLine(ans);
        }
    }
}