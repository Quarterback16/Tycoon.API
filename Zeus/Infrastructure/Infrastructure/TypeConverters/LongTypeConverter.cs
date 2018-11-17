namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a long.
    /// </summary>
    public static class LongTypeConverter
    {

        /// <summary>
        /// Convert a string to a long.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a long.</returns>
        public static long Convert(string source) {
            long result;
            return long.TryParse(source, out result) ? result : 0;
        }
    } 
}
