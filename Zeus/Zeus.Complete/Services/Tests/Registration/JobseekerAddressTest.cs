using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Esc.Registration.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Esc.Registration.Contracts.DataContracts;
using Employment.Web.Mvc.Service.Interfaces.Common;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using System.Collections;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class JobseekerAddressTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IJobSeekerAddress> mockRegWcf;

        private TestContext testContextInstance;

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new RegistrationMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private RegistrationService SystemUnderTest()
        {
            return new RegistrationService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, mockAdwService.Object);
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
            mockRegWcf = new Mock<IJobSeekerAddress>();
            mockAdwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IJobSeekerAddress>("JobSeekerAddress.svc")).Returns(mockRegWcf.Object);
        }

        /// <summary>
        /// Address_s the history_ successful_ test.
        /// </summary>
        [TestMethod]
        public void Address_History_Successful_Test()
        {
            // Arrange
            var addressModel = new AddressModel { AddressLine1 = "109 PRINCE EDWARD ST"};
            mockMappingEngine.Setup(m => m.Map<JobSeekerAddressListRequest>(It.IsAny<long>())).Returns(new JobSeekerAddressListRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<JobSeekerAddressListRequest>())).Returns(new JobSeekerAddressListResponse());
            mockMappingEngine.Setup(m => m.Map<IEnumerable<AddressModel>>(It.IsAny<IEnumerable<AddressItem>>())).Returns(new List<AddressModel> { addressModel });
            //Act
            var result = SystemUnderTest().ReadAddressHistory((long)3187026003);
            //Assert
            mockRegWcf.Verify(m => m.List(It.IsAny<JobSeekerAddressListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<JobSeekerAddressListRequest>((long)3187026003), Times.Once());
        }

        /// <summary>
        /// Address history wcf method throw fault validation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Address_History_throw_FaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            var addressModel = new AddressModel { AddressLine1 = "109 PRINCE EDWARD ST" };
            mockMappingEngine.Setup(m => m.Map<JobSeekerAddressListRequest>(It.IsAny<long>())).Returns(new JobSeekerAddressListRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<JobSeekerAddressListRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<AddressModel>>(It.IsAny<IEnumerable<AddressItem>>())).Returns(new List<AddressModel> { addressModel });
            //Act
            var result = SystemUnderTest().ReadAddressHistory((long)3187026003);
        }

        /// <summary>
        /// Address history wcf method throw fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Address_History_throw_FaultException_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            var addressModel = new AddressModel { AddressLine1 = "109 PRINCE EDWARD ST" };
            mockMappingEngine.Setup(m => m.Map<JobSeekerAddressListRequest>(It.IsAny<long>())).Returns(new JobSeekerAddressListRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<JobSeekerAddressListRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<AddressModel>>(It.IsAny<IEnumerable<AddressItem>>())).Returns(new List<AddressModel> { addressModel });
            //Act
            var result = SystemUnderTest().ReadAddressHistory((long)3187026003);
        }


    }
}
