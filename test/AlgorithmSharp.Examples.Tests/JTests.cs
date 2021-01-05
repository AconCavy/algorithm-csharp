using NUnit.Framework;

namespace AlgorithmSharp.Examples.Tests
{
    public class JTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"5 5
1 2 3 2 1
2 1 5
3 2 3
1 3 1
2 2 4
3 1 3";
            const string output = @"3
3
2
6";
            Tester.InOutTest(J.Solve, input, output);
        }
    }
}