namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a null to an empty string.
    /// </summary>
    public static class NullStringTypeConverter
    {
        /// <summary>
        /// Convert a null string to empty
        /// </summary>
        /// <param name="source"></param>
        /// <returns>source string or string empty if source is null</returns>
        public static string Convert(string source) {
            return source ?? string.Empty;
        }
    } 
}
