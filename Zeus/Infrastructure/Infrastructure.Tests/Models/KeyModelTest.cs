using System.Linq;
using Employment.Web.Mvc.Infrastructure.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Tests.Models
{


    /// <summary>
    ///This is a test class for KeyModelTest and is intended
    ///to contain all KeyModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KeyModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for KeyModel Constructor
        ///</summary>
        [TestMethod()]
        public void KeyModelConstructorTest()
        {
            const string key = "Key";
            var target = new KeyModel(key);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for KeyModel Constructor
        ///</summary>
        [TestMethod()]
        public void KeyModelConstructorTest1()
        {
            const string key = "Key";

            var target = new KeyModel(key);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            const string key = "Key";

            IList<object> values = new List<object>();
            var target = new KeyModel(key);
            Assert.IsNotNull(target);

            string value = "Value";

            KeyModel actual = target.Add(value);
            Assert.AreEqual(key, actual.Key);
            Assert.IsNotNull(actual.Values);
            var s = (string)actual.Values[0];
            Assert.AreEqual(value, s);
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {
            const string key = "Key";

            IList<object> values = new List<object>();
            var target = new KeyModel(key);
            Assert.IsNotNull(target);

            string value = "Value";

            KeyModel actual = target.Add(value);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Values.Any());

            target.Clear();
            Assert.IsFalse(target.Values.Any());
        }

        /// <summary>
        ///A test for Employment.Web.Mvc.Infrastructure.Interfaces.IFluent.GetType
        ///</summary>
        [TestMethod()]
        public void GetTypeTest()
        {
            const string key = "Key";
            IFluent target = new KeyModel(key);
            Type expected = typeof(KeyModel);
            Type actual = target.GetType();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetKey should throw exception if namespace not set
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetKeyTest()
        {
            const string key = "Key";
            var target = new KeyModel(key);
            const string expected = "Test.Key";
            string actual = target.GetKey();
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeyModelConstructor_ExpectedException4()
        {
            var target = new KeyModel(null);
        }

    }
}
