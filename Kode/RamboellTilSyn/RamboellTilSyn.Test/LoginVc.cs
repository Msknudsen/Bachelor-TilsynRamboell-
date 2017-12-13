using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ramboell.iOS;

namespace RamboellTilSyn.Test
{
    [TestClass]
    public class LoginVC
    {
        [TestCase(12, 4, 3)]

        public void TestMethod1(string a)
        {
            Assert.IsTrue(Validator.EmailIsValid(a));
        }
    }
}
