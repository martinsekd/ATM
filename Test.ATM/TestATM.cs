using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ATM
{
    [TestFixture]
    public class TestATM
    {
        [SetUp]
        public void setUp()
        {

        }

        [Test]
        public void something()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }

        [Test]
        public void something2()
        {
            test uut = new test();
            Assert.That(uut.metodeA(), Is.True);
        }

        [Test]
        public void something3()
        {
            test uut = new test();
            Assert.That(uut.metodeB(), Is.True);
        }
    }
}
