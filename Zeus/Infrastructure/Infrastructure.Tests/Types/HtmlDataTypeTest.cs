using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Helpers;

namespace Employment.Web.Mvc.Infrastructure.Tests.Types
{
    [TestClass()]
    public class HtmlDataTypeTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void UniqueValues()
        {
            var htmlDataType = new HtmlDataType();
            var properties = htmlDataType.GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.FieldType == typeof(string));

            var list = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(htmlDataType) as string;

                // Ensure we have a value
                Assert.IsFalse(string.IsNullOrEmpty(value));
                
                // Fail if the value has already been used
                Assert.IsFalse(list.Contains(value), "The static readonly property '{0}' is using a value that's already in use: {1}", property.Name, value);

                // Add value to check for on next iteration
                list.Add(value);
            }
        }

        public void ConvertToJavaScript()
        {
            var htmlDataType = new HtmlDataType();
            var properties = htmlDataType.GetType().GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.FieldType == typeof(string));

            var values = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(htmlDataType) as string;

                values.Add(property.Name, value);
            }

            var sb = new StringBuilder();

            foreach (var kvp in values)
            {
                sb.AppendLine(string.Format("{0}: '{1}',{2}", kvp.Key, kvp.Value, Environment.NewLine));
            }

            var result = sb.ToString().Trim(',');
        }
    }
}
