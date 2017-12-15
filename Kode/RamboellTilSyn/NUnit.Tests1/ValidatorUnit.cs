using NUnit.Framework;

namespace NUnit.Tests1
{
    [TestFixture]
    public class ValidatorUnit
    {
        [TestCase("Ao")]
        [TestCase("Asdsads")]
        public void NameIsValid_Is_Valid(string s)
        {
            Assert.IsTrue(Ramboell.iOS.Validator.NameIsValid(s));
        }
        [TestCase("o")]
        [TestCase("oo")]
        [TestCase("oo1")]
        [TestCase("#¤%&")]
        [TestCase("Ao1")]
        [TestCase("Ao#1")]
        [TestCase("#1")]
        [TestCase("A!")]
        [TestCase("")]
        public void NameIsValid_Is_Not_Valid(string s)
        {
            Assert.IsFalse(Ramboell.iOS.Validator.NameIsValid(s));
        }
        [TestCase("liao@live.dk")]
        [TestCase("s@live.sccd")]
        [TestCase("2314@live.dk")]
        [TestCase("s@123.sd")]
        [TestCase("s.d@123.sd")]
        [TestCase("li@oo.dk")]
        public void EmailIsValid_Is_Valid(string s)
        {
            Assert.IsTrue(Ramboell.iOS.Validator.EmailIsValid(s));
        }
        [TestCase(".d@123.sd")]
        [TestCase("s.@123.sd")]
        [TestCase("s@live.s")]
        [TestCase("s@s.123")]
        [TestCase("s@s.shj")]
        [TestCase("s@s.s")]
        [TestCase("")]
        [TestCase("s@s.")]
        [TestCase("s@s")]
        [TestCase("s@")]
        [TestCase("sd")]
        [TestCase("@")]
        [TestCase("s.sd")]
        [TestCase("s@123.sd   ")]
        [TestCase("s@123   .sd")]
        [TestCase("s    @123.sd")]
        [TestCase("s@s.s")]
        public void EmailIsValid_Is_Not_Valid(string s)
        {
            Assert.IsFalse(Ramboell.iOS.Validator.EmailIsValid(s));
        }

        [TestCase("Aa2345")]
        [TestCase("Abc123")]
        [TestCase("Aa#345")]
        public void PasswordIsValid_Is_Valid(string s)
        {
            Assert.IsTrue(Ramboell.iOS.Validator.PasswordIsValid(s));
        }
        [TestCase("aa2345")]
        [TestCase("123456")]
        [TestCase("!\"#¤%&/()=")]
        [TestCase("")]
        [TestCase("sdsdadsaf")]
        [TestCase("12345")]
        [TestCase("RTYVFUGHBNJK")]
        [TestCase("DXFG")]
        [TestCase("¤%&fbf83bgc")]
        [TestCase("Aa#345dsavshw543I/T  VIYI")]
        public void PasswordIsValid_Is_Not_Valid(string s)
        {
            Assert.IsFalse(Ramboell.iOS.Validator.PasswordIsValid(s));
        }
    }
}
