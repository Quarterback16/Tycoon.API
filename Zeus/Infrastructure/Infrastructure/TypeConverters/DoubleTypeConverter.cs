namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a double.
    /// </summary>
    public static class DoubleTypeConverter
    {
        /// <summary>
        /// Convert a string to a double.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a double.</returns>
        public static double Convert(string source) {
            double result;
            return double.TryParse(source, out result) ? result : 0;
        }
    } 
}
