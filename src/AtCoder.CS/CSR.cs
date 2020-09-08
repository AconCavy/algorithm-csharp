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
            var es = edges.ToArray();
            Edges = new T[es.Length];
            foreach (var e in es) Start[e.Item1 + 1]++;
            for (var i = 0; i < n; i++) Start[i + 1] += Start[i];
            var counter = Start.ToArray();
            foreach (var (i, t) in es) Edges[counter[i]++] = t;
        }
    }
}