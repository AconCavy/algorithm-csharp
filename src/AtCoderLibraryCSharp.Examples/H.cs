using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class H
    {
        public static void Solve()
        {
            var ND = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, D) = (ND[0], ND[1]);
            var X = new int[N];
            var Y = new int[N];
            for (var i = 0; i < N; i++)
            {
                var XY = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                (X[i], Y[i]) = (XY[0], XY[1]);
            }

            var ts = new TwoSatisfiability(N);
            for (var i = 0; i < N; i++)
            {
                for (var j = i + 1; j < N; j++)
                {
                    if (System.Math.Abs(X[i] - X[j]) < D) ts.AddClause(i, false, j, false);
                    if (System.Math.Abs(X[i] - Y[j]) < D) ts.AddClause(i, false, j, true);
                    if (System.Math.Abs(Y[i] - X[j]) < D) ts.AddClause(i, true, j, false);
                    if (System.Math.Abs(Y[i] - Y[j]) < D) ts.AddClause(i, true, j, true);
                }
            }

            if (!ts.IsSatisfiable())
            {
                Console.WriteLine("No");
                return;
            }

            Console.WriteLine("Yes");
            var answer = ts.Answer;
            for (var i = 0; i < N; i++)
                Console.WriteLine(answer[i] ? X[i] : Y[i]);
        }
    }
}