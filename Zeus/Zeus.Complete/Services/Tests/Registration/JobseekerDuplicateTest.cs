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
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class JobseekerDuplicateTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<ILinkJobSeeker> mockRegWcf;

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
            mockAdwService = new Mock<IAdwService>();
            mockRegWcf = new Mock<ILinkJobSeeker>();
            mockClient.Setup(m => m.Create<ILinkJobSeeker>("LinkJobSeeker.svc")).Returns(mockRegWcf.Object);
        }

        /// <summary>
        /// Get the list of Duplicate Jobseeker
        /// </summary>
        [TestMethod]
        public void Get_Duplicate_Jobseeker_Successful_Test()
        {
            // Arrange
            mockMappingEngine.Setup(m => m.Map<LinkedJobSeekerRequest>(It.IsAny<long>())).Returns(new LinkedJobSeekerRequest { JobSeekerId = 3187026003, LinkType="DUPL" });
            mockRegWcf.Setup(m => m.GetLinkedList(It.IsAny<LinkedJobSeekerRequest>())).Returns(new LinkedJobSeekerResponse());
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<LinkedJobSeekerResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().GetDuplicateList((long)3187026003);
            //Assert
            mockRegWcf.Verify(m => m.GetLinkedList(It.IsAny<LinkedJobSeekerRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<LinkedJobSeekerRequest>((long)3187026003), Times.Once());
        }

        /// <summary>
        ///Get the list of Duplicate Jobseeker wcf method throw fault validation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_Duplicate_Jobseeker_throw_FaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            mockMappingEngine.Setup(m => m.Map<LinkedJobSeekerRequest>(It.IsAny<long>())).Returns(new LinkedJobSeekerRequest { JobSeekerId = 3187026003, LinkType = "DUPL" });
            mockRegWcf.Setup(m => m.GetLinkedList(It.IsAny<LinkedJobSeekerRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<LinkedJobSeekerResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().GetDuplicateList((long)3187026003);
        }

        /// <summary>
        /// Get the list of Duplicate Jobseeker wcf method throw fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Get_Duplicate_Jobseeker_throw_FaultException_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            mockMappingEngine.Setup(m => m.Map<LinkedJobSeekerRequest>(It.IsAny<long>())).Returns(new LinkedJobSeekerRequest { JobSeekerId = 3187026003, LinkType = "DUPL" });
            mockRegWcf.Setup(m => m.GetLinkedList(It.IsAny<LinkedJobSeekerRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<LinkedJobSeekerResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().GetDuplicateList((long)3187026003);
        }

    }
}
