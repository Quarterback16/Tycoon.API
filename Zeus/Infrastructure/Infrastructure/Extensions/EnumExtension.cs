using System;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Enum"/>.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Get the description of an enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The enum description.</returns>
        public static string GetDescription(this Enum value)
        {
            return value.GetType().GetEnumDescription(value);
        }
    }
}
