namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable float.
    /// </summary>
    public static class NullFloatTypeConverter
    {
        /// <summary>
        /// Convert a string to a nullable float.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable float.</returns>
        public static float? Convert(string source) {
            if (source == null) {
                return null;
            }

            float result;
            return float.TryParse(source, out result) ? (float?)result : null;
        }
    } 
}
