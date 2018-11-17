using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Referrals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Referrals
{
    /// <summary>
    /// Unit tests for <see cref="ReferralHistoryMapper" />.
    /// </summary>
    [TestClass]
    public class ReferralHistoryMapperTest
    {
        private ReferralHistoryMapper SystemUnderTest()
        {
            return new ReferralHistoryMapper();
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
