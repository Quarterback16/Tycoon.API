namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a string to a nullable int.
    /// </summary>
    public static class NullIntTypeConverter
    {


        /// <summary>
        /// Convert a string to a nullable int.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Source string as a nullable int.</returns>
        public static int? Convert(string source) {
            if (source == null) {
                return null;
            }

            int result;
            return int.TryParse(source, out result) ? (int?)result : null;
        }
    } 
}
