using System;

namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a date time.
    /// </summary>
    public static class DateTimeTypeConverter
    {


        /// <summary>
        /// Convert a string to a date time.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a date time.</returns>
        public static DateTime Convert(string source) {
            DateTime result;
            return DateTime.TryParse(source, out result) ? result : DateTime.MinValue;
        }
    } 
}
