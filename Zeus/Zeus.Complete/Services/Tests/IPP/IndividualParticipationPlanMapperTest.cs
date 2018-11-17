using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.IndividualParticipationPlan;

namespace Employment.Web.Mvc.Service.Tests.IPP
{    /// <summary>
    /// Summary description for IndividualParticipationPlanMapperTest
    /// </summary>
    [TestClass]
    public class IndividualParticipationPlanMapperTest
    {
        private IndividualParticipationPlanMapper SystemUnderTest()
        {
            return new IndividualParticipationPlanMapper();
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
