using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Surgicalogic.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex EmailRegex = new Regex(@"^[a-z][a-z0-9\.\-_]*[a-z0-9]+@[a-z][a-z0-9\.\-]*\.[a-z0-9\.\-]+$");
        private static readonly Regex LandPhoneNumberRegex = new Regex("^[1-9][0-9]{9}$");
        private static readonly Regex CellPhoneNumberRegex = new Regex("^5(0|3|4|5)[0-9]{8}$");

        public static string FormatX(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool IsEmail(this string value)
        {
            return !value.IsNullOrEmpty() && EmailRegex.IsMatch(value.ToLowerInvariant());
        }

        public static bool IsLandPhoneNumber(this string value)
        {
            return !value.IsNullOrEmpty() && LandPhoneNumberRegex.IsMatch(value);
        }

        public static bool IsCellPhoneNumber(this string value)
        {
            return !value.IsNullOrEmpty() && CellPhoneNumberRegex.IsMatch(value);
        }

        /// <summary>
        /// String bir değeri integer'a çevirir, integer olamayacak bir değere sahipse hata fırlatmaz, sıfır döner.
        /// </summary>
        public static int ToNCInt(this string text)
        {
            int.TryParse(text, out int result);
            return result;
        }
    }
}