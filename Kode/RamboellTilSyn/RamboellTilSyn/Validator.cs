using System.Text.RegularExpressions;

namespace Ramboell.iOS
{
    /// <summary>
    /// Taken from https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
    /// </summary>
    /// <returns></returns>
    public static class Validator
    {

        static readonly System.Text.RegularExpressions.Regex ValidEmailRegex = CreateValidEmailRegex();
        static readonly System.Text.RegularExpressions.Regex ValidNameRegex = CreateValidNameRegex();
        static readonly System.Text.RegularExpressions.Regex ValidPasswordRegex = CreateValidPasswordRegex();

  

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        private static Regex CreateValidNameRegex()
        {
            string validPattern = @"^[A-Z][a-zA-Z .,'-]*${2,30}";

            return new Regex(validPattern, RegexOptions.None);
        }
        private static Regex CreateValidPasswordRegex()
        {
            //https://stackoverflow.com/questions/19605150/regex-for-password-must-contain-at-least-eight-characters-at-least-one-number-a
            string validPasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
            return new Regex(validPasswordPattern, RegexOptions.None);
        }
        public static bool EmailIsValid(string emailAddress)
        {
            return ValidEmailRegex.IsMatch(emailAddress);
        }

        public static bool NameIsValid(string emailAddress)
        {
            return ValidNameRegex.IsMatch(emailAddress);
        }

        public static bool PasswordIsValid(string password)
        {
            return ValidPasswordRegex.IsMatch(password);
        }
    }
}
