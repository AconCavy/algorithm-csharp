using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class CTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"5
4 10 6 3
6 5 4 3
1 1 0 0
31415 92653 58979 32384
1000000000 1000000000 999999999 999999999";
            const string output = @"3
13
0
314095480
499999999500000000";
            Tester.InOutTest(C.Solve, input, output);
        }
    }
}