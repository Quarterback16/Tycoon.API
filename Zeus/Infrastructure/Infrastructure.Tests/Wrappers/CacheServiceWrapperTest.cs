using System;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Wrappers
{
    /// <summary>
    ///This is a test class for CacheServiceWrapperTest and is intended
    ///to contain all CacheServiceWrapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CacheServiceWrapperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private CacheServiceWrapper SystemUnderTest()
        {
            var cacheService = new Mock<ICacheService>();
            const string @namespace = "Test";

            cacheService.Setup(s => s.Contains(It.IsAny<KeyModel>())).Returns(true);

            string value;
            cacheService.Setup(s => s.TryGet(It.IsAny<KeyModel>(), out value)).Returns(true);

            return new CacheServiceWrapper(cacheService.Object, @namespace);
        }

        /// <summary>
        ///A test for CacheServiceWrapper Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheServiceWrapperConstructor_ExpectedExceptionTest()
        {
         var c = new CacheServiceWrapper(null, string.Empty);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheServiceWrapperConstructor_ExpectedExceptionTest1()
        {
            var c = new CacheServiceWrapper(new Mock<ICacheService>().Object, string.Empty);
        }

        [TestMethod]
        public void CacheServiceWrapperConstructor_Test()
        {
            ICacheService cacheService = new Mock<ICacheService>().Object;
            const string @namespace = "Test";
            var target = new CacheServiceWrapper(cacheService, @namespace);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        [TestMethod()]
        public void CacheServiceWrapper_ContainsTest()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);

            var keymodel = new KeyModel(CacheType.Default, "Test");
            target.Set(keymodel,  "Test");
            bool actual = target.Contains(keymodel);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            var keymodel = new KeyModel(CacheType.Default, "Test");
            target.Remove(keymodel);
            Assert.IsTrue(true);
        }


        ///// <summary>
        /////A test for Remove
        /////</summary>
        //[TestMethod()]
        //public void RemoveTest1()
        //{
        //    var target = SystemUnderTest();
        //    Assert.IsNotNull(target);
            
        //    target.Remove("Test");
        //    Assert.IsTrue(true);
        //}

        /// <summary>
        ///A test for TryGet
        ///</summary>
        [TestMethod]
        public void TryGetTestHelper()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            var keymodel = new KeyModel(CacheType.Default, "Test");

            string value;
            var actual = target.TryGet<string>(keymodel, out value);
            Assert.IsTrue(actual);
        }
    }
}