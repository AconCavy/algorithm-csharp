using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class LTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"5 5
0 1 0 0 1
2 1 5
1 3 4
2 2 5
1 1 3
2 1 2";
            const string output = @"2
0
1";
            Tester.InOutTest(L.Solve, input, output);
        }
    }
}