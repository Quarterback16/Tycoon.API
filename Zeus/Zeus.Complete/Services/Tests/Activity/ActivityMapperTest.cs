using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Service.Implementation.Activity;
using Employment.Web.Mvc.Service.Interfaces.Activity;
using Employment.Esc.ActivityManagement.Contracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Employment.Web.Mvc.Service.Tests.Activity
{
    /// <summary>
    ///This is a test class for ActivityMapperTest and is intended
    ///to contain all ActivityMapperTest Unit Tests
    ///</summary>
    [TestClass]
    public class ActivityMapperTest
    {
        public ActivityMapper SystemUnderTest()
        {
            return new ActivityMapper();
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ActivityMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        /// <summary>
        ///A test for Mappping configuration
        ///</summary>
        [TestMethod]
        public void IsMappingValid()
        {
            SystemUnderTest().Map(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
        
        #region Tests related to Activity
        [TestMethod]
        public void ActivitySearchModelToACTSearchRequest_MappingTest_Valid()
        {
            //1. setup data
            var source = ActivityTestDataHelper.CreateDummyActivityListModel();

            //2. Exec
            var dest = MappingEngine.Map<ACTSearchRequest>(source);

            //3. Verification
            Assert.AreEqual(source.SearchType, dest.SearchType);
            Assert.AreEqual(source.ActivityType, dest.ActivityType);
            Assert.AreEqual(source.SubType, dest.SubType);
            Assert.AreEqual(source.OrgCode, dest.OrgCode);
            Assert.AreEqual(source.SiteCode, dest.SiteCode);
            Assert.AreEqual(source.ESACode, dest.ESACode);
            Assert.AreEqual(source.Status, dest.ActivityStatus);
            Assert.AreEqual(source.FromPostCode, dest.FromPostCode);
            Assert.AreEqual(source.StartActivityId, dest.StartActivityId);
            Assert.AreEqual(source.StartActivityStartDate, dest.StartActivityStartDate);
        }

        [TestMethod]
        public void ACTSearchResponseToActivitySearchModel_MappingTest_Valid()
        {
            //1. setup data
            List<ACTList> actLists = new List<ACTList>();

            for (int i = 1; i < 10; i++)
            {
                actLists.Add(ActivityTestDataHelper.CreateDummyACTList(i));
            }

            var source = new ACTSearchResponse
            {
                MoreDataflag = "Y",
                NextActivityId = 123123,
                NextActivityStartDate = DateTime.Today,
                actLists = actLists.ToArray(),
            };

            //2. Exec
            var dest = MappingEngine.Map<ActivityListModel>(source);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, dest.HasMoreRecords);
            Assert.AreEqual(source.NextActivityId, dest.StartActivityId);
            Assert.AreEqual(source.NextActivityStartDate, dest.StartActivityStartDate);
            //Verify response list
            Assert.AreEqual(source.actLists.Length, dest.Results.Count());
        }

        [TestMethod]
        public void ACTListToActivityModel_MappingTest_Valid()
        {
            //1. setup data
            var source =ActivityTestDataHelper.CreateDummyACTList(8);

            //2. Exec
            var dest = MappingEngine.Map<ActivityModel>(source);

            //3. Verification
            Assert.AreEqual(source.ActivityId, dest.ActivityId);
            Assert.AreEqual(source.TitleText, dest.ActivityName);
        }

         [TestMethod]
        public void ACTDetailsExtResponseToActivityModel_MappingTest_Valid()
        {
            //1. setup data
            var source = ActivityTestDataHelper.CreateDummyACTDetailsExResponse();

            //2. Exec
            var dest = MappingEngine.Map<ActivityModel>(source);

            //3. Verification
            Assert.AreEqual(source.ActivityName, dest.ActivityName);
            Assert.AreEqual(source.ActivityDescription, dest.ActivityDescription);
            Assert.AreEqual(source.ActivityType, dest.ActivityType);
            Assert.AreEqual(source.SubType, dest.SubType);
            Assert.AreEqual(source.OrgCode, dest.OrgCode);
            Assert.AreEqual(source.SiteCode, dest.SiteCode);
            Assert.AreEqual(source.StartDate, dest.StartDate);
            Assert.AreEqual(source.EndDate, dest.EndDate);
            Assert.AreEqual(source.Status, dest.Status);
            Assert.AreEqual(source.IndustryCode.Length, dest.IndustryCodes.Length);
            Assert.AreEqual(source.RelatedActivityIds.Length, dest.RelatedActivityIds.Length);
             //flag
            Assert.AreEqual(source.IntegrityControlNumber, dest.IntegrityControlNumber);
             //Check location
            Assert.AreEqual(source.LocOrganisationName, dest.RelatedInfo.Location.HostName);
             //Check host
            Assert.AreEqual(source.HostOrganisationName, dest.RelatedInfo.HostLink.Host.HostName);

        }


         [TestMethod]
         public void ACTDetailsExtResponseToActivityModel_MappingTest_WithoutHostAndLocation()
         {
             //1. setup data
             var source = ActivityTestDataHelper.CreateDummyACTDetailsExResponse();
             source.LocSeqNum = 0;
             source.HostId = 0;
             //2. Exec
             var dest = MappingEngine.Map<ActivityModel>(source);

             //3. Verification
             //Check location
             Assert.IsNull(dest.RelatedInfo.Location);
             //Check host
             Assert.IsNull(dest.RelatedInfo.HostLink);

         }
        #endregion
    }
}
