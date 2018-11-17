using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Employment.Web.Mvc.Infrastructure.Tests.Wrappers
{
    /// <summary>
    ///This is a test class for BuildManagerWrapperTest and is intended
    ///to contain all BuildManagerWrapperTest Unit Tests
    /// BuildManager is not easily tested or Moq friendly
    ///</summary>
    [TestClass()]
    public class BuildManagerWrapperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for BuildManagerWrapper Constructor
        ///</summary>
        [TestMethod()]
        public void BuildManagerWrapperConstructorTest()
        {
            var target = new BuildManagerWrapper();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Assemblies
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssembliesTest()
        {
            var buildManager = BuildManagerWrapper.Current;
            Assert.IsFalse(buildManager.Assemblies.Any());
        }

        /// <summary>
        ///A test for ConcreteTypes
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConcreteTypesTest()
        {
            var buildManager = BuildManagerWrapper.Current;
            Assert.IsFalse(buildManager.ConcreteTypes.Any());
        }

        /// <summary>
        ///A test for Current
        ///</summary>
        [TestMethod()]
        public void CurrentTest()
        {
            Assert.IsNotNull(BuildManagerWrapper.Current);
        }

        /// <summary>
        ///A test for PublicTypes
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PublicTypesTest()
        {
            var buildManager = BuildManagerWrapper.Current;
            Assert.IsFalse(buildManager.PublicTypes.Any());
        }

        /// <summary>
        /// Resolve empty type throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResolveType_ThrowsException()
        {
            var m = BuildManagerWrapper.Current;
            m.ResolveType("");
        }
    }
}