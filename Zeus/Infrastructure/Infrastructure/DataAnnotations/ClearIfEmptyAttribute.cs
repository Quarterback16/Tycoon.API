﻿using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on if the value of a dependent property should be cleared.
    /// </summary>
    public class ClearIfEmptyAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfEmptyAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public ClearIfEmptyAttribute(string dependentProperty) : base(ActionForDependencyType.Disabled, dependentProperty) { }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override bool IsConditionMet(object propertyValue, object dependentPropertyValue, object container)
        {
            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            if (FailOnNull && dependentPropertyValue == null)
            {
                return false;
            }

            if (dependentPropertyValue == null)
            {
                return true;
            }

            if (dependentPropertyValue is string)
            {
                return string.IsNullOrEmpty(dependentPropertyValue.ToString().Trim());
            }

            return ComparisonType.EqualTo.Compare(dependentPropertyValue, DelegateHelper.CreateConstructorDelegate(dependentPropertyValue.GetType())());
        }
    }
}
