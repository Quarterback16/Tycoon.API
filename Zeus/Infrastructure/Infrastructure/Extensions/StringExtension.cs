using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="string"/>.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Convert from a query string value to a dictionary of key value pairs.
        /// </summary>
        /// <param name="queryString">The query string value.</param>
        /// <returns>The query string as a dictionary.</returns>
        public static IDictionary<string, object> FromQueryStringToDictionary(this string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return null;
            }

            var nameValues = HttpUtility.ParseQueryString(queryString);

            return nameValues.AllKeys.ToDictionary(k => k, k => (object)nameValues[k]);
        }

        /// <summary>
        /// Make the first letter in a string uppercase.
        /// </summary>
        /// <param name="s">The string to make the first letter uppercase</param>
        /// <returns>The string with the first letter made uppercase.</returns>
        public static string ToUpperFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }
    }
}
