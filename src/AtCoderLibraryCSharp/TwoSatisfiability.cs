using System;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public class TwoSatisfiability
    {
        public bool[] Answer { get; }

        private readonly int _n;
        private readonly StronglyConnectedComponent _scc;

        public TwoSatisfiability(int n = 0)
        {
            _n = n;
            Answer = new bool[n];
            _scc = new StronglyConnectedComponent(2 * n);
        }

        public void AddClause(int i, bool f, int j, bool g)
        {
            if (i < 0 || _n < i) throw new IndexOutOfRangeException(nameof(i));
            if (j < 0 || _n < j) throw new IndexOutOfRangeException(nameof(j));
            _scc.AddEdge(2 * i + (f ? 0 : 1), 2 * j + (g ? 1 : 0));
            _scc.AddEdge(2 * j + (g ? 0 : 1), 2 * i + (f ? 1 : 0));
        }

        public bool IsSatisfiable()
        {
            var (_, tmp) = _scc.GetIds();
            var ids = tmp.ToArray();
            for (var i = 0; i < _n; i++)
            {
                if (ids[2 * i] == ids[2 * i + 1]) return false;
                Answer[i] = ids[2 * i] < ids[2 * i + 1];
            }

            return true;
        }
    }
}