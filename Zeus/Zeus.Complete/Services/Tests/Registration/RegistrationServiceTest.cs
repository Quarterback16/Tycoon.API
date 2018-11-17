using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;
using AutoMapper;
using Employment.Esc.Employer.Contracts.ServiceContracts;
using Employment.Esc.Registration.Contracts.DataContracts;
using Employment.Esc.Registration.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Esc.SpecialPlacementHistory.Contracts.DataContracts;
using Employment.Esc.SpecialPlacementHistory.Contracts.ServiceContracts;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Service.Implementation.Registration;
using Employment.Web.Mvc.Service.Interfaces.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Service.Tests.Registration
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RegistrationServiceTest
    {
        private Mock<IContainerProvider> _mockContainerProvider;
        private Mock<IAdwService> _mockAdwService;
        private Mock<IClient> _mockClient;
        private Mock<IMappingEngine> _mockMappingEngine;
        private Mock<ICacheService> _mockCacheService;
        private Mock<IUserService> _mockUserService;
        private Mock<IRegistrationDetails> _mockRegWcf;
        private Mock<ISpecialPlacementHistory> _mockSpecialPlacementWcf;
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

 
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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
            _mockRegWcf = new Mock<IRegistrationDetails>();
            _mockAdwService = new Mock<IAdwService>();
            _mockSpecialPlacementWcf = new Mock<ISpecialPlacementHistory>();
            _mockClient.Setup(m => m.Create<IRegistrationDetails>("RegistrationDetails.svc")).Returns(_mockRegWcf.Object);
            _mockClient.Setup(m => m.Create<ISpecialPlacementHistory>("SpecialPlacementHistory.svc")).Returns(_mockSpecialPlacementWcf.Object);
        }

        [TestMethod]
        public void DetailsValid()
        {

            var response = new RegistrationDetailsReadAllResponse { GivenName = "Joe", Surname = "Blow", PhoneItem = new PhoneItem[] { }, AddressItem = new AddressItem[] { }, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);
            var request = MappingEngine.Map<RegistrationDetailsReadAllRequest>((long)123);

            _mockMappingEngine.Setup(m => m.Map<RegistrationDetailsReadAllRequest>((long)123)).Returns(request);
            _mockRegWcf.Setup(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>())).Returns(response);
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);
            _mockUserService.Setup(m => m.Session.JobSeekerID).Returns(0);
            _mockUserService.Setup(m => m.Session.CRN).Returns("");

            var result = SystemUnderTest().ReadJobseeker(123);

            Assert.IsTrue(result.GivenName == "Joe");
            Assert.IsTrue(result.Surname == "Blow");
            _mockRegWcf.Verify(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>()), Times.Once());
            _mockMappingEngine.Verify(m => m.Map<RegistrationDetailsReadAllRequest>((long)123), Times.Once());
        }

        [TestMethod]
        public void ReadJobseeker()
        {
            var response = new RegistrationDetailsReadAllResponse { GivenName = "Joe", Surname = "Blow",PhoneItem = new PhoneItem[]{}, AddressItem = new AddressItem[]{}, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);
            var request = MappingEngine.Map<RegistrationDetailsReadAllRequest>((long)123);

            _mockMappingEngine.Setup(m => m.Map<RegistrationDetailsReadAllRequest>((long)123)).Returns(request);
            _mockRegWcf.Setup(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>())).Returns(response);
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);
            _mockUserService.Setup(m => m.Session.JobSeekerID).Returns(0);
            _mockUserService.Setup(m => m.Session.CRN).Returns("");

            var result = SystemUnderTest().ReadJobseeker(123);

            Assert.IsTrue(result.GivenName == "Joe");
            Assert.IsTrue(result.Surname == "Blow");
            _mockRegWcf.Verify(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>()), Times.Once());
            _mockMappingEngine.Verify(m => m.Map<RegistrationDetailsReadAllRequest>((long)123), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ValidationException()
        {
            var response = new RegistrationDetailsReadAllResponse { GivenName = "Joe", Surname = "Blow", PhoneItem = new PhoneItem[] { }, AddressItem = new AddressItem[] { }, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var outModel = MappingEngine.Map<JobseekerModel>(response);
            var request = new RegistrationDetailsReadAllRequest();

            _mockMappingEngine.Setup(m => m.Map<RegistrationDetailsReadAllRequest>(It.IsAny<long>())).Returns(request);
            _mockRegWcf.Setup(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>())).Returns(response);
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Returns(outModel);

            var result = SystemUnderTest().ReadJobseeker(123);

            Assert.IsTrue(result.GivenName == "Joe");
            Assert.IsTrue(result.Surname == "Blow");
            _mockRegWcf.Verify(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>()), Times.Once());
            _mockMappingEngine.Verify(m => m.Map<RegistrationDetailsReadAllRequest>((long)123), Times.Once());
        }

        /// <summary>
        /// The path where a fault exception is thrown, transalted to ServiceValidationException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void FaultException()
        {
            var response = new RegistrationDetailsReadAllResponse { GivenName = "Joe", Surname = "Blow", PhoneItem = new PhoneItem[] { }, AddressItem = new AddressItem[] { }, ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success } };
            var request = MappingEngine.Map<RegistrationDetailsReadAllRequest>((long)123);
            _mockMappingEngine.Setup(m => m.Map<RegistrationDetailsReadAllRequest>((long)123)).Returns(request);
            _mockRegWcf.Setup(m => m.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>())).Returns(response);
            _mockMappingEngine.Setup(m => m.Map<JobseekerModel>(response)).Throws(new FaultException());
            SystemUnderTest().ReadJobseeker(123);
        }

        [TestMethod]
        public void ModelMisc()
        {
            //var model = new RegistrationModel() {DateOfBirth = DateTime.Parse("30/09/1978")};
            //Assert.IsTrue(model.DateOfBirth.Day == 30);
            //Assert.IsTrue(model.DateOfBirth.Month == 9);
            //Assert.IsTrue(model.DateOfBirth.Year == 1978);
        }

        [TestMethod]
        public void TestReadSpecialPlacements()
        {

            var service = SystemUnderTest();

            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(20);

            var response = new SpecialPlacementHistoryListResponse() { ListItems = new[] { new SpecialPlacementHistoryListItem() { StartDate = startDate, EndDate = endDate, CircumCd = "EOL1" } } };
            var mappedModel = MappingEngine.Map<JobseekerModel>(response);

            var request = MappingEngine.Map<RegistrationDetailsReadAllRequest>((long)123);

            _mockMappingEngine.Setup(m => m.Map<RegistrationDetailsReadAllRequest>((long)123)).Returns(request);

            // define how the mapping will happen
            _mockMappingEngine.Setup(x => x.Map<JobseekerModel>(It.IsAny<SpecialPlacementHistoryListResponse>())).Returns(mappedModel);
            // define how adw will work.
            _mockAdwService.Setup(x => x.GetRelatedCodes("EOLO")).Returns(new[] { new RelatedCodeModel { SubordinateCode = "EOL1" }, new RelatedCodeModel { SubordinateCode = "EOL2" } });
            _mockRegWcf.Setup(
                call => call.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>()))
                .Returns(new RegistrationDetailsReadAllResponse());
            // define the 'wcf' call
            _mockSpecialPlacementWcf.Setup(
                call => call.GetSpecialPlacementHistoryList(It.IsAny<SpecialPlacementHistoryListRequest>()))
                .Returns(new SpecialPlacementHistoryListResponse()
                {
                    ListItems = new[] { new SpecialPlacementHistoryListItem() { StartDate = startDate, EndDate = endDate, CircumCd = "EOL1" } }
                });

            var modelResponse = service.ReadSpecialPlacements(123);
            Assert.IsNotNull(modelResponse);
        }

        [TestMethod]
        public void TestReadSpecialPlacementFilterEol()
        {
            var service = SystemUnderTest();

            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(20);

            var response = new SpecialPlacementHistoryListResponse() { ListItems = new[] { new SpecialPlacementHistoryListItem() { StartDate = startDate, EndDate = endDate, CircumCd = "EOL1" } } };
            var mappedModel = MappingEngine.Map<JobseekerModel>(response);
            // define how the mapping will happen
            _mockMappingEngine.Setup(x => x.Map<RegistrationDetailsReadAllRequest>(It.IsAny<long>())).Returns(new RegistrationDetailsReadAllRequest() { ApplicationName = "UES", JobSeekerId = 123 });
            _mockMappingEngine.Setup(x => x.Map<JobseekerModel>(It.IsAny<SpecialPlacementHistoryListResponse>())).Returns(mappedModel);
            // define how adw will work.
            _mockAdwService.Setup(x => x.GetRelatedCodes("EOLO")).Returns(new[] { new RelatedCodeModel { SubordinateCode = "EOL1" }, new RelatedCodeModel { SubordinateCode = "EOL2" } });
            // define the 'wcf' call
            _mockSpecialPlacementWcf.Setup(
                call => call.GetSpecialPlacementHistoryList(It.IsAny<SpecialPlacementHistoryListRequest>()))
                .Returns(new SpecialPlacementHistoryListResponse()
                {
                    ListItems = new[] { new SpecialPlacementHistoryListItem() { StartDate = startDate, EndDate = endDate, CircumCd = "EOL1" } }
                });

            _mockRegWcf.Setup(
                call => call.GetAll(It.IsAny<RegistrationDetailsReadAllRequest>()))
                .Returns(new RegistrationDetailsReadAllResponse());


            //var modelResponse = service.ReadSpecialPlacements(123, true, string.Empty);
            //var items = new List<SpecialPlacementModel>(modelResponse.HistoryItems);
            //// here we are testing that the filter is working. 
            //Assert.IsTrue(items.Count == 0);

            //modelResponse = service.ReadSpecialPlacements(123, false, string.Empty);
            //items = new List<SpecialPlacementModel>(modelResponse.HistoryItems);
            //// here we are testing that the filter is working. 
            //Assert.IsTrue(items.Count == 0);

            var modelResponse = service.ReadSpecialPlacements(123);
            var items = new List<SpecialPlacementModel>(modelResponse.SpecialPlacements);
            // here we are testing that the filter is working. 
            Assert.IsTrue(items.Count == 1);
        }
         
    }
}
