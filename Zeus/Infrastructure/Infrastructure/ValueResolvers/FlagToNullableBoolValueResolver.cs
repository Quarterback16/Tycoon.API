using System;
using System.Globalization;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a nullable bool based on whether the flag is "Y", "N" or empty.
    /// </summary>
    public static class FlagToNullableBoolValueResolver 
    {

        /// <summary>
        /// Resolves whether the nullable bool is true, false or null.
        /// </summary>
        /// <returns><c>true</c> if the flag is "Y", <c>false</c> if the flag is "N"; otherwise, <c>null</c>.</returns>
        public static bool? Resolve(string source) {
            if (string.IsNullOrEmpty(source)) {
                return null;
            }

            return string.Equals(source, "Y", StringComparison.OrdinalIgnoreCase);
        }
    } 
}


