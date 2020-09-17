using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public class ITests
    {
        [Test]
        public void Test1()
        {
            const string input = @"abcbcba";
            const string output = @"21";
            Tester.InOutTest(I.Solve, input, output);
        }

        [Test]
        public void Test2()
        {
            const string input = @"mississippi";
            const string output = @"53";
            Tester.InOutTest(I.Solve, input, output);
        }

        [Test]
        public void Test3()
        {
            const string input = @"ababacaca";
            const string output = @"33";
            Tester.InOutTest(I.Solve, input, output);
        }

        [Test]
        public void Test4()
        {
            const string input = @"aaaaa";
            const string output = @"5";
            Tester.InOutTest(I.Solve, input, output);
        }
    }
}