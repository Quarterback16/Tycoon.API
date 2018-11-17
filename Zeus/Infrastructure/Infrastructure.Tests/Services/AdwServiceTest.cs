using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Web.Mvc;
using Employment.Esc.Adw.Contracts.DataContracts;
using Employment.Esc.Adw.Contracts.ServiceContracts;
using Employment.Esc.Shared.Contracts.Execution;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Services;
using System.IdentityModel.Claims;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Employment.Web.Mvc.Infrastructure.Interfaces;

using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    /// <summary>
    ///This is a test class for AdwServiceTest and is intended
    ///to contain all AdwServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdwServiceTest
    {
        private AdwService SystemUnderTest()
        {
            return new AdwService(mockSqlService.Object, mockConfigurationManager.Object, mockClient.Object,  mockCacheService.Object);
        }

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext { get; set; }

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var adwMapper = new AdwMapper();
        //            adwMapper.Map(Mapper.Configuration);

        //            var stringMapper = new StringMapper();
        //            stringMapper.Map(Mapper.Configuration);

        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IClient> mockClient;
        private Mock<ISqlService> mockSqlService;
        private Mock<IRuntimeCacheService> mockCacheService;
        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;
        private Mock<IAdwActiveDirectory> mockAdwActiveDirectory;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockAdwActiveDirectory = new Mock<IAdwActiveDirectory>();
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<IRuntimeCacheService>();
            mockSqlService = new Mock<ISqlService>();
            mockConfigurationManager = new Mock<IConfigurationManager>();

            mockConfigurationManager.Setup(m => m.ConnectionStrings(It.IsAny<string>())).Returns("ConnectionString");

            mockContainerProvider = new Mock<IContainerProvider>();
            mockUserService = new Mock<IUserService>();
            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();

            // Setup principal to return identity
            mockClaimsPrincipal.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup identity to default as authenticated
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");
            mockClaimsIdentity.Setup(m => m.IsAuthenticated).Returns(true);

            // Setup claims identity in User Service
            mockUserService.Setup(m => m.DateTime).Returns(DateTime.Now);
            mockUserService.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup Dependency Resolver to use mocked Container Provider
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            //mockContainerProvider.Setup(m => m.GetService<IMappingEngine>()).Returns(MappingEngine);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            // Setup Sql Service
            IEnumerable<PropertyModel> propertyData = new[] { new PropertyModel { Code = "Code", CodeType = "CodeType", PropertyType = "PropertyType", Value = "Value", StartDate = DateTime.MinValue } };
            IEnumerable<CodeModel> listCodeData = new[] { new CodeModel { Code = "Code", Description = "Description", ShortDescription = "ShortDescription", StartDate = DateTime.MinValue } };
            IEnumerable<RelatedCodeModel> relatedCodeData = new[] { new RelatedCodeModel { RelatedCode = "RelatedCode", DominantCode = "DominantCode", DominantDescription = "DominantDescription", DominantShortDescription = "DominantShortDescription", SubordinateCode = "SubordinateCode", SubordinateDescription = "SubordinateDescription", SubordinateShortDescription = "SubordinateShortDescription", StartDate = DateTime.Now } };

            mockSqlService.Setup(m => m.Execute<PropertyModel>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, PropertyModel>>())).Returns(propertyData);
            mockSqlService.Setup(m => m.Execute<CodeModel>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, CodeModel>>())).Returns(listCodeData);
            mockSqlService.Setup(m => m.Execute<RelatedCodeModel>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, RelatedCodeModel>>())).Returns(relatedCodeData);

            mockClient.Setup(m => m.Create<IAdwActiveDirectory>(It.IsAny<string>())).Returns(mockAdwActiveDirectory.Object);

            UsersResponse usersResponse = new UsersResponse();

            usersResponse.ExecutionResult = new ExecutionResult { Status = ExecuteStatus.Success };
            usersResponse.Users = new[] { new User ( "UserID", "LastName", "FirstName", "OrganisationCode", "SiteCode", "Role") };

            mockAdwActiveDirectory.Setup(m => m.ListUsersByOrg(It.IsAny<ListUsersByOrgRequest>())).Returns(usersResponse);
            mockAdwActiveDirectory.Setup(m => m.ListUsersBySite(It.IsAny<ListUsersBySiteRequest>())).Returns(usersResponse);

            mockAdwActiveDirectory.Setup(m => m.GetUserIdNameDetails(It.IsAny<GetUserIdNameDetailsRequest>()))
                                  .Returns(usersResponse);
        }

        /// <summary>
        ///A test for AdwService Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwServiceConstructor_ExpectedException()
        {
            var target = new AdwService(null, null, null, null);
            Assert.Fail("Expected Exception");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwServiceConstructor_ExpectedException1()
        {
            IClient client = new Mock<IClient>().Object;
            IRuntimeCacheService cacheService = new Mock<IRuntimeCacheService>().Object;
            IUserService userService = new Mock<IUserService>().Object;
            ISqlService sql = new Mock<ISqlService>().Object;
            var target = new AdwService(sql, null, client,  cacheService);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for AdwService Constructor
        ///</summary>
        [TestMethod()]
        public void AdwServiceConstructor()
        {
            Assert.IsNotNull(SystemUnderTest());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwService_GetPropertyValueThrowsExceptionWhenNoCodeTypeSupplied_ThrowsArgumentNullException()
        {
            var sut = SystemUnderTest();
            Assert.IsNotNull(sut);
            sut.GetPropertyValue(null, "Code", "PropertyType");
            Assert.Fail("Expected Exception");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwService_GetPropertyValueThrowsExceptionWhenNoCodeSupplied_ThrowsArgumentNullException()
        {
            var sut = SystemUnderTest();
            Assert.IsNotNull(sut);
            sut.GetPropertyValue("CodeType", null, "PropertyType");
            Assert.Fail("Expected Exception");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AdwService_GetPropertyValueThrowsExceptionWhenNoPropertyTypeSupplied_ThrowsArgumentNullException()
        {
            var sut = SystemUnderTest();
            Assert.IsNotNull(sut);
            sut.GetPropertyValue("CodeType", "Code", null);
            Assert.Fail("Expected Exception");
        }

        [TestMethod]
        public void AdwService_GetPropertyValue_ValueReturned()
        {
            var value = SystemUnderTest().GetPropertyValue("CodeType", "Code", "PropertyType");

            Assert.IsTrue(value == "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListCode_ExpectedException()
        {
            var sut = SystemUnderTest();
            Assert.IsNotNull(sut);
            sut.GetListCode("", null);
            Assert.Fail("Expected Exception");
        }

        [TestMethod]
        public void AdwService_GetListCode()
        {
            var code = SystemUnderTest().GetListCode("FOO", "Code");

            Assert.IsTrue(code.Code == "Code");
        }

        [TestMethod]
        public void AdwService_GetListCodeDescription()
        {
            var description = SystemUnderTest().GetListCodeDescription("FOO", "Code");

            Assert.IsTrue(description == "Description");
        }

        [TestMethod]
        public void AdwService_GetListCodeShortDescription()
        {
            var description = SystemUnderTest().GetListCodeDescriptionShort("FOO", "Code");

            Assert.IsTrue(description == "ShortDescription");
        }

        [TestMethod]
        public void AdwService_GetListCodes()
        {
            var codes = SystemUnderTest().GetListCodes("FOO");

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetListCodesWithCurrentCodesOnly()
        {
            var codes = SystemUnderTest().GetListCodes("FOO", true);

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetListCodesWithStartingCode()
        {
            var codes = SystemUnderTest().GetListCodes("FOO", "Code");

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetListCodesWithStartingCodeAndCurrentCodesOnly()
        {
            var codes = SystemUnderTest().GetListCodes("FOO", "Code", true);

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetRelatedCode()
        {
            var code = SystemUnderTest().GetRelatedCode("FOOO", "DominantCode", "SubordinateCode", true).ToCodeModel();

            Assert.IsTrue(code.Code == "SubordinateCode");
        }

        [TestMethod]
        public void AdwService_GetRelatedCodeDescription()
        {
            var description = SystemUnderTest().GetRelatedCodeDescription("FOOO", "DominantCode", "SubordinateCode", true);

            Assert.IsTrue(description == "SubordinateDescription");
        }

        [TestMethod]
        public void AdwService_GetRelatedCodeShortDescription()
        {
            var description = SystemUnderTest().GetRelatedCodeDescriptionShort("FOOO", "DominantCode", "SubordinateCode", true);

            Assert.IsTrue(description == "SubordinateShortDescription");
        }

        [TestMethod]
        public void AdwService_GetRelatedCodes()
        {
            var codes = SystemUnderTest().GetRelatedCodes("FOOO");

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetRelatedCodesWithCurrentCodesOnly()
        {
            var codes = SystemUnderTest().GetRelatedCodes("FOOO", true);

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetRelatedCodesWithSearchCode()
        {
            var codes = SystemUnderTest().GetRelatedCodes("FOOO", "DominantCode", true);

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetListCodesWithSearchCodeAndCurrentCodesOnly()
        {
            var codes = SystemUnderTest().GetRelatedCodes("FOOO", "DominantCode", true, true);

            Assert.IsTrue(codes.Any());
        }

        [TestMethod]
        public void AdwService_GetUsersInOrganisation()
        {
            var users = SystemUnderTest().GetUsersInOrganisation("FOOO");

            Assert.IsTrue(users.Any());
        }

        [TestMethod]
        public void AdwService_GetUsersInSite()
        {
            var users = SystemUnderTest().GetUsersInSite("FOOO");

            Assert.IsTrue(users.Any());
        }

        [TestMethod]
        public void AdwService_GetUserDetails()
        {
            string userId = "AB1234_D";
            var userDetails = SystemUnderTest().GetUserDetails(userId);

            Assert.IsNotNull(userDetails);
            Assert.AreEqual("FirstName", userDetails.FirstName);
        }


        [TestMethod]
        public void AdwService_GetUserDetails_EmptyUserId()
        {
            string userId = string.Empty;
            var userDetails = SystemUnderTest().GetUserDetails(userId);

            Assert.IsNotNull(userDetails);
            Assert.IsTrue(string.IsNullOrEmpty(userDetails.FirstName));
        }


        [TestMethod]
        public void AdwService_GetUserDetails_RetrievedFromCache()
        {
            UserModel userModel = null;
            string userId = "AB1234_D";
            var userDetails = SystemUnderTest().GetUserDetails(userId);

            mockCacheService.Setup(m => m.TryGet(It.IsAny<KeyModel>(), out userModel)).Returns(true);

            Assert.IsNotNull(userDetails);
            Assert.AreEqual("FirstName", userDetails.FirstName);
            Assert.AreNotEqual("FirstName", userDetails.LastName);
            Assert.AreEqual("LastName", userDetails.LastName);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AdwService_GetUserDetails_ThrowsExceptionFault()
        {
            string userId = "AB1234_D";
            ValidationDetail details = new ValidationDetail("Error Occurred", "Key", "tag");

            mockAdwActiveDirectory.Setup(m => m.GetUserIdNameDetails(It.IsAny<GetUserIdNameDetailsRequest>()))
                                  .Throws(new FaultException<ValidationFault>(new ValidationFault(){Details = new List<ValidationDetail>() {details} }));

            var userDetails = SystemUnderTest().GetUserDetails(userId);
        }


        [TestMethod]
        [ExpectedException(typeof(ServiceValidationException))]
        public void AdwService_GetUserDetails_ThrowsException()
        {
            string userId = "AB1234_D";

            mockAdwActiveDirectory.Setup(m => m.GetUserIdNameDetails(It.IsAny<GetUserIdNameDetailsRequest>()))
                                  .Throws(new FaultException());

            var userDetails = SystemUnderTest().GetUserDetails(userId);
        }
    }
}
