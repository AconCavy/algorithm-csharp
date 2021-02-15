using NUnit.Framework;

namespace AlgorithmSharp.Examples.Tests
{
    public class BTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"5 5
1 2 3 4 5
1 0 5
1 2 4
0 3 10
1 0 5
1 0 3";
            const string output = @"15
7
25
6";
            Tester.InOutTest(B.Solve, input, output);
        }
    }
}