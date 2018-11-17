using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Diary
{
    [TestClass]
    public class DiaryMapperTest
    {
        private DiaryMapper SystemUnderTest()
        {
            return new DiaryMapper();
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