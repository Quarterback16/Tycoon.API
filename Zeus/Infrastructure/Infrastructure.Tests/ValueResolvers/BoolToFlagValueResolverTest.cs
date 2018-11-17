using System;
using Employment.Web.Mvc.Infrastructure.TypeConverters;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="BoolToFlagValueResolver" />.
    ///</summary>
    [TestClass]
    public class BoolToFlagValueResolverTest
    {

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        /// Test the resolves source of true to destination of "Y".
        ///</summary>
        [TestMethod]
        public void BoolToFlagValueResolver_ResolveWithSourceTrue_ResolvesDestinationAsY()
        {
            //var sut = SystemUnderTest();

            bool source = true;
            string destination = string.Empty;

            destination = BoolToFlagValueResolver.Resolve(source);

            Assert.IsTrue(destination == "Y");
        }

        /// <summary>
        /// Test the resolves source of false to destination of "N".
        ///</summary>
        [TestMethod]
        public void BoolToFlagValueResolver_ResolveWithSourceFalse_ResolvesDestinationAsN()
        {
            bool source = false;
            string destination = string.Empty;

            destination = BoolToFlagValueResolver.Resolve(source);

            Assert.IsTrue(destination == "N");
        }
    }
}
