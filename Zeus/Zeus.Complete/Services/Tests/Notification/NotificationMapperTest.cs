using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Notification
{
    /// <summary>
    /// Unit tests for <see cref="NotificationMapper" />.
    /// </summary>
    [TestClass]
    public class NotificationMapperTest
    {
        private NotificationMapper SystemUnderTest()
        {
            return new NotificationMapper();
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