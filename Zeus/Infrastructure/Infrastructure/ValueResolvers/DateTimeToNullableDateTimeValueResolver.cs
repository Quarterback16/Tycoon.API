using System;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a nullable DateTime from a DateTime, where <see cref="DateTime.MinValue" /> is treated as <c>null</c>.
    /// </summary>
    public static class DateTimeToNullableDateTimeValueResolver 
    {
        /// <summary>
        /// Resolves the DateTime as a nullable DateTime, where <see cref="DateTime.MinValue" /> is treated as <c>null</c>.
        /// </summary>
        /// <returns>Nullable DateTime where <see cref="DateTime.MinValue" /> is treated as <c>null</c>.</returns>
        public static DateTime? Resolve(DateTime source) {
            return source.Equals(DateTime.MinValue) ? (DateTime?)null : source;
        }
    } 
}


