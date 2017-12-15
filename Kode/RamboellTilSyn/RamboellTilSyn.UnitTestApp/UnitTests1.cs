using System;
using NUnit.Framework;

namespace RamboellTilSyn.UnitTestApp
{
    [TestFixture]
    public class UnitTests1
    {
        [Test]
        public void Pass()
        {
            Assert.True(true);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }
    }
}