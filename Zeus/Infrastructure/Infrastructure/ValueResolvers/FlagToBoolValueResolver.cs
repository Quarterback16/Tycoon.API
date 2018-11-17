using System;
using System.Globalization;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a bool based on whether the flag is "Y" or "N".
    /// </summary>
    public static  class FlagToBoolValueResolver 
    {

        /// <summary>
        /// Resolves whether the bool is true or false.
        /// </summary>
        /// <returns><c>true</c> if the flag is "Y"; otherwise, <c>false</c>.</returns>
        public static bool Resolve(string source)
        {
            return string.Equals(source, "Y", StringComparison.OrdinalIgnoreCase);
        }
    } 
}


