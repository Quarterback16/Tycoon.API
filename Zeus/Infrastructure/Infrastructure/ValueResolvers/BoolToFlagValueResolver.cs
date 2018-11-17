namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a flag of "Y" or "N" based on whether the bool is true or false.
    /// </summary>
    public static class BoolToFlagValueResolver 
    {
        /// <summary>
        /// Resolves whether the flag is "Y" or "N".
        /// </summary>
        /// <returns>"Y" if the bool is <c>true</c>; otherwise, "N".</returns>
        public static string Resolve(bool source) {
            return source ? "Y" : "N";
        }
    } 
}


