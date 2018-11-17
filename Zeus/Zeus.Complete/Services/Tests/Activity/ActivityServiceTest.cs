using System.Web.Mvc;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Activity;
using Employment.Web.Mvc.Service.Interfaces.Activity;
using Employment.Esc.ActivityManagement.Contracts.ServiceContracts;
using Employment.Esc.ActivityManagement.Contracts.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Web.Mvc.Service.Interfaces.ServicesAdministration;

namespace Employment.Web.Mvc.Service.Tests.Activity
{
    [TestClass]
    public class ActivityServiceTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        /// <summary>
        /// WCF service
        /// </summary>
        private Mock<IActivityManagementService> mockActivityManagementWcf;

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

        private ActivityService SystemUnderTest()
        {
            return new ActivityService(mockClient.Object, MappingEngine, mockCacheService.Object);
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
            mockActivityManagementWcf = new Mock<IActivityManagementService>();
            mockClient.Setup(m => m.Create<IActivityManagementService>("ActivityManagement.svc")).Returns(mockActivityManagementWcf.Object);
         
        }


        #region Activity search test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void List_Valid()
        {
            //1. Setup data
            //a.request
            var request = ActivityTestDataHelper.CreateDummyActivityListModel();
            //b.response
            List<ACTList> actLists = new List<ACTList>();

            for (int i = 1; i < 10; i++)
            {
                actLists.Add(ActivityTestDataHelper.CreateDummyACTList(i));
            }

            var response = new ACTSearchResponse
            {
                MoreDataflag = "Y",
                NextActivityId = 123123,
                NextActivityStartDate = DateTime.Today,
                actLists = actLists.ToArray(),
            };

            mockActivityManagementWcf.Setup(m => m.GetActivityList(It.IsAny<ACTSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListActivities(request);
            
            //3. Verification
            //Verify More parameters
            Assert.AreEqual(true, result.HasMoreRecords);
            Assert.AreEqual(response.NextActivityId, result.StartActivityId);
            Assert.AreEqual(response.NextActivityStartDate, result.StartActivityStartDate);
            //Verify response list
            Assert.AreEqual(response.actLists.Length, result.Results.Count());
            
            //Verify behaviour
            mockActivityManagementWcf.Verify(m => m.GetActivityList(It.Is<ACTSearchRequest>(r=>r.SearchType==request.SearchType)), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test invalid search combination
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_RequestDataError_ThrowsServiceValidationException()
        {

            //1. Setup data
            //a.request
            var request = new ActivityListModel
            {
                SearchType = "ACTIVITYTYPE",
                ActivityType = "AETV",
            };

            //2. exec
            var result = SystemUnderTest().ListActivities(request);

            //3. Verification
        }

        /// <summary>
        /// Test failed ExecutionResult
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_FailedResponse_ThrowsServiceValidationException()
        {
            //1. Setup data
            //a.request
            var request = ActivityTestDataHelper.CreateDummyActivityListModel();

            //b.response
            var response = new ACTSearchResponse();
            response.ExecutionResult =ActivityTestDataHelper.CreateDummyFailedExecutionResult();

            mockActivityManagementWcf.Setup(m => m.GetActivityList(It.IsAny<ACTSearchRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListActivities(request);

            //3. Verification
        }

       

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = ActivityTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            var request = ActivityTestDataHelper.CreateDummyActivityListModel();

            mockActivityManagementWcf.Setup(m => m.GetActivityList(It.IsAny<ACTSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListActivities(request);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = ActivityTestDataHelper.CreateDummyFaultException();

             //1. Setup data
            //a.request
            var request = ActivityTestDataHelper.CreateDummyActivityListModel();

            mockActivityManagementWcf.Setup(m => m.GetActivityList(It.IsAny<ACTSearchRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().ListActivities(request);
        }
        #endregion
        #endregion

        #region Activity test
        [TestMethod]
        public void Get_Valid()
        {
            //1. Setup data
            //a.request
            long activityId = 12313123;
            //b.response
            var response = ActivityTestDataHelper.CreateDummyACTDetailsExResponse();

            mockActivityManagementWcf.Setup(m => m.GetActivityDetailsEx(It.IsAny<ACTDetailsRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().GetActivitity(activityId);

            //3. Verification
            //Verify More parameters
            Assert.AreEqual(activityId, result.ActivityId);
            Assert.AreEqual(response.ActivityName, result.ActivityName);
            //No need to verify other attributes, it is tested by the mapping tests
            
            //Verify behaviour
            mockActivityManagementWcf.Verify(m => m.GetActivityDetailsEx(It.Is<ACTDetailsRequest>(r => r.ActivityId == activityId)), Times.Once());
        }

        #region exceptions
        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = ActivityTestDataHelper.CreateDummyFaultExceptionValidationFault();

            //1. Setup data
            //a.request
            long activityId = 12313123;

            mockActivityManagementWcf.Setup(m => m.GetActivityDetailsEx(It.IsAny<ACTDetailsRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().GetActivitity(activityId);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = ActivityTestDataHelper.CreateDummyFaultException();

            //1. Setup data
            //a.request
            long activityId = 12313123;

            mockActivityManagementWcf.Setup(m => m.GetActivityDetailsEx(It.IsAny<ACTDetailsRequest>())).Throws(exception);

            //2. exec
            var result = SystemUnderTest().GetActivitity(activityId);
        }
        #endregion
        #endregion

        #region Contract search test
        /// <summary>
        /// Test Search runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void ContractList_Valid()
        {
            //1. Setup data
            //a.request
            string orgCode = "MYTU", siteCode = "IC00";

            //b.response
            List<ContractList> list = new List<ContractList>();

            var contract = new ContractList()
            {
                ContractId = "0206123B",
                ContractName = "SSC 4MTI AALL JOB FUTURES LTD",
                ContractType = "SSC"
                };
            list.Add(contract);

            contract = new ContractList()
                {
                    ContractId = "0207417B",
                    ContractName = "DES - DMS 4MTI AALL JOB FUTURES LTD",
                    ContractType = "DESA"
                };
            list.Add(contract);

            var response = new ContractListResponse
            {
                 MoreDataFlag="",
                 contractLists = list.ToArray(),
            };

            mockActivityManagementWcf.Setup(m => m.ContractList(It.IsAny<ContractListRequest>())).Returns(response);

            //2. exec
            var result = SystemUnderTest().ListContracts(orgCode,siteCode);

            //3. Verification
            //Verify response list
            Assert.AreEqual(response.contractLists.Length, result.Count());

            //Verify behaviour
            mockActivityManagementWcf.Verify(m => m.ContractList(It.Is<ContractListRequest>(r => r.OrgCode == orgCode)), Times.Once());
        }
        #endregion
    }
}