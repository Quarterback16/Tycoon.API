
namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a flag of "Y", "N" or empty based on whether the nullable bool is true, false or null.
    /// </summary>
    public static class NullableBoolToFlagValueResolver 
    {

        /// <summary>
        /// Resolves whether the flag is "Y", "N" or empty.
        /// </summary>
        /// <returns>"Y" if the nullable bool is <c>true</c>, "N" if the nullable bool is <c>false</c>; otherwise, empty.</returns>
        public static string Resolve(bool? source) {
            if (!source.HasValue) {
                return string.Empty;
            }

            return source.Value ? "Y" : "N";
        }
    }
}


