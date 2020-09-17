using System;
using System.Linq;

namespace AtCoderLibraryCSharp.Examples
{
    public static class F
    {
        public static void Solve()
        {
            var NM = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            var (N, M) = (NM[0], NM[1]);
            var A = Console.ReadLine().Split(" ").Select(long.Parse)
                .Select(x => new ModuloInteger(x)).ToArray();
            var B = Console.ReadLine().Split(" ").Select(long.Parse)
                .Select(x => new ModuloInteger(x)).ToArray();
            var c = Convolution.Execute(A, B);
            
            Console.WriteLine(string.Join(" ", c));
        }
    }
}