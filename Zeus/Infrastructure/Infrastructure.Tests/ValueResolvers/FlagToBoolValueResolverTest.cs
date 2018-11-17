using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="FlagToBoolValueResolver" />.
    ///</summary>
    [TestClass]
    public class FlagToBoolValueResolverTest
    {

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        /// <summary>
        /// Test the resolves source of "Y" to destination of true.
        ///</summary>
        [TestMethod]
        public void FlagToBoolValueResolver_ResolveWithSourceY_ResolvesDestinationAsTrue()
        {
            string source = "Y";
            bool destination = false;

            destination = FlagToBoolValueResolver.Resolve(source);

            Assert.IsTrue(destination);
        }

        /// <summary>
        /// Test the resolves source of "N" to destination of false.
        ///</summary>
        [TestMethod]
        public void FlagToBoolValueResolver_ResolveWithSourceN_ResolvesDestinationAsFalse()
        {

            string source = "N";
            bool destination = true;

            destination = FlagToBoolValueResolver.Resolve(source);

            Assert.IsFalse(destination);
        }
    }
}
