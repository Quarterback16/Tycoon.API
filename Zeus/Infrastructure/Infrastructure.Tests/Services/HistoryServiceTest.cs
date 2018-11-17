using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Employment.Web.Mvc.Infrastructure.Interfaces;

using Moq;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Employment.Web.Mvc.Infrastructure.Tests.Services
{
    /// <summary>
    ///This is a test class for HistoryService and is intended
    ///to contain all HistoryService Unit Tests
    ///</summary>
    [TestClass()]
    public class HistoryServiceTest
    {
        private HistoryService SystemUnderTest()
        {
            return new HistoryService(mockConfigurationManager.Object, mockSqlService.Object, mockClient.Object,  mockCacheService.Object);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        [ExcludeFromCodeCoverage]
        public TestContext TestContext { get; set; }

        private Mock<IContainerProvider> mockContainerProvider;
        private Mock<IConfigurationManager> mockConfigurationManager;
        private Mock<IClient> mockClient;
        private Mock<ISqlService> mockSqlService;
        private Mock<ICacheService> mockCacheService;
        private Mock<IUserService> mockUserService;
       // private Mock<IMappingEngine> mockMappingEngine;

        private NameValueCollection appSettings = new NameValueCollection();
        private readonly string username = "User";
        private readonly RouteValueDictionary objectValues = new RouteValueDictionary {{"id", "123456789"}};
        private readonly string displayName = "John A. Smith";
        private readonly HistoryType historyType = HistoryType.JobSeeker;
        private Pageable<HistoryModel> getResults;
        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockConfigurationManager = new Mock<IConfigurationManager>();
            mockClient = new Mock<IClient>();
            mockCacheService = new Mock<ICacheService>();
            mockSqlService = new Mock<ISqlService>();
            mockUserService = new Mock<IUserService>();
           // mockMappingEngine = new Mock<IMappingEngine>();

            appSettings.Add("RHEA.History.PageSize", "10");
            mockConfigurationManager.SetupGet(m => m.AppSettings).Returns(appSettings);

            mockUserService.Setup(m => m.Username).Returns(username);
            mockContainerProvider = new Mock<IContainerProvider>();
            mockContainerProvider.Setup(m => m.GetService<IUserService>()).Returns(mockUserService.Object);
            DependencyResolver.SetResolver(mockContainerProvider.Object);

            // Setup Sql Service
            var results = new[] { 
                new HistoryModel { DateAccessed = DateTime.Now, IsPinned = false, Values = objectValues,  DisplayName = displayName, HistoryType = historyType } 
            };

            mockSqlService.Setup(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryInsert", It.IsAny<IEnumerable<SqlParameter>>()));
            mockSqlService.Setup(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryDelete", It.IsAny<IEnumerable<SqlParameter>>()));
            mockSqlService.Setup(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryUpdate", It.IsAny<IEnumerable<SqlParameter>>()));
            mockSqlService.Setup(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryUpdate", It.IsAny<IEnumerable<SqlParameter>>()));
            mockSqlService.Setup(m => m.Execute<HistoryModel>(It.IsAny<string>(), "rheRecentHistoryGet", It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader,HistoryModel>>())).Returns(results);
            mockSqlService.Setup(m => m.Execute<HistoryModel>(It.IsAny<string>(), "rheRecentHistoryGetSingle", It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, HistoryModel>>())).Returns(results);

            getResults = new Pageable<HistoryModel>(new PageMetadata());
            getResults.AddRange(results);

          //  mockMappingEngine.Setup(m => m.Map<IEnumerable<HistoryModel>, IPageable<HistoryModel>>(results)).Returns(getResults);
        }

        /// <summary>
        ///A test for HistoryService_Constructor_ExpectedException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Constructor_ExpectedException()
        {
            var target = new HistoryService(null, null,  null, null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for HistoryService_Constructor_ExpectedException1
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Constructor_ExpectedException1()
        {
            var target = new HistoryService(null, mockSqlService.Object, mockClient.Object,  mockCacheService.Object);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for HistoryService_Constructor_ExpectedException2
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Constructor_ExpectedException2()
        {
            var target = new HistoryService(mockConfigurationManager.Object, null, mockClient.Object,  mockCacheService.Object);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for HistoryService_Constructor_ExpectedExceptio3
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Constructor_ExpectedException3()
        {
            var target = new HistoryService(mockConfigurationManager.Object, mockSqlService.Object, null,  mockCacheService.Object);
            Assert.Fail("Expected Exception");
        }



        /// <summary>
        ///A test for HistoryService_Constructor_ExpectedException4
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Constructor_ExpectedException5()
        {
            var target = new HistoryService(mockConfigurationManager.Object, mockSqlService.Object, mockClient.Object,  null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for HistoryService_Constructor
        ///</summary>
        [TestMethod()]
        public void HistoryService_Constructor()
        {
            Assert.IsNotNull(SystemUnderTest());
        }

        /// <summary>
        /// A test for HistoryService_Set_ExpectedException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Set_ExpectedException()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Set(HistoryType.JobSeeker, new RouteValueDictionary(), displayName);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        /// A test for HistoryService_Set_ExpectedException1
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Set_ExpectedException1()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Set(HistoryType.JobSeeker, objectValues, null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        /// A test for HistoryService_Set
        /// </summary>
        [TestMethod]
        public void HistoryService_Set()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Set(HistoryType.JobSeeker, objectValues, displayName);
            mockSqlService.Verify(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryInsert", It.IsAny<IEnumerable<SqlParameter>>()), Times.Exactly(1));
        }

        /// <summary>
        /// A test for HistoryService_Remove_ExpectedException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Remove_ExpectedException()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Remove(historyType, null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        /// A test for HistoryService_Remove
        /// </summary>
        [TestMethod]
        public void HistoryService_Remove()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Remove(historyType, objectValues);
            mockSqlService.Verify(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryDelete", It.IsAny<IEnumerable<SqlParameter>>()), Times.Exactly(1));
        }

        /// <summary>
        /// A test for HistoryService_Pin_ExpectedException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Pin_ExpectedException()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Pin(historyType, null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        /// A test for HistoryService_Pin
        /// </summary>
        [TestMethod]
        public void HistoryService_Pin()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Pin(historyType, objectValues);
            mockSqlService.Verify(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryUpdate", It.IsAny<IEnumerable<SqlParameter>>()), Times.Exactly(1));
        }

        /// <summary>
        /// A test for HistoryService_Unpin_ExpectedException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HistoryService_Unpin_ExpectedException()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Unpin(historyType, null);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        /// A test for HistoryService_Unpin
        /// </summary>
        [TestMethod]
        public void HistoryService_Unpin()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            target.Unpin(historyType, objectValues);
            mockSqlService.Verify(m => m.ExecuteNonQuery(It.IsAny<string>(), "rheRecentHistoryUpdate", It.IsAny<IEnumerable<SqlParameter>>()), Times.Exactly(1));
        }

        /// <summary>
        /// A test for HistoryService_Get
        /// </summary>
        [TestMethod]
        public void HistoryService_Get()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            IEnumerable<HistoryModel> model = target.Get(historyType);
            Assert.AreEqual(getResults.First().Values, model.First().Values);
            mockSqlService.Verify(m => m.Execute<HistoryModel>(It.IsAny<string>(), "rheRecentHistoryGet", It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, HistoryModel>>()), Times.Exactly(1));
        }

        /// <summary>
        /// A test for HistoryService_Get
        /// </summary>
        [TestMethod]
        public void HistoryService_GetSingle()
        {
            var target = SystemUnderTest();
            Assert.IsNotNull(target);
            HistoryModel model = target.Get(historyType, objectValues);
            Assert.IsNotNull(model);
            Assert.AreEqual(getResults.First(), model);
            mockSqlService.Verify(m => m.Execute<HistoryModel>(It.IsAny<string>(), "rheRecentHistoryGetSingle", It.IsAny<IEnumerable<SqlParameter>>(), It.IsAny<Func<IDataReader, HistoryModel>>()), Times.Exactly(1));
        }
    }
}
