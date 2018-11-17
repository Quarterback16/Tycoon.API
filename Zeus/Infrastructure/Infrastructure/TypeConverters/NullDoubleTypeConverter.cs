namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable double.
    /// </summary>
    public static class NullDoubleTypeConverter
    {
        /// <summary>
        /// Convert a string to a nullable double.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable double.</returns>
        public static double? Convert(string source) {
            if (source == null) {
                return null;
            }

            double result;
            return double.TryParse(source, out result) ? (double?)result : null;
        }
    } 
}
