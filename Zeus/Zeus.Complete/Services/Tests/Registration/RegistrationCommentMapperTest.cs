using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.Registration;
using AutoMapper;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class RegistrationCommentMapperTest
    {
        private RegistrationCommentMapper SystemUnderTest()
        {
            return new RegistrationCommentMapper();
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
