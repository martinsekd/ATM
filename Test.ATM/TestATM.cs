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
    }
}
