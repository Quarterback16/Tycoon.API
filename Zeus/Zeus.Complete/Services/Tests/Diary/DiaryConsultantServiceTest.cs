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
    /// Unit tests for <see cref="DiaryConsultantServiceTest" />.
    /// </summary>
    [TestClass]
    public class DiaryConsultantServiceTest
    {
        private DiaryConsultantService SystemUnderTest()
        {
            return new DiaryConsultantService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
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
        private Mock<IDiaryConsultantAbsence> mockDiaryConsultantAbsenceWcf;

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
            mockDiaryConsultantAbsenceWcf = new Mock<IDiaryConsultantAbsence>();
            mockClient.Setup(m => m.Create<IDiaryConsultantAbsence>("DiaryConsultantAbsence.svc")).Returns(mockDiaryConsultantAbsenceWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new DiaryConsultantService(null, null, null);
        }

        #region GetAbsenceList tests

        /// <summary>
        /// Test GetAbsenceList runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetAbsenceList_Valid()
        {
            var siteCode = "HA10";
            var userID = "AA0000";

            var request = new ConsultantAbsenceListRequest
            {
                SiteCode = siteCode,
                ConsultantID = userID
            };
            var response = new ConsultantAbsenceListResponse
            {
                ConsultantAbsences = new List<ConsultantAbsenceListData>
                {
                    new ConsultantAbsenceListData
                    {
                        AbsenceID = 1,
                        ConsultantID = userID
                    }
                }
            };

            var outModel = MappingEngine.Map<IEnumerable<ConsultantAbsenceModel>>(response.ConsultantAbsences);

            mockDiaryConsultantAbsenceWcf.Setup(m => m.ConsultantAbsenceList(It.IsAny<ConsultantAbsenceListRequest>())).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<ConsultantAbsenceModel>>(response.ConsultantAbsences)).Returns(outModel);

            var result = SystemUnderTest().GetAbsenceList(siteCode, userID);

            Assert.IsTrue(result.Count() == outModel.Count());
            Assert.IsTrue(result.First().AbsenceID == outModel.First().AbsenceID);
            Assert.IsTrue(result.First().UserID == outModel.First().UserID);

            mockDiaryConsultantAbsenceWcf.Verify(m => m.ConsultantAbsenceList(It.IsAny<ConsultantAbsenceListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<ConsultantAbsenceModel>>(response.ConsultantAbsences), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAbsenceList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var siteCode = "HA10";
            var userID = "AA0000";

            mockDiaryConsultantAbsenceWcf.Setup(m => m.ConsultantAbsenceList(It.IsAny<ConsultantAbsenceListRequest>())).Throws(exception);

            SystemUnderTest().GetAbsenceList(siteCode, userID);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetAbsenceList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var siteCode = "HA10";
            var userID = "AA0000";

            mockDiaryConsultantAbsenceWcf.Setup(m => m.ConsultantAbsenceList(It.IsAny<ConsultantAbsenceListRequest>())).Throws(exception);

            SystemUnderTest().GetAbsenceList(siteCode, userID);
        }

        #endregion
    }
}
