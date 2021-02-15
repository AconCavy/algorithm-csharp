using NUnit.Framework;

namespace AlgorithmSharp.Examples.Tests
{
    public class ETests
    {
        [Test]
        public void Test1()
        {
            const string input = @"3 1
5 3 2
1 4 8
7 6 9";
            const string output = @"19
X..
..X
.X.";
            Tester.InOutTest(E.Solve, input, output);
        }

        [Test]
        public void Test2()
        {
            const string input = @"3 2
10 10 1
10 10 1
1 1 10";
            const string output = @"50
XX.
XX.
..X";
            Tester.InOutTest(E.Solve, input, output);
        }
    }
}