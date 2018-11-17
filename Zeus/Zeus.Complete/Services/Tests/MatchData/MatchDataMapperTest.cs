using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.MatchData;

namespace Employment.Web.Mvc.Service.Tests.MatchData
{
    /// <summary>
    /// Unit tests for <see cref="EmployerMapper" />.
    /// </summary>
    [TestClass]
    public class MatchDataMapperTest
    {
        private MatchDataMapper SystemUnderTest()
        {
            return new MatchDataMapper();
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
