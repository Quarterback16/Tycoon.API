using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Controllers
{
    /// <summary>
    ///This is a test class for InfrastructureControllerTest and is intended
    ///to contain all InfrastructureControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InfrastructureControllerTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }



        /// <summary>
        ///A test for InfrastructureController Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InfrastructureControllerConstructorTest1()
        {
            var adwService = new Mock<IAdwService>().Object;

            var target = new InfrastructureController( null, adwService);
            Assert.Fail("Expected Exception");
        }

        /// <summary>
        ///A test for InfrastructureController Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InfrastructureControllerConstructorTest2()
        {
            var userService = new Mock<IUserService>().Object;
            var adwService = new Mock<IAdwService>().Object;

            var target = new InfrastructureController( userService, null);
            Assert.Fail("Expected Exception");
        }

        private class TestDummyViewModel
        {
            /// <summary>
            /// Dummy ID.
            /// </summary>
            [Key]
            public long DummyID { get; set; }
        }

        private class TestPagedGridController : InfrastructureController
        {
            public const string PropertyName = "MyProperty";
            public const string SuccessMessage = "My success message";
            public const string InformationMessage = "My information message";
            public const string WarningMessage = "My warning message";
            public const string ErrorMessage = "My error message";

            public TestPagedGridController(IUserService userService, IAdwService adwService)
                : base( userService, adwService)
            {
            }

            public ActionResult DisplayInformationMessage()
            {
                this.AddInformationMessage("Unit test information");
                return View();
            }

            public ActionResult DisplayInformationPropertyMessage()
            {
                this.AddInformationMessage("AProperty","Unit test information");
                return View();
            }

            public ActionResult DisplaySuccess()
            {
                this.AddSuccessMessage("Unit test success");
                return View();
            }

            public ActionResult DisplayPropertySuccess()
            {
                this.AddSuccessMessage("AProperty","Unit test success");
                return View();
            }

            public ActionResult DisplayError()
            {
                this.AddErrorMessage("Unit Test Error Message");
                return View();
            }

            public ActionResult DisplayPropertyError()
            {
                this.AddErrorMessage("AProperty","Unit Test Error Message");
                return View();
            }

            public ActionResult DisplayPropertyWarning()
            {
                this.AddWarningMessage("AProperty", "Unit Test Property Warning Message");
                return View();
            }


            public ActionResult DisplayWarning()
            {
                this.AddWarningMessage("Unit test warning");
                return View();
            }

            public ActionResult TestingPaged()
            {
                var data = new Pageable<string>();
                data.Metadata = new PageMetadata();
                data.Metadata.ModelType = typeof (TestDummyViewModel);
                return PagedView(data);
            }

            #region Get Messages 

            public new ActionResult GetSuccessMessages()
            {
                base.AddSuccessMessage(SuccessMessage);
                var message = base.GetSuccessMessages();
                return View(message);
            }

            public ActionResult GetPropertySuccessMessages()
            {
                base.AddSuccessMessage(PropertyName, SuccessMessage);
                var message = base.GetSuccessMessages(PropertyName);
                return View(message);
            }

            public new ActionResult GetInformationMessages()
            {
                base.AddInformationMessage(InformationMessage);
                var message = base.GetInformationMessages();
                return View(message);
            }

            public ActionResult GetPropertyInformationMessages()
            {
                base.AddInformationMessage(PropertyName, InformationMessage);
                var message = base.GetInformationMessages(PropertyName);
                return View(message);
            }

            public new ActionResult GetWarningMessages()
            {
                base.AddWarningMessage(WarningMessage);
                var message = base.GetWarningMessages();
                return View(message);
            }

            public ActionResult GetPropertyWarningMessages()
            {
                base.AddWarningMessage(PropertyName, WarningMessage);
                var message = base.GetWarningMessages(PropertyName);
                return View(message);
            }

            public new ActionResult GetErrorMessages()
            {
                base.AddErrorMessage(ErrorMessage);
                var message = base.GetErrorMessages();
                return View(message);
            }

            public ActionResult GetPropertyErrorMessages()
            {
                base.AddErrorMessage(PropertyName, ErrorMessage);
                var message = base.GetErrorMessages(PropertyName);
                return View(message);
            }

            #endregion

            #region Has Messages for global

            public ActionResult HasInformationMessageTrue()
            {
                base.AddInformationMessage(InformationMessage);
                var has = base.HasInformationMessage(InformationMessage);
                return View(has);
            }

            public ActionResult HasInformationMessageFalse()
            {
                var has = base.HasInformationMessage(InformationMessage);
                return View(has);
            }

            public ActionResult HasSuccessMessageTrue()
            {
                base.AddSuccessMessage(SuccessMessage);
                var has = base.HasSuccessMessage(SuccessMessage);
                return View(has);
            }

            public ActionResult HasSuccessMessageFalse()
            {
                var has = base.HasSuccessMessage(SuccessMessage);
                return View(has);
            }

            public ActionResult HasWarningMessageTrue()
            {
                base.AddWarningMessage(WarningMessage);
                var has = base.HasWarningMessage(WarningMessage);
                return View(has);
            }

            public ActionResult HasWarningMessageFalse()
            {
                var has = base.HasWarningMessage(WarningMessage);
                return View(has);
            }

            public ActionResult HasErrorMessageTrue()
            {
                base.AddErrorMessage(ErrorMessage);
                var has = base.HasErrorMessage(ErrorMessage);
                return View(has);
            }

            public ActionResult HasErrorMessageFalse()
            {
                var has = base.HasErrorMessage(ErrorMessage);
                return View(has);
            }

            #endregion

            #region Has Messages for Property

            public ActionResult HasPropertyInformationMessageTrue()
            {
                base.AddInformationMessage(PropertyName, InformationMessage);
                var has = base.HasInformationMessage(PropertyName, InformationMessage);
                return View(has);
            }

            public ActionResult HasPropertyInformationMessageFalse()
            {
                var has = base.HasInformationMessage(PropertyName, InformationMessage);
                return View(has);
            }

            public ActionResult HasPropertySuccessMessageTrue()
            {
                base.AddSuccessMessage(PropertyName, SuccessMessage);
                var has = base.HasSuccessMessage(PropertyName, SuccessMessage);
                return View(has);
            }

            public ActionResult HasPropertySuccessMessageFalse()
            {
                var has = base.HasSuccessMessage(PropertyName, SuccessMessage);
                return View(has);
            }

            public ActionResult HasPropertyWarningMessageTrue()
            {
                base.AddWarningMessage(PropertyName, WarningMessage);
                var has = base.HasWarningMessage(PropertyName, WarningMessage);
                return View(has);
            }

            public ActionResult HasPropertyWarningMessageFalse()
            {
                var has = base.HasWarningMessage(PropertyName, WarningMessage);
                return View(has);
            }

            public ActionResult HasPropertyErrorMessageTrue()
            {
                base.AddErrorMessage(PropertyName, ErrorMessage);
                var has = base.HasErrorMessage(PropertyName, ErrorMessage);
                return View(has);
            }

            public ActionResult HasPropertyErrorMessageFalse()
            {
                var has = base.HasErrorMessage(PropertyName, ErrorMessage);
                return View(has);
            }

            #endregion

            #region Edge Cases

            public ActionResult AddNullPropertyErrorMessage()
            {
                base.AddErrorMessage(null, ErrorMessage);
                var has = base.HasErrorMessage(null, ErrorMessage);
                return View(has);
            }

            public ActionResult AddNullPropertyInformationMessage()
            {
                base.AddInformationMessage(null, InformationMessage);
                var has = base.HasInformationMessage(null, InformationMessage);
                return View(has);
            }

            public ActionResult AddMultipleDifferentInformationMessages()
            {
                base.AddInformationMessage(InformationMessage);
                base.AddInformationMessage(InformationMessage.Reverse().ToString());
                var messages = base.GetInformationMessages();
                return View(messages);
            }

            public ActionResult AddMultipleDifferentPropertyInformationMessages()
            {
                base.AddInformationMessage(PropertyName, InformationMessage);
                base.AddInformationMessage(PropertyName, InformationMessage.Reverse().ToString());
                var messages = base.GetInformationMessages(PropertyName);
                return View(messages);
            }

            public ActionResult AddMultipleIdenticalInformationMessages()
            {
                base.AddInformationMessage(InformationMessage);
                base.AddInformationMessage(InformationMessage);
                var messages = base.GetInformationMessages();
                return View(messages);
            }

            public ActionResult AddMultipleIdenticalPropertyInformationMessages()
            {
                base.AddInformationMessage(PropertyName, InformationMessage);
                base.AddInformationMessage(PropertyName, InformationMessage);
                var messages = base.GetInformationMessages(PropertyName);
                return View(messages);
            }

            public ActionResult AddMultipleDifferentErrorMessages()
            {
                base.AddErrorMessage(ErrorMessage);
                base.AddErrorMessage(ErrorMessage.Reverse().ToString());
                var messages = base.GetErrorMessages();
                return View(messages);
            }

            public ActionResult AddMultipleDifferentPropertyErrorMessages()
            {
                base.AddErrorMessage(PropertyName, ErrorMessage);
                base.AddErrorMessage(PropertyName, ErrorMessage.Reverse().ToString());
                var messages = base.GetErrorMessages(PropertyName);
                return View(messages);
            }

            public ActionResult AddMultipleIdenticalErrorMessages()
            {
                base.AddErrorMessage(ErrorMessage);
                base.AddErrorMessage(ErrorMessage);
                var messages = base.GetErrorMessages();
                return View(messages);
            }

            public ActionResult AddMultipleIdenticalPropertyErrorMessages()
            {
                base.AddErrorMessage(PropertyName, ErrorMessage);
                base.AddErrorMessage(PropertyName, ErrorMessage);
                var messages = base.GetErrorMessages(PropertyName);
                return View(messages);
            }

            #endregion
        }

        #region Edge Cases

        [TestMethod]
        public void InfrastructureController_AddNullPropertyErrorMessage()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddNullPropertyErrorMessage() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_AddNullPropertyInformationMessage()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddNullPropertyInformationMessage() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleDifferentInformationMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleDifferentInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage.Reverse().ToString()));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleDifferentPropertyInformationMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleDifferentPropertyInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage.Reverse().ToString()));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleIdenticalInformationMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleIdenticalInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleIdenticalPropertyInformationMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleIdenticalPropertyInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleDifferentErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleDifferentErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage.Reverse().ToString()));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleDifferentPropertyErrorMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleDifferentPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage.Reverse().ToString()));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleIdenticalErrorMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleIdenticalErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
        }

        [TestMethod]
        public void InfrastructureController_AddMultipleIdenticalPropertyErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.AddMultipleIdenticalPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
        }

        #endregion

        #region Get Messages

        [TestMethod]
        public void InfrastructureController_GetSuccessMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetSuccessMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.SuccessMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetPropertySuccessMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetPropertySuccessMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.SuccessMessage));
        }


        [TestMethod]
        public void InfrastructureController_GetInformationMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetPropertyInformationMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetPropertyInformationMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.InformationMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetWarningMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetWarningMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.WarningMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetPropertyWarningMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetPropertyWarningMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.WarningMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetErrorMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
        }

        [TestMethod]
        public void InfrastructureController_GetPropertyErrorMessages()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.GetPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestPagedGridController.ErrorMessage));
        }

        #endregion

        #region Has Messages for global

        [TestMethod]
        public void InfrastructureController_HasInformationMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasInformationMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasInformationMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasInformationMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }


        [TestMethod]
        public void InfrastructureController_HasSuccessMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasSuccessMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasSuccessMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasSuccessMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasWarningMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasWarningMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasWarningMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasWarningMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasErrorMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasErrorMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasErrorMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasErrorMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        #endregion

        #region Has Messages for Property

        [TestMethod]
        public void InfrastructureController_HasPropertyInformationMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyInformationMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertyInformationMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyInformationMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }


        [TestMethod]
        public void InfrastructureController_HasPropertySuccessMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertySuccessMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertySuccessMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertySuccessMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertyWarningMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyWarningMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertyWarningMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyWarningMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertyErrorMessage_Valid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyErrorMessageTrue() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsTrue((bool)result.Model);
        }

        [TestMethod]
        public void InfrastructureController_HasPropertyErrorMessage_Invalid()
        {
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestPagedGridController( userService.Object, adwService.Object);

            var result = controller.HasPropertyErrorMessageFalse() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelMetadata.ModelType == typeof(bool));
            Assert.IsFalse((bool)result.Model);
        }

        #endregion

        /// <summary>
        ///A test for InfrastructureController Constructor
        ///</summary>
        [TestMethod()]
        public void InfrastructureController_PagedViewTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();


            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.TestingPaged();

            //Assert
            Assert.IsTrue(target.ViewData.ContainsKey("PagedMetadata"));
            Assert.IsTrue(target.ViewData.ContainsKey("ParentModel"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InfrastructureController_AddErrorTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayError();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(target.ViewData.ModelState.Values.Count==1);
            var ms = target.ViewData.ModelState.Values.First();
            Assert.IsTrue(ms.Errors[0].ErrorMessage.Equals("Unit Test Error Message"));
        }

        [TestMethod]
        public void InfrastructureController_AddPropertyErrorTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayPropertyError();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(target.ViewData.ModelState.Values.Count == 1);
            var ms = target.ViewData.ModelState.Values.First();
            Assert.IsTrue(target.ViewData.ModelState.ContainsKey("AProperty"));
            Assert.IsTrue(ms.Errors[0].ErrorMessage.Equals("Unit Test Error Message"));
        }

        [TestMethod]
        public void InfrastructureController_AddWarningTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayWarning();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count==1);
            var td = target.TempData["Warning"];
            Assert.IsNotNull(td);
            var t = (((Dictionary<string, List<string>>) (td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit test warning", w[0]);
        }

        [TestMethod]
        public void InfrastructureController_AddPropertyWarningTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayPropertyWarning();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count == 1);
            var td = target.TempData["Warning"];
            Assert.IsNotNull(td);
            var d = ((Dictionary<string, List<string>>) td).First();
            Assert.AreEqual("AProperty",d.Key);

            var t = (((Dictionary<string, List<string>>)(td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit Test Property Warning Message", w[0]);
        }

        [TestMethod]
        public void InfrastructureController_AddSuccessTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplaySuccess();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count == 1);
            var td = target.TempData["Success"];
            Assert.IsNotNull(td);
            var d = ((Dictionary<string, List<string>>)td).First();

            var t = (((Dictionary<string, List<string>>)(td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit test success", w[0]);
        }
 

        [TestMethod]
        public void InfrastructureController_AddPropertySuccessTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayPropertySuccess();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count == 1);
            var td = target.TempData["Success"];
            Assert.IsNotNull(td);
            var d = ((Dictionary<string, List<string>>)td).First();
            Assert.AreEqual("AProperty", d.Key);

            var t = (((Dictionary<string, List<string>>)(td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit test success", w[0]);
        }

        [TestMethod]
        public void InfrastructureController_AddInformationTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayInformationMessage();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count == 1);
            var td = target.TempData["Information"];
            Assert.IsNotNull(td);
            var d = ((Dictionary<string, List<string>>)td).First();

            var t = (((Dictionary<string, List<string>>)(td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit test information", w[0]);
        }

        [TestMethod]
        public void InfrastructureController_DisplayInformationPropertyTest()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var target = new TestPagedGridController( userService.Object, adwService.Object);
            Assert.IsNotNull(target);

            //Act
            ActionResult result = target.DisplayInformationPropertyMessage();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(target.TempData);
            Assert.IsTrue(target.TempData.Count == 1);
            var td = target.TempData["Information"];
            Assert.IsNotNull(td);
            var d = ((Dictionary<string, List<string>>)td).First();
            Assert.AreEqual("AProperty", d.Key);

            var t = (((Dictionary<string, List<string>>)(td))).Values;
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Count == 1);
            var w = t.First();
            Assert.AreEqual("Unit test information", w[0]);
        }
    }
}