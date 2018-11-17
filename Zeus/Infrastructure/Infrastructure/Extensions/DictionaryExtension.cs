using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Convert from a dictionary to a query string using the key value pairs.
        /// </summary>
        /// <param name="dictionary">The dictionary instance.</param>
        /// <returns>The dictionary as a query string.</returns>
        public static string ToQueryString(this IDictionary<string, object> dictionary)
        {
            return dictionary.ToQueryString(false);
        }

        /// <summary>
        /// Convert from a dictionary to a query string using the key value pairs.
        /// </summary>
        /// <param name="dictionary">The dictionary instance.</param>
        /// <param name="lowerCaseKey">Whether to force the key to be lower case.</param>
        /// <returns>The dictionary as a query string.</returns>
        public static string ToQueryString(this IDictionary<string, object> dictionary, bool lowerCaseKey)
        {
            if (dictionary == null || dictionary.Count <= 0)
            {
                return null;
            }
            var query = new StringBuilder();
            if (lowerCaseKey)
            {
                dictionary.ForEach(d => query.AppendFormat("{0}={1}&", d.Key.ToLower(), d.Value));
            }
            else
            {
                dictionary.ForEach(d => query.AppendFormat("{0}={1}&", d.Key, d.Value));
            }
            if (query.Length > 0 && query[query.Length - 1] == '&')
            {
                query.Remove(query.Length - 1, 1);
            }

            return query.ToString();
        }

        /// <summary>
        /// Merges css classes into the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary instance.</param>
        /// <param name="cssClassNames">The CSS class names to merge.</param>
        public static void MergeCssClass(this IDictionary<string, object> dictionary, params string[] cssClassNames)
        {
            if (dictionary == null || cssClassNames == null || cssClassNames.Length == 0)
            {
                return;
            }

            string currentCssClassNames = string.Empty;
            object current;

            // Check for existing CSS classes
            if (dictionary.TryGetValue("class", out current) && current is string)
            {
                currentCssClassNames = current as string;
            }

            // Merge all CSS classes together, ignoring empty entries
            var mergedCssClassNames = new string[] { currentCssClassNames }.Concat(cssClassNames).Where(s => !string.IsNullOrWhiteSpace(s));
            
            dictionary["class"] = string.Join(" ", mergedCssClassNames).Trim();
        }
        
        /// <summary>
        /// Removes css classes from the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary instance.</param>
        /// <param name="cssClassNames">The CSS class names to remove.</param>
        public static void RemoveCssClass(this IDictionary<string, object> dictionary, params string[] cssClassNames)
        {
            if (dictionary == null || cssClassNames == null || cssClassNames.Length == 0)
            {
                return;
            }

            string currentCssClassNames = string.Empty;
            object current;

            // Check for existing CSS classes
            if (dictionary.TryGetValue("class", out current) && current is string)
            {
                currentCssClassNames = current as string;
            }

            // Remove the CSS classes
            dictionary["class"] = string.Join(" ", currentCssClassNames.Split(new char [] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(s => !cssClassNames.Contains(s)));
        }
    }
}
