using System.Diagnostics.CodeAnalysis;
using Employment.Web.Mvc.Infrastructure.ViewEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests
{
    /// <summary>
    ///This is a test class for CsRazorViewEngineTest and is intended
    ///to contain all CsRazorViewEngineTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CsRazorViewEngineTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }


        /// <summary>
        ///A test for CsRazorViewEngine Constructor
        ///</summary>
        [TestMethod()]
        public void CsRazorViewEngineConstructorTest()
        {
            var target = new CsRazorViewEngine();
            Assert.IsNotNull(target);
        }
    }
}
