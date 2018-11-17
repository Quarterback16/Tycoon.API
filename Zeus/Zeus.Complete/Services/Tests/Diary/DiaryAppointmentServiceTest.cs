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
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Service.Implementation.Diary;
using Employment.Web.Mvc.Service.Interfaces.Diary;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Diary
{
    /// <summary>
    /// Unit tests for <see cref="DiaryAppointmentService" />.
    /// </summary>
    [TestClass]
    public class DiaryAppointmentServiceTest
    {
        private DiaryAppointmentService SystemUnderTest()
        {
            return new DiaryAppointmentService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, mockDiarySessionService.Object);
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
        private Mock<IDiarySessionService> mockDiarySessionService;
        private Mock<IDiaryAppointment> mockDiaryAppointmentWcf;

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
            mockDiarySessionService = new Mock<IDiarySessionService>();
            mockDiaryAppointmentWcf = new Mock<IDiaryAppointment>();
            mockClient.Setup(m => m.Create<IDiaryAppointment>("DiaryAppointment.svc")).Returns(mockDiaryAppointmentWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new DiaryAppointmentService(null, null, null, null);
        }

        #region FindAppointments tests

        /// <summary>
        /// Test FindAppointments runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void FindAppointments_Valid()
        {
            var fromDate = new DateTime(2012, 06, 20);
            var toDate = fromDate.AddDays(5);
            var siteCode = "GKGZ";

            var inModel = new AppointmentSearchCriteriaModel { AppointmentDateFrom = fromDate, AppointmentDateTo = toDate, SiteCode = siteCode };
            var request = MappingEngine.Map<AppointmentListCriteria>(inModel);
            var response = new AppointmentListResponse
            {
                Appointment = new[] { new Appointment { AppointmentID = 1, JobSeekerID = 3, SessionID = 2, CreationSite = siteCode } },
                ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success }
            };
            var outModel = MappingEngine.Map<IEnumerable<AppointmentModel>>(response.Appointment);

            mockMappingEngine.Setup(m => m.Map<AppointmentListCriteria>(inModel)).Returns(request);
            mockDiaryAppointmentWcf.Setup(m => m.List(request)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<AppointmentModel>>(response.Appointment)).Returns(outModel);

            var result = SystemUnderTest().FindAppointments(inModel);

            Assert.IsTrue(result.Count() == outModel.Count());
            Assert.IsTrue(result.First().AppointmentID == outModel.First().AppointmentID);
            Assert.IsTrue(result.First().CreationSiteCode == outModel.First().CreationSiteCode);

            mockDiaryAppointmentWcf.Verify(m => m.List(It.IsAny<AppointmentListCriteria>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<AppointmentModel>>(response.Appointment), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindAppointments_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            var fromDate = new DateTime(2012, 06, 20);
            var toDate = fromDate.AddDays(5);
            var siteCode = "GKGZ";

            var inModel = new AppointmentSearchCriteriaModel { AppointmentDateFrom = fromDate, AppointmentDateTo = toDate, SiteCode = siteCode };
            var request = MappingEngine.Map<AppointmentListCriteria>(inModel);

            mockMappingEngine.Setup(m => m.Map<AppointmentListCriteria>(inModel)).Returns(request);
            mockDiaryAppointmentWcf.Setup(m => m.List(request)).Throws(exception);

            SystemUnderTest().FindAppointments(inModel);
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FindAppointments_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var fromDate = new DateTime(2012, 06, 20);
            var toDate = fromDate.AddDays(5);
            var siteCode = "GKGZ";

            var inModel = new AppointmentSearchCriteriaModel { AppointmentDateFrom = fromDate, AppointmentDateTo = toDate, SiteCode = siteCode };
            var request = MappingEngine.Map<AppointmentListCriteria>(inModel);

            mockMappingEngine.Setup(m => m.Map<AppointmentListCriteria>(inModel)).Returns(request);
            mockDiaryAppointmentWcf.Setup(m => m.List(request)).Throws(exception);

            SystemUnderTest().FindAppointments(inModel);
        }

        #endregion
    }
}