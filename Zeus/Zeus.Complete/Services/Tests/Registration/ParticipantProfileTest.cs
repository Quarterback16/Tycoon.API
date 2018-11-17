using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Moq;
using AutoMapper;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Esc.ParticipationProfile.Contracts.ServiceContracts;
using Employment.Esc.ParticipationProfile.Contracts.DataContracts;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Web.Mvc.Infrastructure.Exceptions;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class ParticipantProfileTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IParticipationProfile> mockRegWcf;

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

        /// <summary>
        /// Systems the under test.
        /// </summary>
        /// <returns></returns>
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
            mockRegWcf = new Mock<IParticipationProfile>();
            mockAdwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IParticipationProfile>("ParticipationProfileService.svc")).Returns(mockRegWcf.Object);
        }


        /// <summary>
        /// test the successful test of participant profile read. 
        /// </summary>
        [TestMethod]
        public void Read_Participant_Profile_WCF_service_successful_Test()
        {
            //Arrange
            mockMappingEngine.Setup(m => m.Map<ReadRequest>(It.IsAny<long>())).Returns(new ReadRequest { JobseekerID = 4698653309, IsCached =false  });
            mockRegWcf.Setup(m => m.Read(It.IsAny<ReadRequest>())).Returns(new ReadResponse());
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<ReadResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().ReadParticipantProfile((long)4698653309);
            //Assert
            mockRegWcf.Verify(m => m.Read(It.IsAny<ReadRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<ReadRequest>((long)4698653309), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Read_Participant_Profile_Wcf_Service_throw__FaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            mockMappingEngine.Setup(m => m.Map<ReadRequest>(It.IsAny<long>())).Returns(new ReadRequest { JobseekerID = 4698653309, IsCached = false });
            mockRegWcf.Setup(m => m.Read(It.IsAny<ReadRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<ReadResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().ReadParticipantProfile((long)4698653309);

        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Read_Participant_Profile_WCf_Service_throw__FaultException_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            mockMappingEngine.Setup(m => m.Map<ReadRequest>(It.IsAny<long>())).Returns(new ReadRequest { JobseekerID = 4698653309, IsCached = false });
            mockRegWcf.Setup(m => m.Read(It.IsAny<ReadRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<ReadResponse>())).Returns(new JobseekerModel());
            //Act
            var result = SystemUnderTest().ReadParticipantProfile((long)4698653309);

        }


    }
}
