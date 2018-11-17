using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Wrappers;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Infrastructure.Tests.Wrappers
{
    /// <summary>
    ///This is a test class for ConfigurationManagerWrapperTest and is intended
    ///to contain all ConfigurationManagerWrapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationManagerWrapperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ConfigurationManagerWrapper Constructor
        ///</summary>
        [TestMethod()]
        public void ConfigurationManagerWrapperConstructorTest()
        {
            var m = new ConfigurationManagerWrapper();
            Assert.IsNotNull(m);
        }

        /// <summary>
        ///A test for AppSettings
        ///</summary>
        [TestMethod()]
        public void AppSettingsTest()
        {
            var cmw = new ConfigurationManagerWrapper();
            Assert.IsNotNull(cmw.AppSettings);
        }

        /// <summary>
        ///A test for Current
        ///</summary>
        [TestMethod()]
        public void CurrentTest()
        {
            Assert.IsNotNull(ConfigurationManagerWrapper.Current);
        }

        [TestMethod]
        public void ConnectionStrings()
        {
            var cmw = new ConfigurationManagerWrapper();
            Assert.IsNotNull(cmw.ConnectionStrings("Db_ConnADW"));
        }

        [TestMethod]
        public void GetUnitySection()
        {
            var cmw = new ConfigurationManagerWrapper();

            var section = cmw.GetSection<UnityConfigurationSection>("unity");
            Assert.IsNotNull(section);
        }
    }
}