using System;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using System.Web.Caching;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    
    
    /// <summary>
    ///This is a test class for CacheServiceTest and is intended
    ///to contain all CacheServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HttpCacheServiceTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for CacheService Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CacheServiceConstructor_ExpectedException()
        {
            var target = new HttpCacheService(null);
            Assert.Fail("Expected Exception");
        }
    }
}