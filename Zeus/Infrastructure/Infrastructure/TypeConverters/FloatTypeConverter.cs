namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a float.
    /// </summary>
    public static class FloatTypeConverter
    {
        /// <summary>
        /// Convert a string to a float.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a float.</returns>
        public static float Convert(string source) {
            float result;
            return float.TryParse(source, out result) ? result : 0;
        }
    } 
}
