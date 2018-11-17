using System;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for SerializeExtensionTest and is intended
    ///to contain all SerializeExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SerializeExtensionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Serialize
        ///</summary>
        [TestMethod()]
        public void SerializeTest()
        {
            string obj = "ABCDEFGH";
            string actual = SerializeExtension.Serialize(obj);
            Assert.IsNotNull(actual);

            var deserialized =  SerializeExtension.Deserialize(actual);
            Assert.IsNotNull(deserialized);

            Assert.AreEqual(obj,deserialized);
        }

        [TestMethod]
        public void DeserializeTest1()
        {
            var s = SerializeExtension.Serialize("ABCDEFG");
            byte[] data = Convert.FromBase64String(s);
            var d = SerializeExtension.Deserialize(data);
            Assert.AreEqual("ABCDEFG",d);
        }

        [TestMethod]
        public void SerializeNullTest()
        {
            Assert.IsNull(SerializeExtension.Serialize(null));
        }
        
        [TestMethod]
        public void SerializeTest1()
        {
            string o = "ABCDEFG";
            Assert.IsNotNull(SerializeExtension.Serialize((object)o));
        }

        [TestMethod]
        public void DeSerializeNullTest()
        {
            var d = SerializeExtension.Deserialize(string.Empty);
            Assert.IsNull(d);
        }

        [TestMethod]
        public void DeSerializeNullTest1()
        {
            var d = SerializeExtension.Deserialize((byte[])null);
            Assert.IsNull(d);

            var s = SerializeExtension.Deserialize((string)null);
            Assert.IsNull(s);
        }
    }
}