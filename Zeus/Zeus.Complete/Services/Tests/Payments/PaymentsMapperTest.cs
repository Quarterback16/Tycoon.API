using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.Payments;

namespace Employment.Web.Mvc.Service.Tests.Payments
{
    /// <summary>
    ///This is a test class for PaymentsMapperTest and is intended
    ///to contain all PaymentsMapperTest Unit Tests
    ///</summary>
    [TestClass]
    public class PaymentsMapperTest
    {
        public PaymentsMapper PaymentsMappingTest()
        {
          return new PaymentsMapper();            
        }

        /// <summary>
        ///A test for Map
        ///</summary>
        [TestMethod]
        public void IsMappingValid()
        {
            PaymentsMappingTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
