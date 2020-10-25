using System;
using System.Linq;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class StringAlgorithmTests
    {
        [Test]
        public void InitializeTest()
        {
            Assert.DoesNotThrow(() => StringAlgorithm.CreateSuffixes(""));
            Assert.DoesNotThrow(() => StringAlgorithm.CreateSuffixes(new int[0]));
            Assert.DoesNotThrow(() => StringAlgorithm.ZAlgorithm(""));
            Assert.DoesNotThrow(() => StringAlgorithm.ZAlgorithm(new int[0]));
        }

        [Test]
        public void EmptyTest()
        {
            Assert.That(StringAlgorithm.CreateSuffixes("").Count(), Is.Zero);
            Assert.That(StringAlgorithm.CreateSuffixes(new int[0]).Count(), Is.Zero);

            Assert.That(StringAlgorithm.ZAlgorithm("").Count(), Is.Zero);
            Assert.That(StringAlgorithm.ZAlgorithm(new int[0]).Count(), Is.Zero);
        }

        [Test]
        public void SuffixesTest()
        {
            const string str = "missisippi";
            var sa = StringAlgorithm.CreateSuffixes(str).ToArray();
            var answer = new[]
            {
                "i", // 9
                "ippi", // 6
                "isippi", // 4
                "issisippi", // 1
                "missisippi", // 0
                "pi", // 8
                "ppi", // 7
                "sippi", // 5
                "sisippi", // 3
                "ssisippi", // 2
            };

            Assert.That(sa.Length, Is.EqualTo(answer.Length));
            for (var i = 0; i < sa.Length; i++)
                Assert.That(str.Substring(sa[i]), Is.EqualTo(answer[i]));
        }

        [Test]
        public void LongestCommonPrefixesTest()
        {
            const string str = "aab";
            var sa = StringAlgorithm.CreateSuffixes(str).ToArray();
            Assert.That(sa, Is.EqualTo(new[] {0, 1, 2}));
            var lcp = StringAlgorithm.CreateLongestCommonPrefixes(str, sa).ToArray();
            Assert.That(lcp, Is.EqualTo(new[] {1, 0}));

            Assert.That(StringAlgorithm.CreateLongestCommonPrefixes(new[] {0, 0, 1}, sa), Is.EqualTo(lcp));
            Assert.That(StringAlgorithm.CreateLongestCommonPrefixes(new[] {-100, -100, 100}, sa), Is.EqualTo(lcp));
            Assert.That(StringAlgorithm.CreateLongestCommonPrefixes(new[] {int.MinValue, int.MinValue, 100}, sa),
                Is.EqualTo(lcp));
        }

        [Test]
        public void ZAlgorithmTest()
        {
            Assert.That(StringAlgorithm.ZAlgorithm("abab"), Is.EqualTo(new[] {4, 0, 2, 0}));
            Assert.That(StringAlgorithm.ZAlgorithm(new[] {1, 10, 1, 10}), Is.EqualTo(new[] {4, 0, 2, 0}));
            Assert.That(StringAlgorithm.ZAlgorithm(new[] {0, 0, 0, 0, 0, 0, 0}),
                Is.EqualTo(ZAlgorithmNaive(new[] {0, 0, 0, 0, 0, 0, 0})));
        }

        [Test]
        public void SuffixesAllTest([Range(1, 100)] int n)
        {
            var s = Enumerable.Repeat(10, n).ToArray();
            Assert.That(StringAlgorithm.CreateSuffixes(s), Is.EqualTo(CreateSuffixesNaive(s)));
            Assert.That(StringAlgorithm.CreateSuffixes(s, 10), Is.EqualTo(CreateSuffixesNaive(s)));
            Assert.That(StringAlgorithm.CreateSuffixes(s, 12), Is.EqualTo(CreateSuffixesNaive(s)));

            s = new int[n];
            for (var i = 0; i < n; i++) s[i] = i % 2;
            Assert.That(StringAlgorithm.CreateSuffixes(s), Is.EqualTo(CreateSuffixesNaive(s)));
            Assert.That(StringAlgorithm.CreateSuffixes(s, 3), Is.EqualTo(CreateSuffixesNaive(s)));

            s = new int[n];
            for (var i = 0; i < n; i++) s[i] = 1 - i % 2;
            Assert.That(StringAlgorithm.CreateSuffixes(s), Is.EqualTo(CreateSuffixesNaive(s)));
            Assert.That(StringAlgorithm.CreateSuffixes(s, 3), Is.EqualTo(CreateSuffixesNaive(s)));
        }

        [Test]
        public void SaLcpNaiveTest([Values(4, 2)] int x)
        {
            for (var n = 1; n <= 20 / x; n++)
            {
                var m = 1;
                for (var i = 0; i < n; i++) m *= x;
                for (var f = 0; f < m; f++)
                {
                    var s = new int[n];
                    var g = f;
                    var maxC = 0;
                    for (var i = 0; i < n; i++)
                    {
                        s[i] = g % x;
                        maxC = System.Math.Max(maxC, s[i]);
                        g /= x;
                    }

                    var sa = CreateSuffixesNaive(s);
                    Assert.That(StringAlgorithm.CreateSuffixes(s), Is.EqualTo(sa));
                    Assert.That(StringAlgorithm.CreateSuffixes(s, maxC), Is.EqualTo(sa));
                    Assert.That(StringAlgorithm.CreateLongestCommonPrefixes(s, sa),
                        Is.EqualTo(CreateLongestCommonPrefixesNaive(s, sa)));
                }
            }
        }

        [Test]
        public void ZAlgorithmNaiveTest([Values(4, 2)] int x)
        {
            for (var n = 1; n <= 20 / x; n++)
            {
                var m = 1;
                for (var i = 0; i < n; i++) m *= x;
                for (var f = 0; f < m; f++)
                {
                    var s = new int[n];
                    var g = f;
                    for (var i = 0; i < n; i++)
                    {
                        s[i] = g % x;
                        g /= x;
                    }

                    Assert.That(StringAlgorithm.ZAlgorithm(s), Is.EqualTo(ZAlgorithmNaive(s)));
                }
            }
        }

        [Test]
        public void InvalidArgumentsTest()
        {
            Assert.Throws<ArgumentException>(() => StringAlgorithm.CreateSuffixes(new[] {0, 1}, -1));
            Assert.Throws<ArgumentException>(() => StringAlgorithm.CreateSuffixes(new[] {-1, 1}, 10));
            Assert.Throws<ArgumentException>(() => StringAlgorithm.CreateSuffixes(new[] {2, 2}, 1));

            Assert.Throws<ArgumentException>(
                () => StringAlgorithm.CreateLongestCommonPrefixes(new int[0], new[] {1, 2}));
        }

        private static int[] CreateSuffixesNaive(int[] s)
        {
            var n = s.Length;
            var sa = Enumerable.Range(0, n).ToArray();
            Array.Sort(sa, (l, r) =>
            {
                if (l == r) return 0;
                while (l < n && r < n)
                {
                    if (s[l] != s[r]) return s[l].CompareTo(s[r]);
                    l++;
                    r++;
                }

                return r.CompareTo(l);
            });
            return sa;
        }

        private static int[] CreateLongestCommonPrefixesNaive(int[] s, int[] sa)
        {
            var n = s.Length;
            var lcp = new int[n - 1];
            for (var i = 0; i < n - 1; i++)
            {
                var (l, r) = (sa[i], sa[i + 1]);
                while (l + lcp[i] < n && r + lcp[i] < n && s[l + lcp[i]] == s[r + lcp[i]]) lcp[i]++;
            }

            return lcp;
        }

        private static int[] ZAlgorithmNaive(int[] s)
        {
            var z = new int[s.Length];
            for (var i = 0; i < s.Length; i++)
                while (i + z[i] < s.Length && s[z[i]] == s[i + z[i]])
                    z[i]++;

            return z;
        }
    }
}