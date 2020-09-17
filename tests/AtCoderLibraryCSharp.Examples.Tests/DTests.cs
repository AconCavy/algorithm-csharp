using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class DTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"3 3
#..
..#
...";
            const string output = @"3
#><
><#
><.";
            Tester.InOutTest(D.Solve, input, output);
        }
    }
}