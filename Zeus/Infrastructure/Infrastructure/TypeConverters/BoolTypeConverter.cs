namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a bool.
    /// </summary>
    public static class BoolTypeConverter
    {
        /// <summary>
        /// Convert a string to a bool.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a bool.</returns>
        public static bool Convert(string source)
        {
            bool result;
            return bool.TryParse(source, out result) ? result : false;
        }
    } 
}
