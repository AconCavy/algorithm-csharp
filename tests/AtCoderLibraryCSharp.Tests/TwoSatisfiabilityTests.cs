using NUnit.Framework;

namespace AtCoderLibraryCSharp.Tests
{
    public class TwoSatisfiabilityTests
    {
        [Test]
        public void EmptyTest()
        {
            var ts = new TwoSatisfiability();
            Assert.That(new bool[] { }, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void One1Test()
        {
            var ts = new TwoSatisfiability(1);
            ts.AddClause(0, true, 0, true);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.False);
        }

        [Test]
        public void One2Test()
        {
            var ts = new TwoSatisfiability(1);
            ts.AddClause(0, true, 0, true);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] {true}, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void One3Test()
        {
            var ts = new TwoSatisfiability(1);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] {false}, Is.EqualTo(ts.Answer));
        }
    }
}