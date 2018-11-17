using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Suspensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Suspensions
{
    /// <summary>
    /// Unit tests for <see cref="SuspensionsMapper" />.
    /// </summary>
    [TestClass]
    public class SuspensionsMapperTest
    {
        private SuspensionsMapper SystemUnderTest()
        {
            return new SuspensionsMapper();
        }

        /// <summary>
        /// Test map is valid.
        /// </summary>
        [TestMethod]
        public void Map_Valid()
        {
            SystemUnderTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
