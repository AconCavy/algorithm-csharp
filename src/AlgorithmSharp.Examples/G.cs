using System;
using System.Linq;

namespace AlgorithmSharp.Examples
{
    public static class G
    {
        public static void Solve()
        {
            var NM = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, M) = (NM[0], NM[1]);
            var G = new StronglyConnectedComponent(N);

            for (var i = 0; i < M; i++)
            {
                var UV = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
                var (u, v) = (UV[0], UV[1]);
                G.AddEdge(u, v);
            }

            var scc = G.GetGraph();
            Console.WriteLine(scc.Count());
            foreach (var v in scc)
                Console.WriteLine($"{v.Count()} {string.Join(" ", v)}");
        }
    }
}