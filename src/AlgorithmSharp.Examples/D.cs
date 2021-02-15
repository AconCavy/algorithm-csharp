using System;
using System.Linq;

namespace AlgorithmSharp.Examples
{
    public static class D
    {
        public static void Solve()
        {
            var NM = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, M) = (NM[0], NM[1]);
            var G = new char[N][].Select(x => Console.ReadLine().ToCharArray()).ToArray();
            var fg = new FlowGraph(N * M + 2);
            var s = N * M;
            var t = N * M + 1;

            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                {
                    if (G[i][j] == '#') continue;
                    var v = i * M + j;
                    if ((i + j) % 2 == 0) fg.AddEdge(s, v, 1);
                    else fg.AddEdge(v, t, 1);
                }

            var di = new[] { -1, 0, 1, 0 };
            var dj = new[] { 0, -1, 0, 1 };
            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                {
                    if ((i + j) % 2 == 1 || G[i][j] == '#') continue;
                    var v0 = i * M + j;
                    for (var k = 0; k < 4; k++)
                    {
                        var ci = i + di[k];
                        var cj = j + dj[k];
                        if (ci < 0 || N <= ci) continue;
                        if (cj < 0 || M <= cj) continue;
                        if (G[ci][cj] != '.') continue;
                        var v1 = ci * M + cj;
                        fg.AddEdge(v0, v1, 1);
                    }
                }

            Console.WriteLine(fg.MaxFlow(s, t));

            foreach (var edge in fg.GetEdges().Where(x => x.From != s && x.To != t && x.Flow != 0))
            {
                var (i0, j0) = (edge.From / M, edge.From % M);
                var (i1, j1) = (edge.To / M, edge.To % M);

                if (i0 == i1 + 1)
                {
                    G[i1][j1] = 'v';
                    G[i0][j0] = '^';
                }
                else if (j0 == j1 + 1)
                {
                    G[i1][j1] = '>';
                    G[i0][j0] = '<';
                }
                else if (i0 == i1 - 1)
                {
                    G[i0][j0] = 'v';
                    G[i1][j1] = '^';
                }
                else
                {
                    G[i0][j0] = '>';
                    G[i1][j1] = '<';
                }
            }

            Console.WriteLine(string.Join("\n", G.Select(x => new string(x))));
        }
    }
}