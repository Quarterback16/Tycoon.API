using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Exceptions
{
    /// <summary>
    ///This is a test class for SelectorExceptionTest and is intended
    ///to contain all SelectorExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SelectorExceptionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
      

        /// <summary>
        ///A test for SelectorException Constructor
        ///</summary>
        [TestMethod()]
        public void SelectorExceptionConstructorTest()
        {
            const string message = "Message";
            var target = new SelectorException(message);
            Assert.IsNotNull(target);
            Assert.AreEqual("Message",target.Message);
        }
    }
}
