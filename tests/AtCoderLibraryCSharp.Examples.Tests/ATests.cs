using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class ATests
    {
        [Test]
        public void Test1()
        {
            const string input = @"4 7
1 0 1
0 0 1
0 2 3
1 0 1
1 1 2
0 0 2
1 1 3";
            const string output = @"0
1
0
1";
            Tester.InOutTest(A.Solve, input, output);
        }
    }
}