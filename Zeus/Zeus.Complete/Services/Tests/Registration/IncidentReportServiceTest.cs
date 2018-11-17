using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using AutoMapper;
using Employment.Esc.JobSeekerIncidentReport.Contracts.ServiceContracts;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Employment.Esc.JobSeekerIncidentReport.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    [TestClass]
    public class IncidentReportServiceTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IAdwService> mockAdwService;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
        private Mock<IJSIR> mockIRWcf;
        private Mock<IAdwService> adwService;


        private TestContext testContextInstance;

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new IncidentReportMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private RegistrationService SystemUnderTest()
        {
            return new RegistrationService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object, adwService.Object);
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
            mockIRWcf = new Mock<IJSIR>();
            adwService = new Mock<IAdwService>();
            mockClient.Setup(m => m.Create<IJSIR>("JobSeekerIncidentReport.svc")).Returns(mockIRWcf.Object);
        }

        /// <summary>
        /// List Incident report successful test.
        /// </summary>
        [TestMethod]
        public void ListTest()
        {
            //Arrange
            var item = new IncidentReportModel { CreationUser = "SR0663_D" };
            var outModel = new JobseekerModel { JobSeekerId = 3187026003, IncidentReports = new List<IncidentReportModel> { item } };
            var inModel = new JobseekerModel { JobSeekerId = 3187026003 };
            mockMappingEngine.Setup(m => m.Map<JobSeekerIncidentReportListRequest>(It.IsAny<JobseekerModel>())).Returns(new JobSeekerIncidentReportListRequest());
            mockIRWcf.Setup(m => m.List(It.IsAny<JobSeekerIncidentReportListRequest>())).Returns(new JobSeekerIncidentReportListResponse());
            mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<JobSeekerIncidentReportListResponse>())).Returns(outModel);

            //act
            var result = SystemUnderTest().GetAllIncidentReport(inModel);

            //Assert
            Assert.IsTrue(result.IncidentReports.Count() > 0);
            Assert.IsTrue(result.IncidentReports.ElementAt(0).CreationUser == "SR0663_D");
            mockMappingEngine.Verify(m => m.Map<JobSeekerIncidentReportListRequest>(It.IsAny<JobseekerModel>()), Times.Once());
            mockIRWcf.Verify(m => m.List(It.IsAny<JobSeekerIncidentReportListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<JobseekerModel>(It.IsAny<JobSeekerIncidentReportListResponse>()), Times.Once());
        }

        /// <summary>
        /// List throw the fault valiation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_throw_Fault_valiation_exception_test()
        {
            //Arrange
            var inModel = new JobseekerModel { JobSeekerId = 3187026003 };
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockMappingEngine.Setup(m => m.Map<JobSeekerIncidentReportListRequest>(It.IsAny<JobseekerModel>())).Returns(new JobSeekerIncidentReportListRequest());
            mockIRWcf.Setup(m => m.List(It.IsAny<JobSeekerIncidentReportListRequest>())).Throws(exception);

            //Act
            SystemUnderTest().GetAllIncidentReport(inModel);
        }

        /// <summary>
        /// List throw the fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void List_throw_Fault_exception_test()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            var inModel = new JobseekerModel { JobSeekerId = 3187026003 };

            mockMappingEngine.Setup(m => m.Map<JobSeekerIncidentReportListRequest>(It.IsAny<JobseekerModel>())).Returns(new JobSeekerIncidentReportListRequest());
            mockIRWcf.Setup(m => m.List(It.IsAny<JobSeekerIncidentReportListRequest>())).Throws(exception);

            //Act
            SystemUnderTest().GetAllIncidentReport(inModel);
        }



    }  // end of IncidentReportServiceTest class
}
