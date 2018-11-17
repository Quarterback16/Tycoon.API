using System;
using Employment.Web.Mvc.Infrastructure.Registrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Registrations
{
    /// <summary>
    ///This is a test class for AreaRegistrationTest and is intended
    ///to contain all AreaRegistrationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AreaRegistrationTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

       /// <summary>
        ///A test for AreaRegistration Constructor
        ///</summary>
        [TestMethod()]
        public void AreaRegistrationConstructorTest()
        {
            var target = new AreaRegistration();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Register
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterTest()
        {
            var target = new AreaRegistration();
            target.Register(); 
            Assert.Fail();
        }
    }
}
