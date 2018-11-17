using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; 
using Employment.Web.Mvc.Area.JobSeeker.Controllers;
using Employment.Web.Mvc.Area.JobSeeker.Mappers;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.JobSeeker.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="JobSeekerController" />.
    /// </summary>
    [TestClass]
    public class JobSeekerControllerTest
    {
        private JobSeekerController SystemUnderTest()
        {
            return new JobSeekerController(mockUserService.Object, mockAdwService.Object);
        }


        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        { 
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();
        }

        #region Index tests

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

        #endregion
	}
}