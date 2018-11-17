using System;
using System.Collections.Generic;
using System.Reflection;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.DataAnnotations
{
    /// <summary>
    /// Unit tests for <see cref="SwitcherAttribute" />.
    /// </summary>
    [TestClass]
    public class SwitcherAttributeTest
    {
        const string cText = "checking";
        const string uText = "unchecking";

        private class Model
        {
            [Switcher]
            public bool defaultBool { get; set; }

            [Switcher(SwitcherAttributeTest.cText, SwitcherAttributeTest.uText)]
            public bool customBool { get; set; }
        }

        [TestMethod]
        public void Switcher_ConstructionDefault()
        {
            SwitcherAttribute systemUnderTest = new SwitcherAttribute();

            Assert.IsNotNull(systemUnderTest);
            Assert.IsNotNull(systemUnderTest.CheckedText);
            Assert.IsNotNull(systemUnderTest.UncheckedText);
        }

        [TestMethod]
        public void Switcher_ConstructionWithParameters()
        {

            SwitcherAttribute systemUnderTest = new SwitcherAttribute(cText, uText);

            Assert.IsNotNull(systemUnderTest);
            Assert.AreEqual(cText, systemUnderTest.CheckedText);
            Assert.AreEqual(uText, systemUnderTest.UncheckedText);
        }

        [TestMethod]
        public void Switcher_FromMetadata()
        {
            Model model = new Model();

            SwitcherAttribute att = model.GetType().GetProperty("defaultBool").GetCustomAttribute<SwitcherAttribute>();
            Assert.IsNotNull(att);
            Assert.IsNotNull(att.CheckedText);
            Assert.IsNotNull(att.UncheckedText);

            att = model.GetType().GetProperty("customBool").GetCustomAttribute<SwitcherAttribute>();
            Assert.IsNotNull(att);
            Assert.AreEqual(cText, att.CheckedText);
            Assert.AreEqual(uText, att.UncheckedText);
        }
    }
}
