using Employment.Web.Mvc.Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Tests.ViewModels
{
    /// <summary>
    ///This is a test class for DynamicViewModelTest and is intended
    ///to contain all DynamicViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DynamicViewModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for DynamicViewModel Constructor
        ///</summary>
        [TestMethod()]
        public void DynamicViewModelConstructorTest()
        {
            var target = new DynamicViewModel();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod]
        public void AddTestHelper()
        {
            var target = new DynamicViewModel();
            ObjectViewModel<string> ob  = new StringViewModel();
            ob.Value = "Test";
            target.Add(ob);
            Assert.IsTrue(target.Values.Count == 1);
            Assert.AreEqual("Test", ((ObjectViewModel<string> )target.Values[0]).Value);
        }

        /// <summary>
        ///A test for Values
        ///</summary>
        [TestMethod()]
        public void ValuesTest()
        {
            var target = new DynamicViewModel(); 
            List<object> actual = target.Values;
            Assert.IsNotNull(actual);
        }
    }
}