using System.Text.RegularExpressions;
using ARKit;

namespace Ramboell.iOS
{
    /// <summary>
    /// This class is used when there is a need for validation of user input
    /// </summary>
    /// <returns></returns>
    public static class Validator
    {
        static readonly System.Text.RegularExpressions.Regex ValidEmailRegex = CreateValidEmailRegex();
        static readonly System.Text.RegularExpressions.Regex ValidNameRegex = CreateValidNameRegex();
        static readonly System.Text.RegularExpressions.Regex ValidPasswordRegex = CreateValidPasswordRegex();

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx 11/12-2017
        /// </summary>
        /// <returns> A regex pattern scheme for matching email strings</returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        ///  Creating a Regex matching pattern
        /// </summary>
        /// <returns> A regex pattern scheme for matching name</returns>
        private static Regex CreateValidNameRegex()
        {
            string validPattern = @"^[A-Z][a-zA-Z .,'-]*${2,30}";

            return new Regex(validPattern, RegexOptions.None);
        }
        /// <summary>
        ///  Creating a Regex matching pattern
        /// </summary>
        /// <returns> A regex pattern scheme for matching password</returns>
        private static Regex CreateValidPasswordRegex()
        {
            //https://stackoverflow.com/questions/19605150/regex-for-password-must-contain-at-least-eight-characters-at-least-one-number-a /11/12-2017
            //Rule: 1 Upper char, 1 number min 6 characters, optional symbols
            string validPasswordPattern = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{6,}$";

            return new Regex(validPasswordPattern, RegexOptions.None);
        }

        /// <summary>
        /// Validating the input param
        /// </summary>
        /// <param name="emailAddress">input string</param>
        /// <returns>true if input is valid email string, else false</returns>
        public static bool EmailIsValid(string emailAddress)
        {
            return ValidEmailRegex.IsMatch(emailAddress);
        }
        /// <summary>
        /// Validating the input param
        /// </summary>
        /// <param name="name">input string</param>
        /// <returns>true if input is valid name string, else false</returns>
        public static bool NameIsValid(string name)
        {
            return ValidNameRegex.IsMatch(name);
        }
        /// <summary>
        /// Validating the input param
        /// </summary>
        /// <param name="password">input string</param>
        /// <returns>true if input is valid password string, else false</returns>
        public static bool PasswordIsValid(string password)
        {
            return ValidPasswordRegex.IsMatch(password);
        }
    }
}
