using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Collections.Generic.IEnumerable{T}" />.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Add ForEach behaviour to generic IEnumerable.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="enumerable">Collection of objects.</param>
        /// <param name="action">Action to perform on each object.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T obj in enumerable)
            {
                action(obj);
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{SelectListItem}" /> by using the enumerable and the specified data value field, data text field, and selected values.
        /// </summary>
        /// <param name="enumerable">The enumerable data to convert.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text)
        {
            return enumerable.ToSelectListItem(value, text, null);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{SelectListItem}" /> by using the enumerable and the specified data value field, data text field, and selected values.
        /// </summary>
        /// <param name="enumerable">The enumerable data to convert.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        /// <param name="selectedValue">The selected value.</param>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text, object selectedValue)
        {
            IEnumerable<object> selected = Enumerable.Empty<object>();

            if (selectedValue != null)
            {
                if (selectedValue.GetType().IsArray || (selectedValue.GetType().IsGenericType && selectedValue.GetType().GetGenericTypeDefinition().IsEnumerableType()))
                {
                    selected = (selectedValue as IEnumerable).Cast<object>();
                }
                else
                {
                    selected = new[] { selectedValue }.AsEnumerable();
                }
            }
	        HashSet<string> selectedHash = null;
	        if (selected != null && selected.Any())
	        {
				selectedHash=new HashSet<string>(StringComparer.Ordinal);
		        foreach (object item in selected)
		        {
			        selectedHash.Add(item.ToString());
		        }
	        }

	        return enumerable.ToSelectListItem(value, text, selectedHash);
        }

		        /// <summary>
        /// Returns an <see cref="IEnumerable{SelectListItem}" /> by using the enumerable and the specified data value field, data text field, and selected values.
        /// </summary>
        /// <param name="enumerable">The enumerable data to convert.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        /// <param name="selectedValues">The selected values.</param>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text, HashSet<string> selectedValues)
        {
            foreach (var f in enumerable)
            {
                yield return new SelectListItem { Value = value(f), Text = text(f), Selected = selectedValues != null && selectedValues.Contains(value(f)) };
            }
        }
        
        /// <summary>
        /// Returns an <see cref="IEnumerable{SelectListItem}" /> by using the enumerable and the specified data value field, data text field, and selected values.
        /// </summary>
        /// <param name="enumerable">The enumerable data to convert.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        /// <param name="selectedValues">The selected values.</param>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text, IEnumerable<object> selectedValues)
        {
            foreach (var f in enumerable)
            {
                yield return new SelectListItem { Value = value(f), Text = text(f), Selected = selectedValues != null ? selectedValues.Any(v => v.ToString() == value(f)) : false };
            }
        }

        /// <summary>
        /// Returns a <see cref="SelectList" /> by using the specified collection for data value field, the data text field, and the selected value.
        /// </summary>
        /// <param name="enumerable">The collection of items.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text)
        {
            return new SelectList(enumerable.ToSelectListItem(value, text), "Value", "Text", null);
        }

        /// <summary>
        /// Returns a <see cref="SelectList" /> by using the specified collection for data value field, the data text field, and the selected value.
        /// </summary>
        /// <param name="enumerable">The collection of items.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        /// <param name="selectedValue">The selected value.</param>
        public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text, object selectedValue)
        {
            return new SelectList(enumerable.ToSelectListItem(value, text), "Value", "Text", selectedValue);
        }

        /// <summary>
        /// Returns a <see cref="MultiSelectList" /> by using the specified collection for data value field, the data text field, and the selected values.
        /// </summary>
        /// <param name="enumerable">The collection of items.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        public static MultiSelectList ToMultiSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text)
        {
            return new MultiSelectList(enumerable.ToSelectListItem(value, text), "Value", "Text", null);
        }

        /// <summary>
        /// Returns a <see cref="MultiSelectList" /> by using the specified collection for data value field, the data text field, and the selected values.
        /// </summary>
        /// <param name="enumerable">The collection of items.</param>
        /// <param name="value">The data value field.</param>
        /// <param name="text">The data text field.</param>
        /// <param name="selectedValues">The selected values.</param>
        public static MultiSelectList ToMultiSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> value, Func<T, string> text, IEnumerable<object> selectedValues)
        {
            return new MultiSelectList(enumerable.ToSelectListItem(value, text), "Value", "Text", selectedValues);
        }
    }
}