using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Employment.Web.Mvc.Infrastructure.Interfaces.IRegistration" />.
    /// </summary>
    public static class RegistrationExtension
    {
        /// <summary>
        /// Gets the registration order based on position in sequence in <seealso cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.OrderAttribute" />.
        /// </summary>
        /// <param name="registration">The registration object.</param>
        /// <returns>The position in sequence.</returns>
        public static int Order(this IRegistration registration)
        {
            var orderAttribute = registration.GetType().GetAttribute<OrderAttribute>();

            if (orderAttribute == null)
            {
                orderAttribute = new OrderAttribute();
            }

            return orderAttribute.PositionInSequence;
        }

        /// <summary>
        /// Gets the registration group based on first in sequence in <seealso cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.OrderAttribute" />.
        /// </summary>
        /// <param name="registration">The registration object.</param>
        /// <returns>The group.</returns>
        public static int Group(this IRegistration registration)
        {
            var orderAttribute = registration.GetType().GetAttribute<OrderAttribute>();

            if (orderAttribute == null)
            {
                orderAttribute = new OrderAttribute();
            }

            // Invert bool and convert to int (true = 0, false = 1)
            return Convert.ToInt32(!orderAttribute.FirstInSequence);
        }
    }
}
