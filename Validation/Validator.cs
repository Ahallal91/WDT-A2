using System;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// Validator class contains static methods for general validation.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// FourDigits validates 4 digit integers
        /// </summary>
        /// <param name="value">integer to validate</param>
        /// <returns>True if validation is successful. Otherwise false.</returns>
        public static bool FourDigits(int value)
        {
            Regex Rgx = new Regex(@"\d{4}");
            return Rgx.IsMatch(value.ToString());
        }
        /// <summary>
        /// EightDigits validates 8 digit strings
        /// </summary>
        /// <param name="value">string to validate</param>
        /// <returns>True if validation is successful. Otherwise false.</returns>
        public static bool EightDigits(string value)
        {
            Regex Rgx = new Regex(@"\d{4}");
            return Rgx.IsMatch(value);
        }
        /// <summary>
        /// NullEmptyValue validates strings
        /// </summary>
        /// <param name="value">integer to validate</param>
        /// <returns>True if string is null or empty, otherwise false.</returns>
        public static bool NullEmptyValue(string value)
        {
            return (value == null) || (value == "");
        }
    }
}
