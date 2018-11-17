using System.Web.Mvc;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Implementation.Activity;
using Employment.Web.Mvc.Service.Interfaces.Activity;
using Employment.Esc.ActivityManagement.Contracts.ServiceContracts;
using Employment.Esc.ActivityManagement.Contracts.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using System.ServiceModel;
using Employment.Web.Mvc.Service.Interfaces.ServicesAdministration;

namespace Employment.Web.Mvc.Service.Tests.Activity
{
    [TestClass]
    public class ActivityHostServiceTest
    {
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IClient> mockClient;
        private Mock<IMappingEngine> mockMappingEngine;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;

        /// <summary>
        /// WCF service
        /// </summary>
        //private Mock<IActivityManagementService> mockActivityManagementWcf;
        private Mock<IActivityHostLocationService> mockActivityHostLocationtWcf;

        private IMappingEngine mappingEngine;

        protected IMappingEngine MappingEngine
        {
            get
            {
                if (mappingEngine == null)
                {
                    var mapper = new ActivityMapper();
                    mapper.Map(Mapper.Configuration);
                    mappingEngine = Mapper.Engine;
                }

                return mappingEngine;
            }
        }

        private ActivityService SystemUnderTest()
        {
            return new ActivityService(mockClient.Object, MappingEngine, mockCacheService.Object);
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

            //WCF services
            //mockActivityManagementWcf = new Mock<IActivityManagementService>();
            //mockClient.Setup(m => m.Create<IActivityManagementService>("ActivityManagement.svc")).Returns(mockActivityManagementWcf.Object);

            mockActivityHostLocationtWcf = new Mock<IActivityHostLocationService>();
            mockClient.Setup(m => m.Create<IActivityHostLocationService>("ActivityHostLocationService.svc")).Returns(mockActivityHostLocationtWcf.Object);
        }


        #region Activity host test
        /// <summary>
        /// Test ListHostLinks
        /// </summary>
        [TestMethod]
        public void ListHostLinks_Valid()
        {
              //public ACTHostListResponse GetHostList(ACTHostListRequest hostListRequest)
            //1. Setup data
            //a.request
            long activityId = 123123;

            //b.response
            List<ACTHostList> list = new List<ACTHostList>();

            for (int i = 1; i < 10; i++)
            {
                list.Add(ActivityTestDataHelper.CreateDummyACTHostList(i));
            }

            var response = new ACTHostListResponse() { hostLists = list.ToArray() };

            mockActivityHostLocationtWcf.Setup(m => m.GetHostList(It.IsAny<ACTHostListRequest>())).Returns(response);

            MappingEngine.Map<IEnumerable<ActivityHostLinkModel>>(response.hostLists);

            //2. Exec service
            var result = SystemUnderTest().ListHostLinks(activityId);

            //3. Verification
            Assert.AreEqual(1, result.ElementAt(0).HostId);
            Assert.AreEqual(1, result.ElementAt(0).Host.HostID);
            //Verify behaviour
            mockActivityHostLocationtWcf.Verify(m => m.GetHostList(It.Is<ACTHostListRequest>(r => r.ActivityId == activityId)), Times.Once());

        }

        /// <summary>
        /// Test valid host details
        /// </summary>
        [TestMethod]
        public void GetHost_Valid()
        {
            //ACTGetHostDetailsResponse GetHostDetails(ACTGetHostDetailsRequest getHostRequest)
            //1. Setup data
            //a.request
            long hostId = 123123;

            var response = ActivityTestDataHelper.CreateDummyACTGetHostDetailsResponse();

            mockActivityHostLocationtWcf.Setup(m => m.GetHostDetails(It.IsAny<ACTGetHostDetailsRequest>())).Returns(response);

            //2. Exec service
            var result = SystemUnderTest().GetHost(hostId);

            //3. Verification
            Assert.AreEqual("Test123", result.HostName);
            Assert.AreEqual("Line1", result.AddressLine1);
            Assert.AreEqual("Line2", result.AddressLine2);
            Assert.AreEqual("Line3", result.AddressLine3);
        }

        ///// <summary>
        ///// Test Update location details
        ///// </summary>
        //[TestMethod]
        //public void UpdateLocation_Valid()
        //{
        //    //1. Setup data
        //    //a.request
        //    var request = ActivityTestDataHelper.CreateDummyActivityLocationModel();

        //    var response = new ACTManageLocationResponse();
        //    response.INTControlNumber = request.IntegrityControlNumber +1;

        //    mockActivityHostLocationtWcf.Setup(m => m.ManageLocation(It.IsAny<ACTManageLocationRequest>())).Returns(response);

        //    //2. Exec service
        //    var result = SystemUnderTest().UpdateLocation(request);

        //    //3. Verification
        //    Assert.AreEqual(result.IntegrityControlNumber, response.INTControlNumber);
        //}

        ///// <summary>
        ///// Test Add location details
        ///// </summary>
        //[TestMethod]
        //public void AddLocation_Valid()
        //{
        //    //1. Setup data
        //    //a.request
        //    var request = ActivityTestDataHelper.CreateDummyActivityLocationModel();
        //    request.IntegrityControlNumber = 0; // Use the same WCF service as Update, use the ICN to tell whether it is add or update

        //    var response = new ACTManageLocationResponse();
        //    response.LocationSeqNumber = 123;

        //    mockActivityHostLocationtWcf.Setup(m => m.ManageLocation(It.IsAny<ACTManageLocationRequest>())).Returns(response);

        //    //2. Exec service
        //    var result = SystemUnderTest().AddLocation(request);

        //    //3. Verification
        //    Assert.AreEqual(result, response.LocationSeqNumber);
        //}
        //#endregion

        //#region Activity Location service exceptions
        ///// <summary>
        ///// Test invalid search request
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void SearchLocations_RequestDataError_ThrowsServiceValidationException()
        //{
        //    //1. Setup data
        //    //a.request

        //    //2. exec
        //    var result = SystemUnderTest().ListLocations(0);
        //}

        ///// <summary>
        ///// Test failed ExecutionResults
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void SearchLocations_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var exception = ActivityTestDataHelper.CreateDummyFaultException();

        //    //1. Setup data
        //    //a.request
        //    mockActivityHostLocationtWcf.Setup(m => m.GetLocationList(It.IsAny<ACTLocSearchRequest>())).Throws(exception);

        //    //2. exec
        //    var result = SystemUnderTest().ListLocations(123);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetLocation_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        //{
        //    //1. Setup data
        //    //a.request
        //    long activityId = 0;
        //    long locationSeqNumber = 0;

        //    //2. exec
        //    var result = SystemUnderTest().GetLocation(activityId, locationSeqNumber);
        //}

        ///// <summary>
        ///// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ServiceValidationException))]
        //public void GetLocation_WcfThrowsFaultException_ThrowsServiceValidationException()
        //{
        //    var exception = ActivityTestDataHelper.CreateDummyFaultException();

        //    //1. Setup data
        //    //a.request
        //    long activityId = 123;
        //    long locationSeqNumber = 1;

        //    mockActivityHostLocationtWcf.Setup(m => m.GetLocationDetails(It.IsAny<ACTGetLocDetailsRequest>())).Throws(exception);

        //    //2. exec
        //    var result = SystemUnderTest().GetLocation(activityId, locationSeqNumber);
        //}
        #endregion

    }
}
