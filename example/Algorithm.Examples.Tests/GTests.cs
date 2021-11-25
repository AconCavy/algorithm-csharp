using NUnit.Framework;

namespace Algorithm.Examples.Tests
{
    public class GTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"6 7
1 4
5 2
3 0
5 5
4 1
0 3
4 2";
            const string output = @"4
1 5
2 1 4
1 2
2 0 3";
            Tester.InOutTest(G.Solve, input, output);
        }
    }
}