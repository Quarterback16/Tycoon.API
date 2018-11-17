using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.Case;
using AutoMapper;

namespace Employment.Web.Mvc.Service.Tests.Case
{
    /// <summary>
    /// Summary description for CaseLoadMapperTest
    /// </summary>
    [TestClass]
    public class CaseLoadMapperTest
    {
        private CaseLoadMapper SystemUnderTest()
        {
            return new CaseLoadMapper();
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
