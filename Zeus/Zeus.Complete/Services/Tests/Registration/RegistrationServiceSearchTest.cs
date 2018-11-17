using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Registration.Contracts.DataContracts;
using Employment.Esc.Registration.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Esc.Shared.Contracts.Faults;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    
    
    /// <summary>
    ///This is a test class for RegistrationServiceTest and is intended
    ///to contain all RegistrationServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegistrationServiceSearchTest
    {
        private Mock<IContainerProvider> _mockContainerProvider;
        private Mock<IAdwService> _mockAdwService;
        private Mock<IClient> _mockClient;
        private Mock<IMappingEngine> _mockMappingEngine;
        private Mock<ICacheService> _mockCacheService;
        private Mock<IUserService> _mockUserService;
        private Mock<IRegistrationSearch> _mockRegWcf;
        private TestContext _testContextInstance;
        private IMappingEngine _mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (_mappingEngine == null)
                {
                    var mapper = new RegistrationMapper();
                    mapper.Map(Mapper.Configuration);
                    _mappingEngine = Mapper.Engine;
                }

                return _mappingEngine;
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
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        private RegistrationService SystemUnderTest()
        {
            return new RegistrationService(_mockClient.Object, _mockMappingEngine.Object, _mockCacheService.Object, _mockAdwService.Object);
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            _mockClient = new Mock<IClient>();
            _mockMappingEngine = new Mock<IMappingEngine>();
            _mockCacheService = new Mock<ICacheService>();
            _mockUserService = new Mock<IUserService>();
            _mockContainerProvider = new Mock<IContainerProvider>();
            _mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(_mockUserService.Object);
            DependencyResolver.SetResolver(_mockContainerProvider.Object);
            _mockRegWcf = new Mock<IRegistrationSearch>();
            _mockAdwService = new Mock<IAdwService>();
            _mockClient.Setup(m => m.Create<IRegistrationSearch>("RegistrationSearch.svc")).Returns(_mockRegWcf.Object);
        }

        /// <summary>
        ///A test for Search
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            // Arrange
            var inModel = new JobseekerModel { DateOfBirth = DateTime.Now, GivenName = "GLENN", Surname = "SUFONG", Gender = "M" };
            var request = MappingEngine.Map<RegistrationSearchRequest>(inModel);
            var searchResultItem = new RegistrationSearchResultItem { JobSeekerId = 3187026003, CRN = "205882473X", Surname = "SUFONG", GivenName = "GLENN", Gender = "M", DateOfBirth = DateTime.Now };
            var response = new RegistrationSearchResponse { RegistrationSearchResultItem = (new List<RegistrationSearchResultItem> { searchResultItem }).ToArray() };
            var outModel = MappingEngine.Map<JobseekerModel>(response);

            _mockMappingEngine.Setup(m => m.Map<RegistrationSearchRequest>(inModel)).Returns(request);
            _mockRegWcf.Setup(m => m.Search(request)).Returns(response);
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);

            // Act
            var result = SystemUnderTest().Search(inModel);
            //Assert
            Assert.IsTrue(result.DuplicateJobseekers.Count() > 0);
            Assert.IsTrue(result.DuplicateJobseekers.ElementAt(0).JobSeekerId == 3187026003);
            _mockMappingEngine.Verify(m => m.Map<RegistrationSearchRequest>(inModel), Times.Once());
            _mockRegWcf.Verify(m => m.Search(request), Times.Once());
            _mockMappingEngine.Verify(m => m.Map<JobseekerModel>(response), Times.Once());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchTestWcfThrowsFaultExceptionValidationFaultThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });
            var inModel = new JobseekerModel { DateOfBirth = DateTime.Now, GivenName = "GLENN", Surname = "SUFONG", Gender = "M" };
            var request = MappingEngine.Map<RegistrationSearchRequest>(inModel);
            var searchResultItem = new RegistrationSearchResultItem { JobSeekerId = 3187026003, CRN = "205882473X", Surname = "SUFONG", GivenName = "GLENN", Gender = "M", DateOfBirth = DateTime.Now };
            var response = new RegistrationSearchResponse { RegistrationSearchResultItem = (new List<RegistrationSearchResultItem> { searchResultItem }).ToArray() };
            var outModel = MappingEngine.Map<IEnumerable<JobseekerModel>>(response.RegistrationSearchResultItem);

            _mockMappingEngine.Setup(m => m.Map<RegistrationSearchRequest>(inModel)).Returns(request);
            _mockRegWcf.Setup(m => m.Search(request)).Throws(exception);
            _mockMappingEngine.Setup(m => m.Map<IEnumerable<JobseekerModel>>(response.RegistrationSearchResultItem)).Returns(outModel);

            //Act
            var result = SystemUnderTest().Search(inModel);

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchTestWcfThrowsFaultExceptionThrowsServiceValidationException()
        {
            //Arrange
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            var inModel = new JobseekerModel { DateOfBirth = DateTime.Now, GivenName = "GLENN", Surname = "SUFONG", Gender = "M" };
            var request = MappingEngine.Map<RegistrationSearchRequest>(inModel);
            var searchResultItem = new RegistrationSearchResultItem { JobSeekerId = 3187026003, CRN = "205882473X", Surname = "SUFONG", GivenName = "GLENN", Gender = "M", DateOfBirth = DateTime.Now };
            var response = new RegistrationSearchResponse { RegistrationSearchResultItem = (new List<RegistrationSearchResultItem> { searchResultItem }).ToArray() };
            var outModel = MappingEngine.Map<IEnumerable<JobseekerModel>>(response.RegistrationSearchResultItem);

            _mockMappingEngine.Setup(m => m.Map<RegistrationSearchRequest>(inModel)).Returns(request);
            _mockRegWcf.Setup(m => m.Search(request)).Throws(exception);
            _mockMappingEngine.Setup(m => m.Map<IEnumerable<JobseekerModel>>(response.RegistrationSearchResultItem)).Returns(outModel);

            //Act
            SystemUnderTest().Search(inModel);

        }

        /// <summary>
        /// Searches the linkto center link test.
        /// </summary>
        [TestMethod]
        public void SearchLinktoCenterLinkTest()
        {
            //Arrange
            //Note: all the expectance are setup in lambda expression - Setup method set expetances
            _mockMappingEngine.Setup(m => m.Map<SearchLinkToCentrelinkRequest>(It.IsAny<JobseekerModel>())).Returns(new SearchLinkToCentrelinkRequest { Gender = "M",GivenName = "James",Surname = "Bond"});
            _mockRegWcf.Setup(m => m.SearchLinkToCentrelink(It.IsAny<SearchLinkToCentrelinkRequest>())).Returns(new SearchLinkToCentrelinkResponse());
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(It.IsAny<SearchLinkToCentrelinkResponse>())).Returns(new JobseekerModel());
            //ACT
            var result = SystemUnderTest().SearchLinkToCenterLink(It.IsAny<JobseekerModel>());
            //ASSERT
            // VerifyAll - Everything we have done in Arrange area as a setup should be check to see it occur.
            _mockMappingEngine.VerifyAll();
            _mockRegWcf.Verify();
        }

        /// <summary>
        /// Searches the linkto center link test throw validation fault.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchLinktoCenterLinkTest_throw_ValidationFault()
        {
            //Arrange
            //Note: all the expectance are setup in lambda expression - Setup method set expetances
            _mockMappingEngine.Setup(m => m.Map<SearchLinkToCentrelinkRequest>(It.IsAny<JobseekerModel>())).Returns(new SearchLinkToCentrelinkRequest { Gender = "M", GivenName = "James", Surname = "Bond" });
            var exception = new FaultException<ValidationFault>(new ValidationFault(new List<ValidationDetail> { new ValidationDetail { Key = "key", Message = "message", Tag = "tag" } }));
            _mockRegWcf.Setup(fn => fn.SearchLinkToCentrelink(It.IsAny<SearchLinkToCentrelinkRequest>())).Throws(exception);
            //ACT
            var result = SystemUnderTest().SearchLinkToCenterLink(It.IsAny<JobseekerModel>());
        }

        /// <summary>
        /// Searches the linkto center link test throw fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchLinktoCenterLinkTest_throw_FaultException()
        {
            //Arrange
            //Note: all the expectance are setup in lambda expression - Setup method set expetances
            _mockMappingEngine.Setup(m => m.Map<SearchLinkToCentrelinkRequest>(It.IsAny<JobseekerModel>())).Returns(new SearchLinkToCentrelinkRequest { Gender = "M", GivenName = "James", Surname = "Bond" });
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));
            _mockRegWcf.Setup(m => m.SearchLinkToCentrelink(It.IsAny<SearchLinkToCentrelinkRequest>())).Throws(exception);
            //ACT
            var result = SystemUnderTest().SearchLinkToCenterLink(It.IsAny<JobseekerModel>());

        }

        /// <summary>
        /// Searches the linkto center link test throw fault exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void SearchLinktoCenterLinkTest_throw_CentrelinkFault()
        {
            //Arrange
            //Note: all the expectance are setup in lambda expression - Setup method set expetances
            _mockMappingEngine.Setup(m => m.Map<SearchLinkToCentrelinkRequest>(It.IsAny<JobseekerModel>())).Returns(new SearchLinkToCentrelinkRequest { Gender = "M", GivenName = "James", Surname = "Bond" });
            var exception = new FaultException<CentrelinkFault>(new CentrelinkFault { Details = "details", FaultCode = "code", FaultString = "faultstring" });
            _mockRegWcf.Setup(m => m.SearchLinkToCentrelink(It.IsAny<SearchLinkToCentrelinkRequest>())).Throws(exception);
            //ACT
            var result = SystemUnderTest().SearchLinkToCenterLink(It.IsAny<JobseekerModel>());

        }
    }
}
