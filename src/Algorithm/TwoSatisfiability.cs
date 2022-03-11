using System;
using System.Linq;

namespace Algorithm
{
    public class TwoSatisfiability
    {
        public int Length { get; }

        private readonly StronglyConnectedComponent _scc;

        public TwoSatisfiability(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            Answer = new bool[length];
            _scc = new StronglyConnectedComponent(length * 2);
        }

        public bool[] Answer { get; }

        public void AddClause(int i, bool f, int j, bool g)
        {
            if (i < 0 || Length < i) throw new ArgumentOutOfRangeException(nameof(i));
            if (j < 0 || Length < j) throw new ArgumentOutOfRangeException(nameof(j));
            _scc.AddEdge(i * 2 + (f ? 0 : 1), j * 2 + (g ? 1 : 0));
            _scc.AddEdge(j * 2 + (g ? 0 : 1), i * 2 + (f ? 1 : 0));
        }

        public bool IsSatisfiable()
        {
            var ids = _scc.GetIDs().IDs;
            for (var i = 0; i < Length; i++)
            {
                if (ids[i * 2] == ids[i * 2 + 1]) return false;
                Answer[i] = ids[i * 2] < ids[i * 2 + 1];
            }

            return true;
        }
    }
}