using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.HelpDeskNotification.Contracts.DataContracts;
using Employment.Esc.HelpDeskNotification.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Service.Implementation.HelpDesk;
using Employment.Web.Mvc.Service.Interfaces.HelpDesk;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.HelpDesk
{
    /// <summary>
    /// Unit tests for <see cref="HelpDeskService" />.
    /// </summary>
    [TestClass]
    public class HelpDeskServiceTest
    {
        private HelpDeskService SystemUnderTest()
        {
            return new HelpDeskService(mockAdwService.Object, mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new HelpDeskMapper();
                    mapper.Map(Mapper.Configuration);

                    var adwMapper = new AdwMapper();
                    adwMapper.Map(Mapper.Configuration);

                    var stringMapper = new StringMapper();
                    stringMapper.Map(Mapper.Configuration);

                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IHelpDeskNotification> mockHelpDeskWcf;
        private Mock<IContainerProvider> mockContainerProvider;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockAdwService = new Mock<IAdwService>();
            mockClient = new Mock<IClient>();
            mockMappingEngine = new Mock<IMappingEngine>();
            mockCacheService = new Mock<ICacheService>();
            mockUserService = new Mock<IUserService>();
        
            mockHelpDeskWcf = new Mock<IHelpDeskNotification>();

            mockAdwService = new Mock<IAdwService>();
            mockUserService = new Mock<IUserService>();

            mockContainerProvider = new Mock<IContainerProvider>();
            
            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            mockContainerProvider.Setup(m => m.GetService<IAdwService>()).Returns(mockAdwService.Object);
            mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            mockClient.Setup(m => m.Create<IHelpDeskNotification>("HelpDeskNotification.svc")).Returns(mockHelpDeskWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new HelpDeskService(null, mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        #region Create tests

        /// <summary>
        /// Test create runs successfully and returns expected results on valid use with Lap code.
        /// </summary>
        [TestMethod]
        public void Create_WithLapCode_Valid()
        {
            var subject = "FOO";
            var inModel = new HelpDeskModel { Subject = subject };
            var request = MappingEngine.Map<InsHelpDeskNotificationRequest>(inModel);
            var response = new InsHelpDeskNotificationResponse { requestID = 123, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var sinf = new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, SubordinateCode = subject } };
            var sula = new RelatedCodeModel { Dominant = true, SubordinateCode = subject, SubordinateDescription = "LAPCODE" };

            mockAdwService.Setup(m => m.GetRelatedCodes("SINF", inModel.Subject)).Returns(sinf);
            mockAdwService.Setup(m => m.GetRelatedCode("SULA", request.subjectArea, "TODO")).Returns(sula);

            mockMappingEngine.Setup(m => m.Map<InsHelpDeskNotificationRequest>(inModel)).Returns(request);
            mockHelpDeskWcf.Setup(m => m.Insert(request)).Returns(response);

            var result = SystemUnderTest().Create(inModel);

            Assert.IsTrue(result == response.requestID);
            mockMappingEngine.Verify(m => m.Map<InsHelpDeskNotificationRequest>(inModel), Times.Once());
            mockHelpDeskWcf.Verify(m => m.Insert(request), Times.Once());
        }

        /// <summary>
        /// Test create runs successfully and returns expected results on valid use without Lap code.
        /// </summary>
        [TestMethod]
        public void Create_WithoutLapCode_Valid()
        {
            var subject = "FOO";
            var inModel = new HelpDeskModel { Subject = subject };
            var request = MappingEngine.Map<InsHelpDeskNotificationRequest>(inModel);
            var response = new InsHelpDeskNotificationResponse { requestID = 123, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var sinf = new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, SubordinateCode = subject } };
            var sula = new RelatedCodeModel { Dominant = true, SubordinateCode = subject, SubordinateDescription = string.Empty };

            mockAdwService.Setup(m => m.GetRelatedCodes("SINF", inModel.Subject)).Returns(sinf);
            mockAdwService.Setup(m => m.GetRelatedCode("SULA", request.subjectArea, "TODO")).Returns(sula);

            mockMappingEngine.Setup(m => m.Map<InsHelpDeskNotificationRequest>(inModel)).Returns(request);
            mockHelpDeskWcf.Setup(m => m.Insert(request)).Returns(response);

            var result = SystemUnderTest().Create(inModel);

            Assert.IsTrue(result == response.requestID);
            mockMappingEngine.Verify(m => m.Map<InsHelpDeskNotificationRequest>(inModel), Times.Once());
            mockHelpDeskWcf.Verify(m => m.Insert(request), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Create_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var subject = "FOO";
            var inModel = new HelpDeskModel { Subject = subject };
            var request = MappingEngine.Map<InsHelpDeskNotificationRequest>(inModel);
            var sinf = new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, SubordinateCode = subject } };
            var sula = new RelatedCodeModel { Dominant = true, SubordinateCode = subject, SubordinateDescription = "LAPCODE" };

            mockAdwService.Setup(m => m.GetRelatedCodes("SINF", inModel.Subject)).Returns(sinf);
            mockAdwService.Setup(m => m.GetRelatedCode("SULA", request.subjectArea, string.Empty)).Returns(sula);

            mockMappingEngine.Setup(m => m.Map<InsHelpDeskNotificationRequest>(inModel)).Returns(request);
            mockHelpDeskWcf.Setup(m => m.Insert(request)).Throws(exception);

            SystemUnderTest().Create(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void Create_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var subject = "FOO";
            var inModel = new HelpDeskModel { Subject = subject };
            var request = MappingEngine.Map<InsHelpDeskNotificationRequest>(inModel);
            var sinf = new List<RelatedCodeModel> { new RelatedCodeModel { Dominant = true, SubordinateCode = subject } };
            var sula = new RelatedCodeModel { Dominant = true, SubordinateCode = subject, SubordinateDescription = "LAPCODE" };

            mockAdwService.Setup(m => m.GetRelatedCodes("SINF", inModel.Subject)).Returns(sinf);
            mockAdwService.Setup(m => m.GetRelatedCode("SULA", request.subjectArea, string.Empty)).Returns(sula);

            mockMappingEngine.Setup(m => m.Map<InsHelpDeskNotificationRequest>(inModel)).Returns(request);
            mockHelpDeskWcf.Setup(m => m.Insert(request)).Throws(exception);

            SystemUnderTest().Create(inModel);
        }

        #endregion
    }
}
