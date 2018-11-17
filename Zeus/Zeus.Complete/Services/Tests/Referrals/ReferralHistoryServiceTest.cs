using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.ReferralHistory.Contracts.DataContracts;
using Employment.Esc.ReferralHistory.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Referrals;
using Employment.Web.Mvc.Service.Interfaces.Referrals;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Referrals
{
    /// <summary>
    /// Unit tests for <see cref="ReferralHistoryService" />.
    /// </summary>
    [TestClass]
    public class ReferralHistoryServiceTest
    {
        private ReferralHistoryService SystemUnderTest()
        {
            return new ReferralHistoryService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, mockAdwService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ReferralHistoryMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;
        private Mock<IReferralHistory> mockReferralHistoryWcf;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();

            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
            mockReferralHistoryWcf = new Mock<IReferralHistory>();

            mockClient.Setup(m => m.Create<IReferralHistory>("ReferralHistory.svc")).Returns(mockReferralHistoryWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new ReferralHistoryService(null, null, null, null);
        }

        #region GetList tests

        /// <summary>
        /// Test GetList runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetList_Valid()
        {
            var jobseekerID = 1;
            var searchType = ReferralHistoryItemType.All;

            var request = new GetListRequest { JobseekerId = jobseekerID, SearchType = searchType.GetFlag() };
            var response = new GetListResponse
            {
                FirstGivenName = "firstname",
                ListItems = new[] { new ListItem { Type = "A", DisSiteCode = "site" } },
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outModel = MappingEngine.Map<ReferralHistoryModel>(response);
            
            mockUserService.Setup(m => m.Roles).Returns(new[] { "DAD" });
            mockMappingEngine.Setup(m => m.Map<ReferralHistoryModel>(response)).Returns(outModel);
            mockReferralHistoryWcf.Setup(m => m.GetList(It.IsAny<GetListRequest>())).Returns(response);

            var result = SystemUnderTest().GetList(jobseekerID, searchType);

            Assert.IsTrue(result.JobseekerID == jobseekerID);
            Assert.IsTrue(result.JobseekerFirstName == outModel.JobseekerFirstName);
            Assert.IsTrue(result.List.Count() == outModel.List.Count());
            Assert.IsTrue(result.List.First().Site == outModel.List.First().Site);
            mockReferralHistoryWcf.Verify(m => m.GetList(It.IsAny<GetListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<ReferralHistoryModel>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockReferralHistoryWcf.Setup(m => m.GetList(It.IsAny<GetListRequest>())).Throws(exception);

            SystemUnderTest().GetList(1, ReferralHistoryItemType.All);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            mockReferralHistoryWcf.Setup(m => m.GetList(It.IsAny<GetListRequest>())).Throws(exception);

            SystemUnderTest().GetList(1, ReferralHistoryItemType.All);
        }

        #endregion
    }
}
