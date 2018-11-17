using System.Linq;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.Tests.Exceptions
{
    
    
    /// <summary>
    ///This is a test class for ServiceValidationExceptionTest and is intended
    ///to contain all ServiceValidationExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceValidationExceptionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for ServiceValidationException Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceValidationExceptionConstructorTest()
        {
            string error = "Message";
            var target = new ServiceValidationException(error);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ServiceValidationException Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceValidationExceptionConstructorTest1()
        {
            const string error = "error message";
            const string parameterName = "parameter name";
            var target = new ServiceValidationException(error, parameterName);
            Assert.IsNotNull(target);
            Assert.IsTrue(target.Errors.Any());
        }

        /// <summary>
        ///A test for ServiceValidationException Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceValidationExceptionConstructorTest2()
        {
            var errors = new Dictionary<string, string>();
            errors.Add("key","value");
            var target = new ServiceValidationException(errors);
            Assert.IsNotNull(target);

            Assert.IsTrue(target.Errors.Count == 1);
            Assert.IsTrue(target.Errors.ContainsKey("key"));
            Assert.IsTrue(target.Errors.ContainsValue("value"));
        }
    }
}