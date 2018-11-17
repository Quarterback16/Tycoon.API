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
    /// Unit tests for <see cref="DiaryPhoneService" />.
    /// </summary>
    [TestClass]
    public class DiaryPhoneServiceTest
    {
        private DiaryPhoneService SystemUnderTest()
        {
            return new DiaryPhoneService(mockClient.Object, mockMappingEngine.Object, mockCacheService.Object);
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
        private Mock<IDiarySitePhone> mockDiaryPhoneWcf;

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
            mockDiaryPhoneWcf = new Mock<IDiarySitePhone>();
            mockClient.Setup(m => m.Create<IDiarySitePhone>("DiarySitePhone.svc")).Returns(mockDiaryPhoneWcf.Object);
        }

        /// <summary>
        /// Test <see cref="ArgumentNullException" /> is thrown when instantiated with null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_CalledWithNullArguments_ThrowsArgumentNullException()
        {
            new DiaryPhoneService(null, null, null);
        }

        #region GetSitePhoneList tests

        /// <summary>
        /// Test GetSitePhoneList runs successfully and returns expected results on valid use.
        /// </summary>
        [TestMethod]
        public void GetSitePhoneList_Valid()
        {
            var siteCode = "HA10";

            var response = new List<SitePhoneDetails>
            {
                new SitePhoneDetails
                {
                    PhoneId = 1,
                    PhoneNumber = "123",
                    SiteCode = siteCode
                }
            };

            var outModel = MappingEngine.Map<IEnumerable<DiaryPhoneModel>>(response);

            mockDiaryPhoneWcf.Setup(m => m.SitePhoneList(siteCode)).Returns(response);
            mockMappingEngine.Setup(m => m.Map<IEnumerable<DiaryPhoneModel>>(response)).Returns(outModel);

            var result = SystemUnderTest().GetSitePhoneList(siteCode);

            Assert.IsTrue(result.Count() == outModel.Count());
            Assert.IsTrue(result.First().PhoneID == outModel.First().PhoneID);
            Assert.IsTrue(result.First().SiteCode == outModel.First().SiteCode);

            mockDiaryPhoneWcf.Verify(m => m.SitePhoneList(siteCode), Times.Once());
            mockMappingEngine.Verify(m => m.Map<IEnumerable<DiaryPhoneModel>>(response), Times.Once());
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException{ValidationFault}" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSitePhoneList_WcfThrowsFaultExceptionValidationFault_ThrowsServiceValidationException()
        {
            var exception = new FaultException<ValidationFault>(new ValidationFault { Details = new List<ValidationDetail> { new ValidationDetail { Key = "Key", Message = "Message" } } });

            mockDiaryPhoneWcf.Setup(m => m.SitePhoneList("HA10")).Throws(exception);

            SystemUnderTest().GetSitePhoneList("HA10");
        }

        /// <summary>
        /// Test <see cref="ServiceValidationException" /> is thrown when WCF throws <see cref="FaultException" />.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void GetSitePhoneList_WcfThrowsFaultException_ThrowsServiceValidationException()
        {
            var exception = new FaultException(new FaultReason("reason"), new FaultCode("code"));

            mockDiaryPhoneWcf.Setup(m => m.SitePhoneList("HA10")).Throws(exception);

            SystemUnderTest().GetSitePhoneList("HA10");
        }

        #endregion
    }
}