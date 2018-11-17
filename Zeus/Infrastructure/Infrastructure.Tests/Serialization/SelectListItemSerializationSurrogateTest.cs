using System;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Serialization
{
    /// <summary>
    /// Unit tests for <see cref="SelectListItemSerializationSurrogate" />.
    /// </summary>
    [TestClass]
    public class SelectListItemSerializationSurrogateTest
    {
        [TestMethod]
        public void SelectListItemSerializationSurrogate()
        {
            var sut = new SelectListItemSerializationSurrogate();
            var item = new SelectListItem { Selected = true, Text = "Foo", Value = "Bar" };

            var si = new SerializationInfo(typeof (SelectListItem), new FormatterConverter());

            sut.GetObjectData(item, si, new StreamingContext());

            Assert.AreEqual(3, si.MemberCount);
            Assert.AreEqual(item.Selected, si.GetValue("Selected", typeof(bool)));
            Assert.AreEqual(item.Text, si.GetValue("Text", typeof(string)));
            Assert.AreEqual(item.Value, si.GetValue("Value", typeof(string)));

            var setItem = new SelectListItem();

            sut.SetObjectData(setItem, si, new StreamingContext(), null);

            Assert.AreEqual(item.Selected, setItem.Selected);
            Assert.AreEqual(item.Text, setItem.Text);
            Assert.AreEqual(item.Value, setItem.Value);
        }
    }
}
