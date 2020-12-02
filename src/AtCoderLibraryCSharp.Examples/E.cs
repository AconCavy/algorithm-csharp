using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class E
    {
        public static void Solve()
        {
            var NK = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, K) = (NK[0], NK[1]);
            var A = new long[N][].Select(x =>
                    Console.ReadLine().Split(" ").Select(long.Parse).ToArray())
                .ToArray();
            var fg = new FlowGraph(N * 2 + 2);
            var s = N * 2;
            var t = N * 2 + 1;
            const long inf = (long) 1e9;
            fg.AddEdge(s, t, N * K, inf);
            for (var i = 0; i < N; i++)
            {
                fg.AddEdge(s, i, K, 0);
                fg.AddEdge(N + i, t, K, 0);
            }

            for (var i = 0; i < N; i++)
            for (var j = 0; j < N; j++)
                fg.AddEdge(i, N + j, 1, inf - A[i][j]);

            var result = fg.MinCostFlow(s, t, N * K);
            Console.WriteLine(inf * N * K - result.Item2);

            var G = new char[N][].Select(x => Enumerable.Repeat('.', N).ToArray()).ToArray();
            foreach (var edge in fg.GetEdges().Where(x => x.From != s && x.To != t && x.Flow != 0))
                G[edge.From][edge.To - N] = 'X';

            Console.WriteLine(string.Join("\n", G.Select(x => new string(x))));
        }
    }
}