using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; 
using Employment.Web.Mvc.Area.Payment.Controllers;
using Employment.Web.Mvc.Area.Payment.Mappers;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.Payment.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="PaymentController" />.
    /// </summary>
    [TestClass]
    public class PaymentControllerTest
    {
        private PaymentController SystemUnderTest()
        {
            return new PaymentController(mockUserService.Object, mockAdwService.Object);
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