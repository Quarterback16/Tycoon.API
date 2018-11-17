using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.ParticipantCompliance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.ParticipantCompliance
{
    /// <summary>
    /// Unit tests for <see cref="ParticipantComplianceMapper" />.
    /// </summary>
    [TestClass]
    public class ParticipantComplianceTest
    {
        private ParticipantComplianceMapper SystemUnderTest()
        {
            return new ParticipantComplianceMapper();
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
