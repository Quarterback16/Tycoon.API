using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.Extensions
{
    /// <summary>
    ///This is a test class for ModelMetadataExtensionTest and is intended
    ///to contain all ModelMetadataExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ModelStateDictionaryExtensionTest
    {
        private class TestController : InfrastructureController
        {
            public const string PropertyName = "MyProperty";
            public const string Error1 = "Error one";
            public const string Error2 = "Error two";
            public const string Error3 = "Error three";

            public TestController( IUserService userService, IAdwService adwService)
                : base( userService, adwService)
            {
            }

            public ActionResult MergeMultipleDifferentErrorMessages()
            {
                var errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error2);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error3);

                ModelState.Merge(errors);

                var errorMessages = base.GetErrorMessages();
                return View(errorMessages);
            }

            public ActionResult MergeMultipleIdenticalErrorMessages()
            {
                var errors = new Dictionary<string, string>();

                errors.Add(string.Empty, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error1);

                ModelState.Merge(errors);

                var errorMessages = base.GetErrorMessages();
                return View(errorMessages);
            }

            public ActionResult MergeMultipleDifferentPropertyErrorMessages()
            {
                var errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error2);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error3);

                ModelState.Merge(errors);

                var errorMessages = base.GetErrorMessages(PropertyName);
                return View(errorMessages);
            }

            public ActionResult MergeMultipleIdenticalPropertyErrorMessages()
            {
                var errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(PropertyName, Error1);

                ModelState.Merge(errors);

                var errorMessages = base.GetErrorMessages(PropertyName);
                return View(errorMessages);
            }

            public ActionResult MergeMultipleDifferentGlobalAndPropertyErrorMessages()
            {
                var errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error1);
                errors.Add(PropertyName, Error1);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error2);
                errors.Add(PropertyName, Error2);

                ModelState.Merge(errors);

                errors = new Dictionary<string, string>();
                errors.Add(string.Empty, Error3);
                errors.Add(PropertyName, Error3);

                ModelState.Merge(errors);

                var errorMessages = base.GetErrorMessages().Concat(base.GetErrorMessages(PropertyName));
                return View(errorMessages);
            }
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }


        [TestMethod()]
        public void MergeMultipleDifferentErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestController( userService.Object, adwService.Object);

            var result = controller.MergeMultipleDifferentErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 3);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error1));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error2));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error3));
        }

        [TestMethod()]
        public void MergeMultipleIdenticalErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestController( userService.Object, adwService.Object);

            var result = controller.MergeMultipleIdenticalErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error1));
        }

        [TestMethod()]
        public void MergeMultipleDifferentPropertyErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestController( userService.Object, adwService.Object);

            var result = controller.MergeMultipleDifferentPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 3);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error1));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error2));
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error3));
        }

        [TestMethod()]
        public void MergeMultipleIdenticalPropertyErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestController( userService.Object, adwService.Object);

            var result = controller.MergeMultipleIdenticalPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 1);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Contains(TestController.Error1));
        }

        [TestMethod]
        public void MergeMultipleDifferentGlobalAndPropertyErrorMessages()
        {
            
            var userService = new Mock<IUserService>();
            var adwService = new Mock<IAdwService>();

            var controller = new TestController( userService.Object, adwService.Object);

            var result = controller.MergeMultipleDifferentGlobalAndPropertyErrorMessages() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count() == 6);

            // Should each have 2 because the error was added to global and property so aren't duplicates in this case (duplicates are those on the same property)
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count(c => c == TestController.Error1) == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count(c => c == TestController.Error2) == 2);
            Assert.IsTrue(((IEnumerable<string>)result.Model).Count(c => c == TestController.Error3) == 2);
        }
    }
}