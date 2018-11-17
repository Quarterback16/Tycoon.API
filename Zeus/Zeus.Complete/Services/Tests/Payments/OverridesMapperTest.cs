using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Payments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    [TestClass]
    public class OverridesMapperTest
    {
        public OverridesMapper OverridesMappingTest()
        {
            return new OverridesMapper();
        }

        /// <summary>
        ///A test to ensure the mapping configurations
        ///</summary>
        [TestMethod]
        public void IsMappingValid()
        {
            OverridesMappingTest().Map(Mapper.Configuration);
            Mapper.AssertConfigurationIsValid();
        }
    }
}
