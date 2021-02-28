using NUnit.Framework;

namespace Algorithm.Examples.Tests
{
    public class HTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"3 2
1 4
2 5
0 6";
            const string output = @"Yes
4
2
0";
            Tester.InOutTest(H.Solve, input, output);
        }

        [Test]
        public void Test2()
        {
            const string input = @"3 3
1 4
2 5
0 6";
            const string output = @"No";
            Tester.InOutTest(H.Solve, input, output);
        }
    }
}