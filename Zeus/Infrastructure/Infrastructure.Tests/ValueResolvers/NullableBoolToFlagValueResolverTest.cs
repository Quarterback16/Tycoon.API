using System;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="NullableBoolToFlagValueResolver" />.
    ///</summary>
    [TestClass]
    public class NullableBoolToFlagValueResolverTest
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
        public void NullableBoolToFlagValueResolver_ResolveWithSourceTrue_ResolvesDestinationAsY()
        {

            bool? source = true;
            string destination = string.Empty;

            destination = NullableBoolToFlagValueResolver.Resolve(source);

            Assert.IsTrue(destination == "Y");
        }

        /// <summary>
        /// Test the resolves source of false to destination of "N".
        ///</summary>
        [TestMethod]
        public void NullableBoolToFlagValueResolver_ResolveWithSourceFalse_ResolvesDestinationAsN()
        {

            bool? source = false;
            string destination = string.Empty;

            destination = NullableBoolToFlagValueResolver.Resolve(source);

            Assert.IsTrue(destination == "N");
        }

        /// <summary>
        /// Test the resolves source of null to destination of empty.
        ///</summary>
        [TestMethod]
        public void NullableBoolToFlagValueResolver_ResolveWithSourceNull_ResolvesDestinationAsEmpty()
        {

            bool? source = null;
            string destination = string.Empty;

            destination = NullableBoolToFlagValueResolver.Resolve(source);

            Assert.IsTrue(destination == string.Empty);
        }
    }
}
