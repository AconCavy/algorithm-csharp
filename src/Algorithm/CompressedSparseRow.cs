using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
    public class CompressedSparseRow<T>
    {
        public CompressedSparseRow(int length, IEnumerable<(int, T)> edges)
        {
            Start = new int[length + 1];
            var es = edges.ToArray();
            Edges = new T[es.Length];
            foreach (var e in es) Start[e.Item1 + 1]++;
            for (var i = 0; i < length; i++) Start[i + 1] += Start[i];
            var counter = Start.ToArray();
            foreach (var (i, t) in es) Edges[counter[i]++] = t;
        }

        public int[] Start { get; }
        public T[] Edges { get; }
    }
}