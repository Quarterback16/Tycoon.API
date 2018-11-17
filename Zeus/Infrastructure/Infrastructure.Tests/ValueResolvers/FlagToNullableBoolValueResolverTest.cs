using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.ValueResolvers
{
    /// <summary>
    /// Unit tests for <see cref="FlagToNullableBoolValueResolver" />.
    ///</summary>
    [TestClass]
    public class FlagToNullableBoolValueResolverTest
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
        public void FlagToNullableBoolValueResolver_ResolveWithSourceY_ResolvesDestinationAsTrue()
        {
            string source = "Y";
            bool? destination = false;

            destination = FlagToNullableBoolValueResolver.Resolve(source);

            Assert.IsNotNull(destination);
            Assert.IsTrue(destination.Value);
        }

        /// <summary>
        /// Test the resolves source of "N" to destination of false.
        ///</summary>
        [TestMethod]
        public void FlagToNullableBoolValueResolver_ResolveWithSourceN_ResolvesDestinationAsFalse()
        {

            string source = "N";
            bool? destination = true;

            destination = FlagToNullableBoolValueResolver.Resolve(source);

            Assert.IsNotNull(destination);
            Assert.IsFalse(destination.Value);
        }

        /// <summary>
        /// Test the resolves source of empty to destination of null.
        ///</summary>
        [TestMethod]
        public void FlagToNullableBoolValueResolver_ResolveWithSourceEmpty_ResolvesDestinationAsNull()
        {

            string source = string.Empty;
            bool? destination = true;

            destination = FlagToNullableBoolValueResolver.Resolve(source);

            Assert.IsNull(destination);
        }
    }
}
