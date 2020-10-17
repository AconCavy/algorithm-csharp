using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderLibraryCSharp
{
    public static class StringAlgorithm
    {
        public static IEnumerable<int> CreateSuffixes(string str)
        {
            return CreateSuffixesByInducedSorting(str.Select(x => x - 0).ToArray(), 255);
        }

        public static IEnumerable<int> CreateSuffixes<T>(IEnumerable<T> items)
        {
            var s = items.ToArray();
            var n = s.Length;
            var idx = Enumerable.Range(0, n).ToArray();
            Array.Sort(idx, (x, y) => Comparer<T>.Default.Compare(s[x], s[y]));
            var s2 = new int[n];
            var now = 0;
            for (var i = 0; i < n; i++)
            {
                if (i > 0 && !Equals(s[idx[i - 1]], s[idx[i]])) now++;
                s2[idx[i]] = now;
            }

            return CreateSuffixesByInducedSorting(s2, now);
        }

        public static IEnumerable<int> CreateSuffixes(IEnumerable<int> items, int upper)
        {
            if (upper < 0) throw new ArgumentException(nameof(upper));
            var s = items.ToArray();
            if (s.Any(x => x < 0 || upper < x)) throw new ArgumentException(nameof(items));
            return CreateSuffixesByInducedSorting(s, upper);
        }

        public static IEnumerable<int> CreateLongestCommonPrefixes(string str, IEnumerable<int> suffixArray)
        {
            return CreateLongestCommonPrefixes(str.Select(x => x - 0), suffixArray);
        }

        public static IEnumerable<int> CreateLongestCommonPrefixes<T>(IEnumerable<T> items,
            IEnumerable<int> suffixArray)
        {
            var s = items.ToArray();
            var sa = suffixArray.ToArray();
            var n = s.Length;
            if (n < 1) throw new ArgumentException(nameof(items));
            var rnk = new int[n];
            for (var i = 0; i < rnk.Length; i++) rnk[sa[i]] = i;
            var lcp = new int[n - 1];
            var h = 0;
            for (var i = 0; i < n; i++)
            {
                if (h > 0) h--;
                if (rnk[i] == 0) continue;
                var j = sa[rnk[i] - 1];
                while (j + h < n && i + h < n)
                {
                    if (!Equals(s[j + h], s[i + h])) break;
                    h++;
                }

                lcp[rnk[i] - 1] = h;
            }

            return lcp;
        }

        public static IEnumerable<int> ZAlgorithm(string str)
        {
            return ZAlgorithm(str.Select(x => x - 0));
        }

        public static IEnumerable<int> ZAlgorithm<T>(IEnumerable<T> items)
        {
            var s = items.ToArray();
            var n = s.Length;
            if (n == 0) return new List<int>();
            var z = new int[n];
            for (int i = 1, j = 0; i < n; i++)
            {
                z[i] = j + z[j] <= i ? 0 : System.Math.Min(j + z[j] - i, z[i - j]);
                while (i + z[i] < n && Equals(s[z[i]], s[i + z[i]])) z[i]++;
                if (j + z[j] < i + z[i]) j = i;
            }

            z[0] = n;
            return z;
        }

        private static IEnumerable<int> CreateSuffixesByInducedSorting(int[] items, int upper,
            int naive = 10, int doubling = 40)
        {
            var n = items.Length;
            switch (n)
            {
                case 0: return new int[0];
                case 1: return new[] {0};
                case 2: return items[0] < items[1] ? new[] {0, 1} : new[] {1, 0};
            }

            if (n < naive) return CreateSuffixesByNaive(items);
            if (n < doubling) return CreateSuffixesByDoubling(items);

            var sa = new int[n];
            var ls = new bool[n];
            for (var i = n - 2; i >= 0; i--)
            {
                ls[i] = items[i] == items[i + 1] ? ls[i + 1] : items[i] < items[i + 1];
            }

            var sumL = new int[upper + 1];
            var sumS = new int[upper + 1];
            for (var i = 0; i < n; i++)
            {
                if (!ls[i]) sumS[items[i]]++;
                else sumL[items[i] + 1]++;
            }

            for (var i = 0; i <= upper; i++)
            {
                sumS[i] += sumL[i];
                if (i < upper) sumL[i + 1] += sumS[i];
            }

            void Induce(IEnumerable<int> ilms)
            {
                Array.Fill(sa, -1);
                var buffer = new int[upper + 1];
                sumS.CopyTo(buffer, 0);
                foreach (var d in ilms)
                {
                    if (d == n) continue;
                    sa[buffer[items[d]]++] = d;
                }

                sumL.CopyTo(buffer, 0);
                sa[buffer[items[n - 1]]++] = n - 1;
                for (var i = 0; i < n; i++)
                {
                    var v = sa[i];
                    if (v >= 1 && !ls[v - 1]) sa[buffer[items[v - 1]]++] = v - 1;
                }

                sumL.CopyTo(buffer, 0);
                for (var i = n - 1; i >= 0; i--)
                {
                    var v = sa[i];
                    if (v >= 1 && ls[v - 1])
                    {
                        sa[--buffer[items[v - 1] + 1]] = v - 1;
                    }
                }
            }

            var lmsMap = new int[n + 1];
            Array.Fill(lmsMap, -1);
            var m = 0;
            for (var i = 1; i < n; i++)
            {
                if (!ls[i - 1] && ls[i]) lmsMap[i] = m++;
            }

            var lms = new List<int>();
            for (var i = 1; i < n; i++)
            {
                if (!ls[i - 1] && ls[i]) lms.Add(i);
            }

            Induce(lms);

            if (m <= 0) return sa;

            var sortedLms = sa.Where(x => lmsMap[x] != -1).ToArray();
            var recS = new int[m];
            var recUpper = 0;
            recS[lmsMap[sortedLms[0]]] = 0;
            for (var i = 1; i < m; i++)
            {
                var l = sortedLms[i - 1];
                var r = sortedLms[i];
                var el = lmsMap[l] + 1 < m ? lms[lmsMap[l] + 1] : n;
                var er = lmsMap[r] + 1 < m ? lms[lmsMap[r] + 1] : n;
                var isSame = true;
                if (el - l != er - r) isSame = false;
                else
                {
                    while (l < el && items[l] == items[r])
                    {
                        l++;
                        r++;
                    }

                    if (l == n || items[l] != items[r]) isSame = false;
                }

                if (!isSame) recUpper++;
                recS[lmsMap[sortedLms[i]]] = recUpper;
            }

            var recSa = CreateSuffixesByInducedSorting(recS, recUpper, naive, doubling).ToArray();
            for (var i = 0; i < m; i++)
            {
                sortedLms[i] = lms[recSa[i]];
            }

            Induce(sortedLms);
            return sa;
        }

        private static IEnumerable<int> CreateSuffixesByNaive(int[] items)
        {
            var n = items.Length;
            var sa = Enumerable.Range(0, n).ToArray();

            int Compare(int x, int y)
            {
                var comparer = Comparer<int>.Default;
                if (x == y) return 0;
                while (x < n && y < n)
                {
                    if (items[x] != items[y]) return comparer.Compare(items[x], items[y]);
                    x++;
                    y++;
                }

                return comparer.Compare(x, n);
            }

            Array.Sort(sa, Compare);
            return sa;
        }

        private static IEnumerable<int> CreateSuffixesByDoubling(int[] items)
        {
            var n = items.Length;
            var sa = Enumerable.Range(0, n).ToArray();

            var tmp = new int[n];
            for (var k = 1; k < n; k *= 2)
            {
                int Compare(int x, int y)
                {
                    var comparer = Comparer<int>.Default;
                    if (items[x] != items[y]) return comparer.Compare(items[x], items[y]);
                    var rx = x + k < n ? items[x + k] : -1;
                    var ry = y + k < n ? items[y + k] : -1;
                    return comparer.Compare(rx, ry);
                }

                Array.Sort(sa, Compare);
                tmp[sa[0]] = 0;
                for (var i = 1; i < n; i++)
                {
                    tmp[sa[i]] = tmp[sa[i - 1]] + (Compare(sa[i - 1], sa[i]) < 0 ? 1 : 0);
                }

                (tmp, items) = (items, tmp);
            }

            return sa;
        }
    }
}