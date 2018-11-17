namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable decimal.
    /// </summary>
    public static class NullDecimalTypeConverter
    {
        /// <summary>
        /// Convert a string to a nullable decimal.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable decimal.</returns>
        public static decimal? Convert(string source) {
            if (source == null) {
                return null;
            }

            decimal result;
            return decimal.TryParse(source, out result) ? (decimal?)result : null;
        }
    } 
}
