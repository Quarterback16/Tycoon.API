using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Suspensions.Contracts.DataContracts;
using Employment.Esc.Suspensions.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Suspensions;
using Employment.Web.Mvc.Service.Interfaces.Suspensions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Suspensions
{
    /// <summary>
    /// Unit tests for <see cref="SuspensionsService" />.
    /// </summary>
    [TestClass]
    public class SuspensionsServiceTest
    {
        private SuspensionsService SystemUnderTest()
        {
            return new SuspensionsService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new SuspensionsMapper();
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
        private Mock<ISuspensions> mockSuspensionsWcf;

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
            mockSuspensionsWcf = new Mock<ISuspensions>();

            mockClient.Setup(m => m.Create<ISuspensions>("Suspensions.svc")).Returns(mockSuspensionsWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new SuspensionsService(null, null, null);
        }

        #region GetSuspensionList tests

        /// <summary>
        /// Test GetSuspensionList runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetSuspensionList_Valid()
        {
            var jobseekerID = 1;

            var response = new RJCPSuspensionListResponse
            {
                FirstGivenName = "firstname",
                List = new[] { new RJCPSuspensionListItem { SequenceNumber = 1 } },
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outJobseeker = MappingEngine.Map<JobseekerReferralModel>(response);
            var outList = MappingEngine.Map<IEnumerable<SuspensionModel>>(response.List);

            mockSuspensionsWcf.Setup(m => m.RJCPSuspensionList(It.IsAny<RJCPSuspensionListRequest>())).Returns(response);
            mockMappingEngine.Setup(m => m.Map<JobseekerReferralModel>(response)).Returns(outJobseeker);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<SuspensionModel>>(response.List)).Returns(outList);

            var result = SystemUnderTest().GetSuspensionList(jobseekerID);

            Assert.IsTrue(result.JobseekerReferralDetails.JobseekerID == jobseekerID);
            Assert.IsTrue(result.JobseekerReferralDetails.JobseekerFirstName == response.FirstGivenName);
            Assert.IsTrue(result.SuspensionsList.Count() == response.List.Count());
            Assert.IsTrue(result.SuspensionsList.First().SuspensionSequenceNumber == response.List.First().SequenceNumber);
            mockSuspensionsWcf.Verify(m => m.RJCPSuspensionList(It.IsAny<RJCPSuspensionListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<JobseekerReferralModel>(response), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<SuspensionModel>>(response.List), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSuspensionList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockSuspensionsWcf.Setup(m => m.RJCPSuspensionList(It.IsAny<RJCPSuspensionListRequest>())).Throws(exception);

            SystemUnderTest().GetSuspensionList(It.IsAny<long>());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSuspensionList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            mockSuspensionsWcf.Setup(m => m.RJCPSuspensionList(It.IsAny<RJCPSuspensionListRequest>())).Throws(exception);

            SystemUnderTest().GetSuspensionList(It.IsAny<long>());
        }

        #endregion
    }
}

