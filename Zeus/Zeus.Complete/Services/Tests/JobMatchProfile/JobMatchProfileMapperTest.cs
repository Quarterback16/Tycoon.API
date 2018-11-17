using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.JobMatchProfile;

namespace Employment.Web.Mvc.Service.Tests.JobMatchProfile
{
    /// <summary>
    /// Unit tests for <see cref="EmployerMapper" />.
    /// </summary>
    [TestClass]
    public class JobMatchProfileMapperTest
    {
        private JobMatchProfileMapper SystemUnderTest()
        {
            return new JobMatchProfileMapper();
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
