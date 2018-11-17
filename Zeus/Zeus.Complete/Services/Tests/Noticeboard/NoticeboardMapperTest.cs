using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Noticeboard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Noticeboard
{
    /// <summary>
    /// Unit tests for <see cref="NoticeboardMapper" />.
    /// </summary>
    [TestClass]
    public class NoticeboardMapperTest
    {
        private NoticeboardMapper SystemUnderTest()
        {
            return new NoticeboardMapper();
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
