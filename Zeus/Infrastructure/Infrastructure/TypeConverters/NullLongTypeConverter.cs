namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable long.
    /// </summary>
    public static class NullLongTypeConverter
    {

        /// <summary>
        /// Convert a string to a nullable long.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable long.</returns>
        public static long? Convert(string source) {
            if (source == null) {
                return null;
            }

            long result;
            return long.TryParse(source, out result) ? (long?)result : null;
        }
    } 
}
