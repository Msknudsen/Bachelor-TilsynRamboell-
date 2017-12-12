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
            string validPattern = "[a-zA-Z]";

            return new Regex(validPattern, RegexOptions.IgnoreCase);
        }

        internal static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
        internal static bool NameIsValid(string emailAddress)
        {
            bool isValid = ValidNameRegex.IsMatch(emailAddress);
            return isValid;
        }
    }
}
