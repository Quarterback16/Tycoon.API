using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Resume;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.Resume
{
    /// <summary>
    /// Unit tests for <see cref="EmployerMapper" />.
    /// </summary>
    [TestClass]
    public class ResumeMapperTest
    {
        private ResumeMapper SystemUnderTest()
        {
            return new ResumeMapper();
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
