using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="DateTime" />.
    /// </summary>
    public static class DateTimeExtension
    {
        private static IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Get the age in years from the date of birth to the system date (defaulted to <see cref="IUserService.DateTime" />).
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The age in years.</returns>
        public static int GetAge(this DateTime dateOfBirth)
        {
            return dateOfBirth.GetAge(UserService.DateTime);
        }

        /// <summary>
        /// Get the age in years from the date of birth to the system date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="overrideSystemDate">Override the system date.</param>
        /// <returns>The age in years.</returns>
        public static int GetAge(this DateTime dateOfBirth, DateTime overrideSystemDate)
        {
            return overrideSystemDate.Year - dateOfBirth.Year - ((overrideSystemDate.Month < dateOfBirth.Month || (overrideSystemDate.Month == dateOfBirth.Month && overrideSystemDate.Day < dateOfBirth.Day)) ? 1 : 0);
        }

        /// <summary>
        /// Get the date and time converted to a javascript timestamp.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns>The date and time converted to a javascript timestamp.</returns>
        public static long ToJavascriptTimestamp(this DateTime dateTime)
        {
            return dateTime.Subtract(new TimeSpan(DateTime.Parse("1/1/1970").Ticks)).Ticks / 10000;
        }
    }
}
