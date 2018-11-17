using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Diary.Contracts.ServiceContracts;
using Employment.Esc.Diary.Contracts.DataContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Diary;
using Employment.Web.Mvc.Service.Interfaces.Diary;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Diary
{
    /// <summary>
    /// Unit tests for <see cref="DiarySessionService" />.
    /// </summary>
    [TestClass]
    public class DiarySessionServiceTest
    {
        private DiarySessionService SystemUnderTest()
        {
            return new DiarySessionService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
        }

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new DiaryMapper();
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
        private Mock<IDiarySession> mockDiarySessionWcf;

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
            mockDiarySessionWcf = new Mock<IDiarySession>();
            mockClient.Setup(m => m.Create<IDiarySession>("DiarySession.svc")).Returns(mockDiarySessionWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new DiarySessionService(null, null, null);
        }

        #region FindSessions tests

        /// <summary>
        /// Test FindSessions runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void FindSessions_Valid()
        {
            var fromDate = new DateTime(2012, 06, 20);
            var toDate = fromDate.AddDays(5);
            var siteCode = "GKGZ";
            var typeCode = "ISSC";

            //var request = new SessionFindRequest
            //{
            //    FromDate = fromDate.Date.AddHours(12),
            //    ToDate = toDate.Date.AddHours(12),
            //    SiteCode = siteCode,
            //    Type = typeCode,
            //    IncludeAppointments = true,
            //    ConsultantText = string.Empty,
            //    ConsultantId = string.Empty,
            //    Location = string.Empty,
            //    ServiceStream = 0
            //};
            var response = new SessionFindResponse
            {
                Sessions = new List<Session>(),
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };

            response.Sessions.Add(new Session
            {
                StartDateTime = fromDate.AddHours(9).AddMinutes(30).ToString(),
                EndDateTime = fromDate.AddHours(9).AddMinutes(44).ToString(),
                SessionId = 11111111,
                Type = typeCode,
                Address = new SessionAddressData
                {
                    SiteCode = siteCode,
                },
            });

            var outModel = MappingEngine.Map<IEnumerable<SessionModel>>(response.Sessions);

            mockDiarySessionWcf.Setup(m => m.SessionFind(It.IsAny<SessionFindRequest>())).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<SessionModel>>(response.Sessions)).Returns(outModel);

            var result = SystemUnderTest().FindSessions(It.IsAny<SessionSearchCriteriaModel>());

            Assert.IsTrue(result.Count() == outModel.Count());
            Assert.IsTrue(result.First().SessionID == outModel.First().SessionID);
            Assert.IsTrue(result.First().SiteCode == outModel.First().SiteCode);

            mockDiarySessionWcf.Verify(m => m.SessionFind(It.IsAny<SessionFindRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<SessionModel>>(response.Sessions), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindSessions_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockDiarySessionWcf.Setup(m => m.SessionFind(It.IsAny<SessionFindRequest>())).Throws(exception);

            SystemUnderTest().FindSessions(It.IsAny<SessionSearchCriteriaModel>());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindSessions_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            mockDiarySessionWcf.Setup(m => m.SessionFind(It.IsAny<SessionFindRequest>())).Throws(exception);

            SystemUnderTest().FindSessions(It.IsAny<SessionSearchCriteriaModel>());
        }

        #endregion
    }
}