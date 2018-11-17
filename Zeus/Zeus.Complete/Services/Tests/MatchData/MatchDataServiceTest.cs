using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Service.Implementation.MatchData;
using Employment.Esc.MatchData.Contracts.ServiceContracts;
using Employment.Esc.MatchData.Contracts.DataContracts;

namespace Employment.Web.Mvc.Service.Tests.MatchData
{
    /// <summary>
    /// Unit tests for <see cref="EmployerService" />.
    /// </summary>
    [TestClass]
    public class MatchDataServiceTest
    {
        private MatchDataService SystemUnderTest()
        {
            return new MatchDataService(mockClient.Object, MappingEngine, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new MatchDataMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IMatchData> mockResumeService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockResumeService = new Mock<IMatchData>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            mockClient.Setup(m => m.Create<IMatchData>("MatchData.svc")).Returns(mockResumeService.Object);
        }

        private ExecutionResult SuccessResult()
        {
            return new ExecutionResult() { Status = ExecuteStatus.Success };
        }

        private FaultException FaultException()
        {
            return new FaultException();
        }

        private FaultException<ValidationFault> ValidationFaultException()
        {
            return new FaultException<ValidationFault>(new ValidationFault(new ValidationDetail[] { new ValidationDetail() { Key = "A", Message = "B", Tag = "C" } }));
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            var service = new MatchDataService(null, null, null);
        }

        #region GetJobMatchLocations

        [TestMethod]
        public void GetJobMatchLocations_Successful()
        {
            mockResumeService.Setup(m => m.GetLocation(It.Is<LocationGetRequest>(i => i.LocationCode == "A")))
                .Returns(new LocationGetResponse() { ExecutionResult = SuccessResult(), Locations = new[] { 
                    new MatchLocationItem() { LocationName = "Loc1", MatchingKey = "A" }, 
                    new MatchLocationItem() { LocationName = "Loc2", MatchingKey = "B" } 
                } });
            var service = SystemUnderTest();
            service.GetJobMatchLocations("A");

            mockResumeService.Verify(m => m.GetLocation(It.Is<LocationGetRequest>(i => i.LocationCode == "A")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetJobMatchLocations_FaultException_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GetLocation(It.IsAny<LocationGetRequest>())).Throws(FaultException());

            var service = SystemUnderTest();
            service.GetJobMatchLocations("A");
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetJobMatchLocations_FaultException2_ThrowsServiceValidationException()
        {
            mockResumeService.Setup(m => m.GetLocation(It.IsAny<LocationGetRequest>())).Throws(ValidationFaultException());

            var service = SystemUnderTest();
            service.GetJobMatchLocations("A");
        } 
        #endregion
    }
}
