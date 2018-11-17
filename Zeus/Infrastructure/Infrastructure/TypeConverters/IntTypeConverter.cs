namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to an int.
    /// </summary>
    public static class IntTypeConverter
    {
        /// <summary>
        /// Convert a string to an int.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as an int.</returns>
        public static int Convert(string source) {
            int result;
            return int.TryParse(source, out result) ? result : 0;
        }
    } 
}
