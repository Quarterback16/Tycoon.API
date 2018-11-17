using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Case;

namespace Employment.Web.Mvc.Service.Tests.Case
{
    /// <summary>
    /// Summary description for CaseSummaryMapperTest
    /// </summary>
    [TestClass]
    public class CaseSummaryMapperTest
    {
        private CaseSummaryMapper SystemUnderTest()
        {
            return new CaseSummaryMapper();
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
