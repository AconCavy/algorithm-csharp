using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace AtCoderLibraryCSharp.Examples.Tests
{
    public static class Tester
    {
        public static void InOutTest(Action action, string input, string output)
        {
            using var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            Console.SetOut(writer);

            action();

            using var sr = new StringReader(builder.ToString());
            var actual = sr.ReadToEnd().Replace(Environment.NewLine, "");
            var expected = output.Replace(Environment.NewLine, "");
            Assert.That(actual, Is.EqualTo(actual));
        }
    }
}