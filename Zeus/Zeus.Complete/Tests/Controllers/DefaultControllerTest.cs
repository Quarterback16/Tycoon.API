using System.Web.Mvc;
//using AutoMapper;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Zeus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;

namespace Employment.Web.Mvc.Zeus.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="DefaultController" />.
    /// </summary>
    [TestClass]
    public class DefaultControllerTest
    {
        private DefaultController SystemUnderTest()
        {
            return new DefaultController(mockUserService.Object, mockAdwService.Object, mockBulletinService.Object, mockNoticeboardService.Object);
        }

        private Mock<INoticeboardService> mockNoticeboardService;
        private Mock<IBulletinService> mockBulletinService;
        private Mock<IUserService> mockUserService;
        //private Mock<IMappingEngine> mockMappingEngine;
        private Mock<IAdwService> mockAdwService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            mockNoticeboardService = new Mock<INoticeboardService>();
            mockBulletinService = new Mock<IBulletinService>();
            //mockMappingEngine = new Mock<IMappingEngine>();
            mockAdwService = new Mock<IAdwService>();
            mockUserService = new Mock<IUserService>();
        }

        /// <summary>
        /// Test index returns a view result.
        /// </summary>
        [TestMethod]
        public void Index_WithGet_ReturnsViewResult()
        {
            var controller = SystemUnderTest();

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result, "ViewResult should not be null.");
        }
    }
}