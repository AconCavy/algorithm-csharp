using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class KTests
    {
        [Test]
        public void Test1()
        {
            const string input = @"5 7
1 2 3 4 5
1 0 5
0 2 4 100 101
1 0 3
0 1 3 102 103
1 2 5
0 2 5 104 105
1 0 5";
            const string output = @"15
404
41511
4317767";
            Tester.InOutTest(K.Solve, input, output);
        }
    }
}