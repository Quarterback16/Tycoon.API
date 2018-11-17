using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.SmartClient.Provisioner.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Provisioner;
using Employment.Web.Mvc.Service.Interfaces.Provisioner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Provisioner
{
    public class ProvisionerServiceTestMapper : IMapper
    {
        public void Map(IProfileExpression mapper)
        {
            
        }
    }

    [TestClass]
    public class ProvisionerServiceTest
    {
        private ProvisionerService SystemUnderTest()
        {
            return new ProvisionerService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ProvisionerServiceTestMapper();
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
        private Mock<ISmartClientProvisioner> mockSmartClientProvisioner;

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

            mockSmartClientProvisioner = new Mock<ISmartClientProvisioner>();
            mockSmartClientProvisioner.Setup(s => s.EmulateAtNextLogon(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>())).Returns(true);
            mockClient.Setup(m => m.Create<ISmartClientProvisioner>("SmartClientProvisioner.svc")).Returns(mockSmartClientProvisioner.Object);            
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new ProvisionerService(null, null, null);
        }

        [TestMethod]
        public void EmulateAtNextLogon_Valid()
        {
            var model = new ProvisionerModel {UserId = "AB000012", Reason = "Reason", JobNumber = "1234567"};
            var result = SystemUnderTest().EmulateAtNextLogon(model);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void EmulateAtNextLogon_UserIdNotValid()
        {
            var model = new ProvisionerModel { UserId = null, Reason = "Reason", JobNumber = "1234567" };
            SystemUnderTest().EmulateAtNextLogon(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void EmulateAtNextLogon_ReasonNotValid()
        {
            var model = new ProvisionerModel { UserId = "AA123456", Reason = null, JobNumber = "" };
            SystemUnderTest().EmulateAtNextLogon(model);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void EmulateAtNextLogon_JobNumberNotValid()
        {
            var model = new ProvisionerModel { UserId = "AA123456", Reason = "", JobNumber = null };
            SystemUnderTest().EmulateAtNextLogon(model);
        }
    }
}
