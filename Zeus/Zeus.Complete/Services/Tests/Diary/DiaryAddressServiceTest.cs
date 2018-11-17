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
    /// Unit tests for <see cref="DiaryAddressService" />.
    /// </summary>
    [TestClass]
    public class DiaryAddressServiceTest
    {
        private DiaryAddressService SystemUnderTest()
        {
            return new DiaryAddressService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
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
        private Mock<IDiarySessionAddress> mockDiaryAddressWcf;

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
            mockDiaryAddressWcf = new Mock<IDiarySessionAddress>();
            mockClient.Setup(m => m.Create<IDiarySessionAddress>("DiarySessionAddress.svc")).Returns(mockDiaryAddressWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new DiaryAddressService(null, null, null);
        }

        #region GetActiveSiteAddressList tests

        /// <summary>
        /// Test GetActiveSiteAddressList runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetActiveSiteAddressList_Valid()
        {
            var siteCode = "HA10";

            //var request = new SessionAddressListRequest
            //{
            //    SiteCode = siteCode,
            //    Status = addressStatus
            //};

            var response = new SessionAddressListResponse
            {
                SessionAddresses = new List<SessionAddressData>
                {
                    new SessionAddressData
                    {
                        SessionAddressId = 1,
                        SiteCode = siteCode,
                        Suburb = "suburb",
                        State = "state"
                    }
                }
            };

            var outModel = MappingEngine.Map<IEnumerable<DiaryAddressModel>>(response.SessionAddresses);

            mockDiaryAddressWcf.Setup(m => m.SessionAddressList(It.IsAny<SessionAddressListRequest>())).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<DiaryAddressModel>>(response.SessionAddresses)).Returns(outModel);

            var result = SystemUnderTest().GetActiveSiteAddressList(siteCode);

            Assert.IsTrue(result.Count() == outModel.Count());
            Assert.IsTrue(result.First().AddressID == outModel.First().AddressID);
            Assert.IsTrue(result.First().SiteCode == outModel.First().SiteCode);

            mockDiaryAddressWcf.Verify(m => m.SessionAddressList(It.IsAny<SessionAddressListRequest>()), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<DiaryAddressModel>>(response.SessionAddresses), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetActiveSiteAddressList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockDiaryAddressWcf.Setup(m => m.SessionAddressList(It.IsAny<SessionAddressListRequest>())).Throws(exception);

            SystemUnderTest().GetActiveSiteAddressList("HA10");
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetActiveSiteAddressList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            mockDiaryAddressWcf.Setup(m => m.SessionAddressList(It.IsAny<SessionAddressListRequest>())).Throws(exception);

            SystemUnderTest().GetActiveSiteAddressList("HA10");
        }

        #endregion
    }
}