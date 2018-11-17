using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to provide selection options for string or IEnumerable<string> properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SelectionAttribute : SelectionTypeAttribute, IMetadataAware
    {
        /// <summary>
        /// The key values.
        /// </summary>
        /// <remarks>
        /// The keys are paired with <see cref="Values" />.
        /// </remarks>
        public string[] Keys { get; private set; }

        /// <summary>
        /// The display text for the key values.
        /// </summary>
        /// <remarks>
        /// The values are paired with <see cref="Keys" />.
        /// </remarks>
        public string[] Values { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.SelectionTypeAttribute" /> class.
        /// </summary>
        /// <param name="selectionType">The <see cref="SelectionType" /> setting.</param>
        /// <param name="keys">The key values.</param>
        /// <param name="values">The display text values for the key values.</param>
        public SelectionAttribute(SelectionType selectionType, string[] keys, string[] values) : base(selectionType)
        {
            if (keys == null || keys.Length == 0)
            {
                throw new ArgumentNullException("keys");
            }

            if (values == null || values.Length == 0)
            {
                throw new ArgumentNullException("values");
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys and Values must be the same length.");
            }

            if (keys.Contains(null) || keys.Contains(string.Empty))
            {
                throw new ArgumentException("Keys cannot contain a null or empty value. Use AllowEmpty property instead.");
            }

            Keys = keys;
            Values = values;
        }

        /// <summary>Determines whether the specified value of the object is valid.</summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var values = value as IEnumerable<object>;

                if ((values == null && SelectionType == SelectionType.None) || SelectionType == SelectionType.Single)
                {
                    return (string.IsNullOrEmpty(value.ToString()) || Keys.Contains(value));
                }

                if (values != null && (SelectionType == SelectionType.None || SelectionType == SelectionType.Multiple))
                {
                    foreach (var v in values)
                    {
                        if (!(string.IsNullOrEmpty(v.ToString()) || Keys.Contains(v)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the <see cref="Keys" /> and <see cref="Values" /> merged into a <see cref="IEnumerable{SelectListItem}" />.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" />.</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return GetSelectListItems(null);
        }

        /// <summary>
        /// Returns the <see cref="Keys" /> and <see cref="Values" /> merged into a <see cref="IEnumerable{SelectListItem}" />.
        /// </summary>
        /// <param name="selectedKeys">The key values that are selected.</param>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" />.</returns>
        public IEnumerable<SelectListItem> GetSelectListItems(object selectedKeys)
        {
            var items = new List<SelectListItem>();

            for (int i = 0; i < Keys.Length; i++)
            {
                var selected = false;

                if (selectedKeys != null)
                {
                    var values = selectedKeys as IEnumerable<object>;

                    if ((values == null && SelectionType == SelectionType.None) || SelectionType == SelectionType.Single)
                    {
                        selected = Keys[i].Equals(selectedKeys.ToString());
                    }
                    else if (values != null && (SelectionType == SelectionType.None || SelectionType == SelectionType.Multiple))
                    {
                        if ((selectedKeys as IEnumerable<object>).Contains(Keys[i]))
                        {
                            selected = true;
                        }
                    }
                }
                
                items.Add(new SelectListItem { Value = Keys[i], Text = Values[i], Selected = selected });
            }

            return items;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata.ModelType == typeof(IEnumerable<string>))
            {
                if (SelectionType == SelectionType.Multiple || SelectionType == SelectionType.None)
                {
                    metadata.TemplateHint = "SelectionMultiple";
                }
                else
                {
                    throw new InvalidOperationException("SelectionType must be Multiple or None for a property type of IEnumerable<string>.");
                }
            }
            else if (metadata.ModelType == typeof(string))
            {
                if (SelectionType == SelectionType.Single || SelectionType == SelectionType.None)
                {
                    metadata.TemplateHint = "SelectionSingle";
                }
                else
                {
                    throw new InvalidOperationException("SelectionType must be Single or None for a property type of string.");
                }
            }
            else
            {
                throw new InvalidOperationException("Property type must be either string or IEnumerable<string>.");
            }
        }
    }
}
