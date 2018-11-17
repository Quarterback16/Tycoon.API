namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a decimal.
    /// </summary>
    public static class DecimalTypeConverter
    {
        /// <summary>
        /// Convert a string to a decimal.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a decimal.</returns>
        public static decimal Convert(string source) {
            decimal result;
            return decimal.TryParse(source, out result) ? result : 0;
        }

    } 
}
