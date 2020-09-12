using NUnit.Framework;

namespace AtCoder.CS.Tests
{
    public class TwoSatTests
    {
        [Test]
        public void EmptyTest()
        {
            var ts = new TwoSat();
            Assert.That(new bool[] { }, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void One1Test()
        {
            var ts = new TwoSat(1);
            ts.AddClause(0, true, 0, true);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.False);
        }

        [Test]
        public void One2Test()
        {
            var ts = new TwoSat(1);
            ts.AddClause(0, true, 0, true);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] {true}, Is.EqualTo(ts.Answer));
        }

        [Test]
        public void One3Test()
        {
            var ts = new TwoSat(1);
            ts.AddClause(0, false, 0, false);
            Assert.That(ts.IsSatisfiable(), Is.True);
            Assert.That(new[] {false}, Is.EqualTo(ts.Answer));
        }
    }
}