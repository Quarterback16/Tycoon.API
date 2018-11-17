namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable bool.
    /// </summary>
    public static class NullBoolTypeConverter
    {
        /// <summary>
        /// Convert a string to a nullable bool.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable bool.</returns>
        public static bool? Convert(string source) {
            if (source == null) {
                return null;
            }

            bool result;
            return bool.TryParse(source, out result) ? (bool?)result : null;
        }
    } 
}
