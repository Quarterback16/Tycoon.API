using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.JSCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Employment.Web.Mvc.Service.Tests.JSCI
{
    /// <summary>
    /// Unit tests for <see cref="JsciMapper" />.
    /// </summary>
    [TestClass]
    public class JsciMapperTest
    {
        private JsciMapper SystemUnderTest()
        {
            return new JsciMapper();
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