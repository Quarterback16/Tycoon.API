using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ComparisonType"/>.
    /// </summary>
    public static class ComparisonTypeExtension
    {
        /// <summary>
        /// Get whether the comparison is valid.
        /// </summary>
        /// <param name="comparisonType">The comparison type value.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="valueToTestAgainst">The value to test the dependent property value against.</param>
        /// <returns><c>true</c> if the comparison is valid; otherwise, <c>false</c>.</returns>
        public static bool Compare(this ComparisonType comparisonType, object dependentPropertyValue, object valueToTestAgainst)
        {
            switch (comparisonType)
            {
                case ComparisonType.NotEqualTo:
                    return IsNotEqualTo(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.LessThan:
                    return IsLessThan(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.LessThanOrEqualTo:
                    return IsLessThanOrEqualTo(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.GreaterThan:
                    return IsGreaterThan(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.GreaterThanOrEqualTo:
                    return IsGreaterThanOrEqualTo(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.RegExMatch:
                    return IsRegExMatch(dependentPropertyValue, valueToTestAgainst);
                case ComparisonType.NotRegExMatch:
                    return IsNotRegExMatch(dependentPropertyValue, valueToTestAgainst);
                default:
                    return IsEqualTo(dependentPropertyValue, valueToTestAgainst);
            }
        }

        private static bool IsEqualTo(object dependentPropertyValue, object valueToTestAgainst)
        {
            return dependentPropertyValue == null ? valueToTestAgainst == null : dependentPropertyValue.Equals(valueToTestAgainst);
        }

        private static bool IsNotEqualTo(object dependentPropertyValue, object valueToTestAgainst)
        {
            return dependentPropertyValue == null ? valueToTestAgainst != null : !dependentPropertyValue.Equals(valueToTestAgainst);
        }

        private static bool IsLessThan(object dependentPropertyValue, object valueToTestAgainst)
        {
            return (dependentPropertyValue == null || valueToTestAgainst == null) ? false : Comparer<object>.Default.Compare(dependentPropertyValue, valueToTestAgainst) <= -1;
        }

        private static bool IsLessThanOrEqualTo(object dependentPropertyValue, object valueToTestAgainst)
        {
            if (dependentPropertyValue == null && valueToTestAgainst == null)
            {
                return true;
            }

            if (dependentPropertyValue == null || valueToTestAgainst == null)
            {
                return false;
            }

            return IsEqualTo(dependentPropertyValue, valueToTestAgainst) || Comparer<object>.Default.Compare(dependentPropertyValue, valueToTestAgainst) <= -1;
        }

        private static bool IsGreaterThan(object dependentPropertyValue, object valueToTestAgainst)
        {
            return (dependentPropertyValue == null || valueToTestAgainst == null) ? false : Comparer<object>.Default.Compare(dependentPropertyValue, valueToTestAgainst) >= 1;
        }

        private static bool IsGreaterThanOrEqualTo(object dependentPropertyValue, object valueToTestAgainst)
        {
            if (dependentPropertyValue == null && valueToTestAgainst == null)
            {
                return true;
            }

            if (dependentPropertyValue == null || valueToTestAgainst == null)
            {
                return false;
            }

            return IsEqualTo(dependentPropertyValue, valueToTestAgainst) || Comparer<object>.Default.Compare(dependentPropertyValue, valueToTestAgainst) >= 1;
        }

        private static bool IsRegExMatch(object dependentPropertyValue, object valueToTestAgainst)
        {
            return Regex.Match((dependentPropertyValue ?? string.Empty).ToString(), valueToTestAgainst.ToString()).Success;
        }

        private static bool IsNotRegExMatch(object dependentPropertyValue, object valueToTestAgainst)
        {
            return !Regex.Match((dependentPropertyValue ?? string.Empty).ToString(), valueToTestAgainst.ToString()).Success;
        }
    }
}
