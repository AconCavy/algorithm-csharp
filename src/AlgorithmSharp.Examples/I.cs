using System;
using System.Linq;

namespace AlgorithmSharp.Examples
{
    public static class I
    {
        public static void Solve()
        {
            var S = Console.ReadLine();
            var sa = StringAlgorithm.CreateSuffixes(S);
            var answer = (long)S.Length * (S.Length + 1) / 2;
            answer = StringAlgorithm.CreateLongestCommonPrefixes(S, sa)
                .Aggregate(answer, (sum, x) => sum - x);

            Console.WriteLine(answer);
        }
    }
}