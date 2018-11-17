using System;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employment.Esc.Shared.Contracts.Execution;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    public class TestService : Service
    {
        public TestService(IClient client,  ICacheService cacheService)
            : base(client,  cacheService)
        {
        }

        public void ValidateRequest(TestRequest request)
        {
            base.ValidateRequest(request);
        }

        public void ValidateResponse(IResponseWithExecutionResult response)
        {
            base.ValidateResponse(response);
        }
    }


    public class TestRequest
    {
        [NotNullValidator(MessageTemplate = "CodeType is a mandatory field.")]
        public string TestField { get; set; }
    }

    /// <summary>
    ///This is a test class for ServiceTest and is intended
    ///to contain all ServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IUserService> mockUserService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserService = new Mock<IUserService>();
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);
        }

        [TestMethod]
        public void CreateService()
        {
            var client = new Mock<IClient>();
            //var mapEngine = new Mock<IMappingEngine>();
            var cache = new Mock<ICacheService>();
            
            var s = new TestService(client.Object,  cache.Object);
            Assert.IsNotNull(s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateServicewithNullClient()
        {
            //var mapEngine = new Mock<IMappingEngine>();
            var cache = new Mock<ICacheService>();
            var s = new TestService(null,  cache.Object);
            Assert.Fail("Expected Exception)");
        }

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void CreateServicewithNullMapEngine()
        //{
        //    var client = new Mock<IClient>();
        //    var cache = new Mock<ICacheService>();
        //    var s = new TestService(client.Object, null, cache.Object);
        //    Assert.Fail("Expected Exception)");
        //}


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateServicewithNullCache()
        {
            var client = new Mock<IClient>();
            //var mapEngine = new Mock<IMappingEngine>();
            var s = new TestService(client.Object,  null);
            Assert.Fail("Expected Exception)");
        }


        [TestMethod()]
        public void ValidateRequestTest()
        {
            var client = new Mock<IClient>();
            //var mapEngine = new Mock<IMappingEngine>();
            var cache = new Mock<ICacheService>();
            var s = new TestService(client.Object,  cache.Object);
            Assert.IsNotNull(s);
            var t = new TestRequest {TestField = "Test"};
            s.ValidateRequest(t);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceValidationException))]
        public void ValidateRequestTestWithInvalidRequest()
        {
            var client = new Mock<IClient>();
           // var mapEngine = new Mock<IMappingEngine>();
            var cache = new Mock<ICacheService>();
            var s = new TestService(client.Object,  cache.Object);
            Assert.IsNotNull(s);
            var t = new TestRequest();
            s.ValidateRequest(t);
            Assert.Fail("Exception expected");
        }

        [TestMethod()]
        public void ValidateResponseTest()
        {
            var client = new Mock<IClient>();
            var cache = new Mock<ICacheService>();
            var s = new TestService(client.Object,  cache.Object);
            Assert.IsNotNull(s);

            var response = new Mock<IResponseWithExecutionResult>();
            s.ValidateResponse(response.Object);
            Assert.IsTrue(true);
        }
    }
}