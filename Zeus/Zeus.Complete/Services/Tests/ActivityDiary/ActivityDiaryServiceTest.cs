using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.ActivityManagement.Contracts.DataContracts;
using Employment.Esc.ActivityManagement.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Activity;
using Employment.Web.Mvc.Service.Interfaces.Activity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.ActivityDiary
{
    [TestClass]
    public class ActivityDiaryServiceTest
    {
        #region Core

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;

        /// <summary>
        /// WCF service
        /// </summary>
        private Mock<IAttendanceService> mockAttendanceWcf;

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

        private ActivityDiaryService SystemUnderTest()
        {
            return new ActivityDiaryService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            //WCF services
            mockAttendanceWcf = new Mock<IAttendanceService>();
            mockClient.Setup(m => m.Create<IAttendanceService>("AttendanceService.svc")).Returns(mockAttendanceWcf.Object);

        }

        #endregion

        #region GetActivityDiaryList

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetActivityDiaryList_Basic()
        {
            ActivityDiaryListModel request = new ActivityDiaryListModel(){ActivityPlacementStatus = "A"};

            var response = new AASearchResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SearchActivityAttendanceList(It.IsAny<AASearchRequest>())).Returns(response);

            var result = SystemUnderTest().GetActivityDiaryList(request);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetActivityDiaryList_Error()
        {
            ActivityDiaryListModel request = new ActivityDiaryListModel() { ActivityPlacementStatus = "A" };

            var response = new AASearchResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SearchActivityAttendanceList(It.IsAny<AASearchRequest>())).Throws(new FaultException());

            var result = SystemUnderTest().GetActivityDiaryList(request);

            Assert.IsNotNull(result);
        }

        #endregion

        #region GetActivityDiaryDetails

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetActivityDiaryDetails_Basic()
        {
            ActivityDiaryDetailsModel request = new ActivityDiaryDetailsModel(){JobSeekerId = 12345678, ActivityId = 987654321, ActivityPlacementSequenceNo = 1, FromDate = DateTime.Now};

            var response = new AADetailsResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.GetActivityAttendanceList(It.IsAny<AADetailsRequest>())).Returns(response);

            var result = SystemUnderTest().GetActivityDiaryDetails(request);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Search throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetActivityDiaryDetails_Error()
        {
            ActivityDiaryDetailsModel request = new ActivityDiaryDetailsModel() { JobSeekerId = 12345678, ActivityId = 987654321, ActivityPlacementSequenceNo = 1, FromDate = DateTime.Now };

            var response = new AADetailsResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.GetActivityAttendanceList(It.IsAny<AADetailsRequest>())).Throws(new FaultException());

            var result = SystemUnderTest().GetActivityDiaryDetails(request);

            Assert.IsNotNull(result);
        }

        #endregion

        #region SaveActivityDiaryDetails

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SaveActivityDiaryDetails_Basic()
        {
            ActivityDiaryDetailsModel request = new ActivityDiaryDetailsModel(){JobSeekerId = 123456789, ActivityId = 987654321, ActivityPlacementSequenceNo = 1};

            var response = new AASaveResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SaveAttendances(It.IsAny<AASaveRequest>())).Returns(response);

            var result = SystemUnderTest().SaveActivityDiaryDetails(request);
        
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveActivityDiaryDetails_Error()
        {
            ActivityDiaryDetailsModel request = new ActivityDiaryDetailsModel() { JobseekerId = 123456789, ActivityId = 987654321, ActivityPlacementSequenceNo = 1 };

            var response = new AASaveResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SaveAttendances(It.IsAny<AASaveRequest>())).Throws(new FaultException());

            var result = SystemUnderTest().SaveActivityDiaryDetails(request);

            Assert.IsNotNull(result);
        }

        #endregion

        #region GetActivityDiaryBulkDetails

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetActivityDiaryBulkDetails_Basic()
        {
            ActivityDiaryDetailsBulkModel request = new ActivityDiaryDetailsBulkModel(){FromDate = DateTime.Now };

            var response = new AADetailsBulkResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.GetActivityAttendanceListBulk(It.IsAny<AADetailsBulkRequest>())).Returns(response);

            var result = SystemUnderTest().GetActivityDiaryBulkDetails(request);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetActivityDiaryBulkDetails_Error()
        {
            ActivityDiaryDetailsBulkModel request = new ActivityDiaryDetailsBulkModel() { FromDate = DateTime.Now };

            var response = new AADetailsBulkResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.GetActivityAttendanceListBulk(It.IsAny<AADetailsBulkRequest>())).Throws(new FaultException());

            var result = SystemUnderTest().GetActivityDiaryBulkDetails(request);

            Assert.IsNotNull(result);
        }

        #endregion

        #region SaveActivityDiaryBulkDetails

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void SaveActivityDiaryBulkDetails_Basic()
        {
            ActivityDiaryDetailsBulkModel request = new ActivityDiaryDetailsBulkModel(){ActivityId = 987654321, OverrideFlag = "N", StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now, Attendances = new List<AttendanceModel>()};

            var response = new AASaveBulkResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SaveAttendancesBulk(It.IsAny<AASaveBulkRequest>())).Returns(response);

            var result = SystemUnderTest().SaveActivityDiaryBulkDetails(request);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SaveActivityDiaryBulkDetails_Error()
        {
            ActivityDiaryDetailsBulkModel request = new ActivityDiaryDetailsBulkModel() { ActivityId = 987654321, OverrideFlag = "N", StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now, Attendances = new List<AttendanceModel>() };

            var response = new AASaveBulkResponse
            {
            };

            mockAttendanceWcf.Setup(m => m.SaveAttendancesBulk(It.IsAny<AASaveBulkRequest>())).Throws(new FaultException());

            var result = SystemUnderTest().SaveActivityDiaryBulkDetails(request);

            Assert.IsNotNull(result);
        }

        #endregion
    }
}
