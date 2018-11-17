using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

using Employment.Web.Mvc.Area.Admin.Controllers;
using Employment.Web.Mvc.Area.Admin.Mappers;
using Employment.Web.Mvc.Area.Admin.ViewModels.User;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.Provisioner;
using System.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Area.Admin.Tests.Controllers
{
    /// <summary>
    /// Unit tests for <see cref="DefaultController" />.
    /// </summary>
    [TestClass]
    public class DefaultControllerTest
    {
        private DefaultController SystemUnderTest()
        {
            return new DefaultController(mockProvisionerService.Object,  mockUserService.Object, mockAdwService.Object);
        }

        //private IMappingEngine mappingEngine;

        //protected IMappingEngine MappingEngine
        //{
        //    get
        //    {
        //        if (mappingEngine == null)
        //        {
        //            var mapper = new AdminMapper();
        //            mapper.Map(Mapper.Configuration);
        //            mappingEngine = Mapper.Engine;
        //        }

        //        return mappingEngine;
        //    }
        //}

        //private Mock<IMappingEngine> mockMappingEngine;
        private Mock<IUserService> mockUserService;
        private Mock<IAdwService> mockAdwService;
        private Mock<IProvisionerService> mockProvisionerService;
        private Mock<ClaimsPrincipal> mockClaimsPrincipal;
        private Mock<ClaimsIdentity> mockClaimsIdentity;
        private Mock<ClaimsIdentity> mockSubject;


        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void TestInitialize()
        {
            //mockMappingEngine = new Mock<IMappingEngine>();
            mockUserService = new Mock<IUserService>();
            mockAdwService = new Mock<IAdwService>();
            mockProvisionerService = new Mock<IProvisionerService>();

            mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockSubject = new Mock<ClaimsIdentity>();
            

            mockUserService.Setup(i => i.Identity).Returns(mockClaimsIdentity.Object);
            mockUserService.Setup(d => d.DateTime).Returns(DateTime.Now);

            // Setup principal to return identity
            mockClaimsPrincipal.Setup(m => m.Identity).Returns(mockClaimsIdentity.Object);

            // Setup identity to default as authenticated
            mockClaimsIdentity.Setup(m => m.Name).Returns("User");
            mockClaimsIdentity.Setup(c => c.Claims).Returns(new List<System.Security.Claims.Claim>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ControllerConstructor_ProvisionerNull()
        {
// ReSharper disable ObjectCreationAsStatement
            new DefaultController(null,  mockUserService.Object, mockAdwService.Object);
// ReSharper restore ObjectCreationAsStatement
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

        [TestMethod]
        public void DateChanger_WithGet_ReturnsViewResult()
        {
            var controller = SystemUnderTest();
            var result = controller.DateChanger() as ViewResult;
            Assert.IsNotNull(result, "ViewResult should not be null.");
        }

        [TestMethod]
        public void DateChanger_WithPost_ReturnsViewResult()
        {
            var controller = SystemUnderTest();
            var model = new DateTimeContext();
            var current = DateTime.Now.AddDays(1);
            model.Current = current;
            var result = controller.DateChanger(model);
            Assert.IsNotNull(result, "ViewResult should not be null.");
            mockUserService.VerifySet(d=>d.DateTime = current);
        }

        [TestMethod]
        public void DateChanger_WithPost_ReturnsViewResultWarning()
        {
            var controller = SystemUnderTest();
            var model = new DateTimeContext();
            var current = DateTime.Now;
            model.Current = current;
            var result = controller.DateChanger(model);
            Assert.IsNotNull(result, "ViewResult should not be null.");
            Assert.IsTrue(((ViewResult)result).TempData.Count==1);

            var values = ((ViewResult) result).TempData.Values;
            var e = values.GetEnumerator();
            e.MoveNext();
            var d = (Dictionary<string,List<string>>) e.Current;
            Assert.AreEqual("Date not been updated. Select a new date and Submit", d[""][0]);

            mockUserService.VerifySet(dv => dv.DateTime = current,Times.Never());
        }

        [TestMethod]
        public void DateChanger_WithPost_ReturnsViewResultError()
        {
            var controller = SystemUnderTest();
            var model = new DateTimeContext();
            var current = DateTime.Now;
            model.Current = current;

            controller.ModelState.AddModelError("key", @"date field error message"); // set an invalid model state

            var result = controller.DateChanger(model);
            Assert.IsNotNull(result, "ViewResult should not be null.");

            var values = controller.ModelState[""].Errors[0];
            Assert.AreEqual("The date entered was not recognised.  The date entered must be in the form DD/MM/YYYY", values.ErrorMessage);

            mockUserService.VerifySet(dv => dv.DateTime = current, Times.Never());
        }


        [TestMethod]
        public void DepartmentEmulateUser_ReturnsViewResult()
        {
            var controller = SystemUnderTest();
            var result = controller.DepartmentEmulateUser();
            Assert.IsNotNull(result);   
        }

        [TestMethod]
        public void DepartmentEmulateUser_ReturnsErrorViewResult()
        {
            var emulateUser = new DepartmentEmulateUser {JobNumber = "123", Reason = "Test", UserId = "XX001122"};
            var controller = SystemUnderTest();
            mockProvisionerService.Setup(e => e.EmulateAtNextLogon(It.IsAny<ProvisionerModel>())).Returns(false);

            var result = controller.DepartmentEmulateUser(emulateUser);
            Assert.IsNotNull(result);
            Assert.IsTrue(((ViewResult) result).ViewData.ModelState.Values.Count == 1);
            var modelState = ((ViewResult)result).ViewData.ModelState.Values.GetEnumerator();
            modelState.MoveNext();
            Assert.AreEqual("User Emulate Failed.", modelState.Current.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void DepartmentEmulateUser_ReturnsSuccessViewResult()
        {
            var emulateUser = new DepartmentEmulateUser {JobNumber = "123", Reason = "Test", UserId = "XX001122"};
            var controller = SystemUnderTest();
            mockProvisionerService.Setup(e => e.EmulateAtNextLogon(It.IsAny<ProvisionerModel>())).Returns(true);

            var result = controller.DepartmentEmulateUser(emulateUser);
            Assert.IsNotNull(result);
            Assert.IsNotNull(((System.Web.Mvc.ViewResultBase) (result)).Model);
            Assert.IsNotNull( ((DepartmentEmulateUser)((System.Web.Mvc.ViewResultBase)(result)).Model));

            Assert.AreEqual("123", ((DepartmentEmulateUser)((System.Web.Mvc.ViewResultBase)(result)).Model).JobNumber);
        }
    }
}