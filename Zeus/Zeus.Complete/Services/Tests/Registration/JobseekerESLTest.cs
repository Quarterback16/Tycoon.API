using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Esc.Registration.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Employment.Esc.Registration.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class JobseekerESLTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IESLHistory> mockRegWcf;

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
            return new RegistrationService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object,mockAdwService.Object);
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
            mockRegWcf = new Mock<IESLHistory>();
            mockAdwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IESLHistory>("JobSeekerESLHistory.svc")).Returns(mockRegWcf.Object);
        }

        /// <summary>
        /// ESL history successful test.
        /// </summary>
        [TestMethod]
        public void ESL_History_Successful_Test()
        {
            // Arrange
            var eslModel = new EarlySchoolLeaverModel {AdvisedDate = DateTime.Now , EducationLevel ="Year 12 attended" ,EventDate = DateTime.Now.AddMonths(11) , Y12 = "Not Varified" };
            mockMappingEngine.Setup(m => m.Map<ESLHistoryRequest>(It.IsAny<long>())).Returns(new ESLHistoryRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<ESLHistoryRequest>())).Returns(new ESLHistoryResponse());
            mockMappingEngine.Setup(m => m.Map<IEnumerable<EarlySchoolLeaverModel>>(It.IsAny<IEnumerable<ESLHistoryItem>>())).Returns(new List<EarlySchoolLeaverModel> { eslModel });
            //Act
            var result = SystemUnderTest().ReadESLHistory((long)3187026003);
            //Assert
            mockRegWcf.Verify(m => m.List(It.IsAny<ESLHistoryRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<ESLHistoryRequest>((long)3187026003), Times.Once());
        }

        /// <summary>
        /// ESL history wcf method throw fault validation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ESL_History_throw_FaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            var eslModel = new EarlySchoolLeaverModel { AdvisedDate = DateTime.Now, EducationLevel = "Year 12 attended", EventDate = DateTime.Now.AddMonths(11), Y12 = "Not Varified" };
            mockMappingEngine.Setup(m => m.Map<ESLHistoryRequest>(It.IsAny<long>())).Returns(new ESLHistoryRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<ESLHistoryRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<EarlySchoolLeaverModel>>(It.IsAny<IEnumerable<ESLHistoryItem>>())).Returns(new List<EarlySchoolLeaverModel> { eslModel });
            //Act
            var result = SystemUnderTest().ReadESLHistory((long)3187026003);
        }

        /// <summary>
        /// ESL history wcf method throw fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ESL_History_throw_FaultException_ThrowsServiceValidationException()
        {
            // Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            var eslModel = new EarlySchoolLeaverModel { AdvisedDate = DateTime.Now, EducationLevel = "Year 12 attended", EventDate = DateTime.Now.AddMonths(11), Y12 = "Not Varified" };
            mockMappingEngine.Setup(m => m.Map<ESLHistoryRequest>(It.IsAny<long>())).Returns(new ESLHistoryRequest());
            mockRegWcf.Setup(m => m.List(It.IsAny<ESLHistoryRequest>())).Throws(exception);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<EarlySchoolLeaverModel>>(It.IsAny<IEnumerable<ESLHistoryItem>>())).Returns(new List<EarlySchoolLeaverModel> { eslModel });
            //Act
            var result = SystemUnderTest().ReadESLHistory((long)3187026003);
        }

    }
}
