using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Moq;
using AutoMapper;
using Employment.Esc.EligibilityDeterminator.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Esc.EligibilityDeterminator.Contracts.DataContracts;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Web.Mvc.Infrastructure.Exceptions;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class EligibilityTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IEligibilityDeterminator> mockRegWcf;

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
            mockRegWcf = new Mock<IEligibilityDeterminator>();
            mockAdwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IEligibilityDeterminator>("EligibilityDeterminator.svc")).Returns(mockRegWcf.Object);
        }

        ///// <summary>
        ///// successful test of jobseeker eligibility.
        ///// </summary>
        //[TestMethod]
        //public void Eligibility_successful_test()
        //{
        //    //Arrange
        //    mockMappingEngine.Setup(m => m.Map<EligibilityRequest>(It.IsAny<long>())).Returns(new EligibilityRequest { JobSeekerId = 4698653309, IsVerbose = "N", EligType = " " });
        //    mockRegWcf.Setup(m => m.Get(It.IsAny<EligibilityRequest>())).Returns(new EligibilityResponse());
        //    mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<EligibilityResponse>())).Returns(new JobseekerModel());
        //    //Act
        //    var result = SystemUnderTest().GetEligibility((long)4698653309);
        //    //Assert
        //    mockRegWcf.Verify(m => m.Get(It.IsAny<EligibilityRequest>()), Times.Once());
        //    mockMappingEngine.Verify(m => m.Map<EligibilityRequest>((long)4698653309), Times.Once());
        //}

        ///// <summary>
        ///// Get_s the eligibility_throw_ fault exception validation fault_ throws service validation exception.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void Get_Eligibility_throw_FaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    // Arrange
        //    var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
        //    mockMappingEngine.Setup(m => m.Map<EligibilityRequest>(It.IsAny<long>())).Returns(new EligibilityRequest { JobSeekerId = 4698653309, IsVerbose = "N", EligType = " " });
        //    mockRegWcf.Setup(m => m.Get(It.IsAny<EligibilityRequest>())).Throws(exception);
        //    mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<EligibilityResponse>())).Returns(new JobseekerModel());
        //    //Act
        //    var result = SystemUnderTest().GetEligibility((long)4698653309);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void Get_Eligibility_WCF_Service_throw_FaultException_ThrowsServiceValidationException()
        //{
        //    // Arrange
        //    var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
        //    mockMappingEngine.Setup(m => m.Map<EligibilityRequest>(It.IsAny<long>())).Returns(new EligibilityRequest { JobSeekerId = 4698653309, IsVerbose = "N", EligType = " " });
        //    mockRegWcf.Setup(m => m.Get(It.IsAny<EligibilityRequest>())).Throws(exception);
        //    mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<EligibilityResponse>())).Returns(new JobseekerModel());
        //    //Act
        //    var result = SystemUnderTest().GetEligibility((long)4698653309);
        //}


    }
}
