﻿/******************************************************************************* 
 *  _                      _     _ _ _         
 * | |   _   _  __ _ _   _(_) __| (_) |_ _   _ 
 * | |  | | | |/ _` | | | | |/ _` | | __| | | |
 * | |__| |_| | (_| | |_| | | (_| | | |_| |_| |
 * |_____\__, |\__, |\__,_|_|\__,_|_|\__|\__, |
 *       |___/    |_|                    |___/ 
 * 
 *  Lyquidity AmazonSES for Exchange
 *  Version: 1.0.0.1
 *  Generated: Monday Jan 31 20:00:00 GMT 2011 
 *  *
 * ***************************************************************************** 
 *  Copyright Lyquidity Solutions Limited 2011
 * ***************************************************************************** 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

namespace HearthCap.Framework.Validation
{
    /// <summary>
    ///     Allows a property to be decorated with information about the corresponding label
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class LabelDescriptionAttribute : Attribute
    {
        /// <summary>
        ///     The name of the element which is the label (assumed to be [property name]Label if not specified)
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        ///     The name of the element property which holds the label text (defaults to Text)
        /// </summary>
        public string LabelPropertyName { get; set; }

        /// <summary>
        ///     The description to use can either be a string supplied here or as a classname/property pair for localization
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     The name of the class which holds the localized text
        /// </summary>
        public Type LabelResourceType { get; set; }

        /// <summary>
        ///     The name of the property of the localization class which holds the string to display
        /// </summary>
        public string LabelResourceName { get; set; }

        /// <summary>
        ///     Static method to apply labels for a model's properties.  The model specified MUST be view aware.
        /// </summary>
        /// <param name="model">The view to which the label should be applied</param>
        public static void ApplyLabels(IScreen model)
        {
            if (model == null)
            {
                return;
            }

            var va = model as IViewAware;
            if (va == null)
            {
                return;
            }

            var view = va.GetView();
            if (view == null)
            {
                return;
            }

            // Create a dictionary of the label (if any) associated with each class property
            var modelsLabels =
                (from p in model.GetType().GetProperties()
                    let attrs = p.GetAttributes<LabelDescriptionAttribute>(true).ToArray()
                    where attrs.Length != 0
                    select new KeyValuePair<string, LabelDescriptionAttribute[]>(p.Name, attrs)
                    ).ToDictionary(p => p.Key, p => p.Value);

            // Grab any model level properties
            var modelAttrs = model.GetType().GetAttributes<LabelDescriptionAttribute>(true).ToArray();
            if (modelAttrs.Count() > 0)
            {
                modelsLabels.Add("", modelAttrs);
            }

            // Grab the dictionary
            foreach (var name in modelsLabels.Keys)
            {
                foreach (var l in modelsLabels[name])
                {
                    var label = "";

                    // Used to hold previously created classes
                    var references = new Dictionary<string, object>();

                    if (l.LabelResourceType == null
                        || string.IsNullOrEmpty(l.LabelResourceName))
                    {
                        label = l.Label;
                    }
                    else
                    {
                        // Look up the description (if possible)
                        object reference = null;

                        if (references.ContainsKey(l.LabelResourceType.Name))
                        {
                            reference = references[l.LabelResourceType.Name];
                        }
                        else
                        {
                            // Try to get the type
                            reference = Activator.CreateInstance(l.LabelResourceType, true);
                            references.Add(l.LabelResourceType.Name, reference);
                        }

                        // Now try to find the description in the reference class
                        var pi = reference.GetType().GetProperty(l.LabelResourceName);
                        if (pi == null)
                        {
                            continue;
                        }

                        var v = pi.GetValue(reference, null);
                        label = v.ToString();
                    }

                    // If this is a model level label then the attribute MUST specify an element name
                    if (string.IsNullOrEmpty(name)
                        && string.IsNullOrEmpty(l.ElementName))
                    {
                        continue;
                    }

                    var labelName = string.IsNullOrEmpty(l.ElementName) ? name + "Label" : l.ElementName;

                    // Next, find the named element in the view
                    // Named elements appear as properties of the view
                    var element = (view as FrameworkElement).FindName(labelName);
                    if (element == null)
                    {
                        continue;
                    }

                    var labelPropertyName = string.IsNullOrEmpty(l.LabelPropertyName)
                        ? element.GetType().Name == "Button"
                            ? "Content"
                            : element.GetType().Name == "TabItem"
                                ? "Header"
                                : "Text"
                        : l.LabelPropertyName;

                    var piText = element.GetType().GetProperty(labelPropertyName);
                    if (piText == null)
                    {
                        continue;
                    }
                    piText.SetValue(element, label, null);
                }
            }
        }
    }
}
