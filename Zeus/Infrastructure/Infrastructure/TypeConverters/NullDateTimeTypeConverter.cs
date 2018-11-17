using System;

namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable date time.
    /// </summary>
    public static class NullDateTimeTypeConverter
    {
        /// <summary>
        /// Convert a string to a nullable date time.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable date time.</returns>
        public static DateTime? Convert(string source) {
            if (source == null) {
                return null;
            }

            DateTime result;
            return DateTime.TryParse(source, out result) ? (DateTime?)result : null;
        }
    } 
}
