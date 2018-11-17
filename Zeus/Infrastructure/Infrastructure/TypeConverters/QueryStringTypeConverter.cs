using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Extensions;

namespace Employment.Web.Mvc.Infrastructure.TypeConverters
{
    /// <summary>
    /// Defines a converter that converts a query string to dictionary.
    /// </summary>
    public static class QueryStringTypeConverter 
    {
        /// <summary>
        /// Convert a query string to dictionary.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Query string as a dictionary.</returns>
        public static IDictionary<string, object> Convert(string source) {
            return source.FromQueryStringToDictionary();
        }
    } 
}
