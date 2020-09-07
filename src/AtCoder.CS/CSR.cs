using System.Collections.Generic;
using System.Linq;

namespace AtCoder.CS
{
    public class CSR<T>
    {
        public int[] Start { get; }

        public T[] Edges { get; }

        public CSR(int n, IEnumerable<(int, T)> edges)
        {
            Start = new int[n + 1];
            var tuples = edges as (int, T)[] ?? edges.ToArray();
            Edges = new T[tuples.Length];
            foreach (var e in tuples)
            {
                Start[e.Item1 + 1]++;
            }

            for (var i = 0; i < n; i++)
            {
                Start[i + 1] += Start[i];
            }

            foreach (var (i, t) in tuples)
            {
                Edges[Start[i]++] = t;
            }
        }
    }
}