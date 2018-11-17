using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Referrals;
using Employment.Web.Mvc.Service.Interfaces.Referrals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Referrals
{
    /// <summary>
    /// Unit tests for <see cref="ReferralMapper" />.
    /// </summary>
    [TestClass]
    public class ReferralMapperTest
    {
        private ReferralMapper SystemUnderTest()
        {
            return new ReferralMapper();
        }

        /// <summary>
        /// Test map is valid.
        /// </summary>
        [TestMethod]
        public void MapperValid()
        {
            SystemUnderTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}