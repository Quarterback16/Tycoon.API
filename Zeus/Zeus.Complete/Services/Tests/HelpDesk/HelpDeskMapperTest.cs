using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.HelpDesk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.HelpDesk
{
    /// <summary>
    /// Unit tests for <see cref="HelpDeskMapper" />.
    /// </summary>
    [TestClass]
    public class HelpDeskMapperTest
    {
        private HelpDeskMapper SystemUnderTest()
        {
            return new HelpDeskMapper();
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
