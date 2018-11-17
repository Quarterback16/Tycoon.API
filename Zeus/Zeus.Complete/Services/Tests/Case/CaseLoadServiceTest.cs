using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.CaseloadSearch.Contracts.DataContracts;
using Employment.Esc.CaseloadSearch.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Case;
using Employment.Web.Mvc.Service.Interfaces.Case;
using Microsoft.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Case
{
    /// <summary>
    /// Summary description for CaseLoadControllerTest
    /// </summary>
    [TestClass]
    public class CaseLoadServiceTest
    {

        #region Core

        public CaseLoadServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private CaseLoadService SystemUnderTest()
        {
            return new CaseLoadService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }
        private IMappingEngine mappingEngine;
        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new CaseLoadMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        //      private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<ICaseloadSearch> mockCaseLoadWcf;
        private Mock<ISessionService> mockSessionService;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClaimsIdentity> mockIdentity;
        private readonly ExecutionResult mSuccessResult = new ExecutionResult { Status = ExecuteStatus.Success };

        /// <summary>
        /// Use TestInitialize to run code before running each test
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockCaseLoadWcf = new Mock<ICaseloadSearch>();
            mockSessionService = new Mock<ISessionService>();
            mockUserService.SetupGet(u => u.Session).Returns(mockSessionService.Object);
            mockClient.Setup(m => m.Create<ICaseloadSearch>("CaseLoadSearch.svc")).Returns(mockCaseLoadWcf.Object);
            mockIdentity = new Mock<IClaimsIdentity>();
            mockIdentity.SetupGet(id => id.Name).Returns("JT2554");
            mockUserService.SetupGet(us => us.Identity).Returns(mockIdentity.Object);

            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        #endregion 

        #region Search Tests

        [TestMethod]
        public void SearchTest_Basic()
        {
            var dummyResponse = new ListRemoteResponse()
                               {
                                   CaseloadList = new []{new CaseloadItemRemote()
                                                             {
                                                                 ActualEndDate = DateTime.Now, FirstName = "Mickey", Age = 50, CommencementDate = DateTime.Now, Surname = "Mouse" 
                                                             }}
                               };

            var request = new CaseLoadListModel() {OrgCode = "HOST", SiteCode = "QM60"};

            mockCaseLoadWcf.Setup(m => m.GetCaseloadListRemote(It.IsAny<ListRemoteRequest>())).Returns(dummyResponse);

            var response = SystemUnderTest().GetCaseLoad(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Results != null);
            Assert.IsTrue(response.Results.Count == 1);
            Assert.IsNotNull(response.Results[0].FirstName == "Mickey");
            Assert.IsNotNull(response.Results[0].Surname == "Mouse");
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchTest_WithFaultException()
        {
            var request = new CaseLoadListModel() { OrgCode = "HOST", SiteCode = "QM60" };

            mockCaseLoadWcf.Setup(m => m.GetCaseloadListRemote(It.IsAny<ListRemoteRequest>())).Throws(new FaultException());

            var response = SystemUnderTest().GetCaseLoad(request);
        }

        #endregion

        #region Assign Tests

        [TestMethod]
        public void AssignTest_Basic()
        {
            var dummyResponse = new UpdateResponse();

            var request = new AssignJobseekerListModel() {ClientNote = "Hello", ManagedBy = "AB1234_d", Jobseekers = new List<AssignJobseekerItemModel>()};

            mockCaseLoadWcf.Setup(m => m.UpdateManagedByClientNotes(It.IsAny<UpdateRequest>())).Returns(dummyResponse);

            var response = SystemUnderTest().AssignJobseekers(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AssignTest_WithFaultException()
        {
            var request = new AssignJobseekerListModel() { ClientNote = "Hello", ManagedBy = "AB1234_d", Jobseekers = new List<AssignJobseekerItemModel>() };

            mockCaseLoadWcf.Setup(m => m.UpdateManagedByClientNotes(It.IsAny<UpdateRequest>())).Throws(new FaultException());

            var response = SystemUnderTest().AssignJobseekers(request);

            Assert.IsNotNull(response);
        }
        #endregion
    }
}
